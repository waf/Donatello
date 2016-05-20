using Antlr4.Runtime.Misc;
using DotNetLisp.Antlr.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace DotNetLisp.Parser
{
    public partial class ParseExpressionVisitor : DotNetLispBaseVisitor<CSharpSyntaxNode>
    {
        private readonly string ClassName;
        private readonly string NamespaceName;
        private readonly string MainMethodName;
        private readonly SyntaxToken[] PublicStatic = { Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword) };

        public ParseExpressionVisitor(string namespaceName, string className, string mainMethodName)
        {
            this.NamespaceName = namespaceName;
            this.ClassName = className;
            this.MainMethodName = mainMethodName;
        }

        public override CSharpSyntaxNode VisitFile([NotNull] DotNetLispParser.FileContext context)
        {
            var children = context.form().Select(f => this.Visit(f)).ToArray();
            var usings = children.OfType<UsingDirectiveSyntax>().ToArray();
            var fields = children.OfType<FieldDeclarationSyntax>().Select(field => field.AddModifiers(PublicStatic)).ToList();
            var methods = children.OfType<MethodDeclarationSyntax>().Select(field => field.AddModifiers(PublicStatic)).ToList();
            var expressions = children.OfType<ExpressionSyntax>().ToArray();

            if(expressions.Any())
            {
                int finalElement = expressions.Length - 1;
                var statements = expressions
                    .Select((expression, index) =>
                                index == finalElement ?
                                ReturnStatement(expression as ExpressionSyntax) :
                                ExpressionStatement(expression as ExpressionSyntax) as StatementSyntax)
                    .ToArray();
                // make a Program class that has a "Run" method, and embed our program expression inside it.
                methods.Add(
                    MethodDeclaration(ParseTypeName("System.Object"), MainMethodName)
                           .AddModifiers(PublicStatic)
                           .WithBody(Block(statements))
                );
            }

            var @class = CompilationUnit()
                .AddUsings(usings)
                .AddMembers(NamespaceDeclaration(IdentifierName(NamespaceName))
                    .AddMembers(ClassDeclaration(ClassName)
                        .AddMembers(fields.ToArray())
                        .AddMembers(methods.ToArray())
                        .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))));

            return @class;
        }
    }
}
