using Antlr4.Runtime.Misc;
using DotNetLisp.Antlr.Generated;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DotNetLisp.Parser
{
    public partial class ParseExpressionVisitor : DotNetLispBaseVisitor<CSharpSyntaxNode>
    {
        readonly string ClassName;
        readonly string NamespaceName;
        readonly string MainMethodName;

        readonly SyntaxToken[] Public  = { Token(SyntaxKind.PublicKeyword) };
        readonly SyntaxToken[] Static = { Token(SyntaxKind.StaticKeyword) };
        readonly SyntaxToken[] PublicStatic = { Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword) };
        readonly SyntaxToken[] PublicStaticReadonly = { Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.ReadOnlyKeyword) };

        public ParseExpressionVisitor(string namespaceName, string className)
        {
            this.NamespaceName = namespaceName;
            this.ClassName = className;
        }

        public ParseExpressionVisitor(string namespaceName, string className, string mainMethodName)
            :this(namespaceName, className)
        {
            this.MainMethodName = mainMethodName;
        }

        public override CSharpSyntaxNode VisitFile([NotNull] DotNetLispParser.FileContext context)
        {
            var children = context.form().Select(f => this.Visit(f)).ToArray();
            var usings = children.OfType<UsingDirectiveSyntax>().ToArray();
            var baseTypes = children.OfType<SimpleBaseTypeSyntax>().ToArray();

            // the existance of base types means that the user is doing some .NET interop
            bool hasBaseTypes = baseTypes.Any();

            var expressions = children.OfType<ExpressionSyntax>().ToArray();
            var fields = children.OfType<FieldDeclarationSyntax>()
                .Select(field => field.AddModifiers(PublicStaticReadonly))
                .ToArray();
            var methods = children.OfType<MethodDeclarationSyntax>()
                .Select(method => method.AddModifiers(hasBaseTypes ? Public : PublicStatic) as MemberDeclarationSyntax)
                .ToList();

            if(expressions.Any())
            {
                // embed any free-standing expressions in the constructor
                var entryPoint = MainMethodName == null ?
                    CreateConstructor(expressions, hasBaseTypes) :
                    CreateMethod(expressions, MainMethodName);
                methods.Add(entryPoint);
            }

            var classDeclaration = ClassDeclaration(ClassName);

            if(hasBaseTypes)
            {
                classDeclaration = classDeclaration.AddBaseListTypes(baseTypes);
            }
                
            var program = CompilationUnit()
                .AddUsings(usings)
                .AddMembers(NamespaceDeclaration(IdentifierName(NamespaceName))
                    .AddMembers(classDeclaration
                        .AddMembers(fields)
                        .AddMembers(methods.ToArray())
                        .AddModifiers(hasBaseTypes ? Public : PublicStatic)));

            return program;
        }

        private BaseMethodDeclarationSyntax CreateMethod(ExpressionSyntax[] expressions, string mainMethodName)
        {
            int finalElement = expressions.Length - 1;
            var statements = expressions.Select((expression, index) =>
                        index == finalElement ?
                        ReturnStatement(expression) :
                        ExpressionStatement(expression) as StatementSyntax).ToArray();
            return MethodDeclaration(ParseTypeName("System.Object"), mainMethodName)
                       .AddModifiers(PublicStatic)
                       .WithBody(Block(statements));
        }

        private BaseMethodDeclarationSyntax CreateConstructor(ExpressionSyntax[] expressions, bool hasBaseTypes)
        {
            var statements = expressions
                .Select(expression => ExpressionStatement(expression))
                .ToArray();
            return ConstructorDeclaration(ClassName)
                    .AddModifiers(hasBaseTypes ? Public : Static)
                    .WithBody(Block(statements));
        }
    }
}
