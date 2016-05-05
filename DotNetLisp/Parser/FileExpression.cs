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

namespace DotNetLisp.Parser
{
    public partial class ParseExpressionVisitor : DotNetLispBaseVisitor<CSharpSyntaxNode>
    {
        private readonly string ClassName;
        private readonly string NamespaceName;

        public ParseExpressionVisitor(string namespaceName, string className) :base()
        {
            this.NamespaceName = namespaceName;
            this.ClassName = className;
        }

        public override CSharpSyntaxNode VisitFile([NotNull] DotNetLispParser.FileContext context)
        {
            var children = context.form().Select(f => this.Visit(f)).ToArray();
            var methods = children.OfType<MethodDeclarationSyntax>().ToArray();

            if(!methods.Any())
            {
                // make a Program class that has a "Run" method, and embed our program expression inside it.
                methods = new[]
                {
                    MethodDeclaration(ParseTypeName("System.Object"), "Run")
                           .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
                           .WithBody(Block(ReturnStatement(children.First() as ExpressionSyntax)))
                };
            }

            var @class = CompilationUnit()
                .AddMembers(NamespaceDeclaration(IdentifierName(NamespaceName))
                    .AddMembers(ClassDeclaration(ClassName)
                        .AddMembers(methods)));

            return @class;
        }
    }
}
