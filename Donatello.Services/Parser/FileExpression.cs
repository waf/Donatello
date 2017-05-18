using Antlr4.Runtime.Misc;
using Donatello.Services.Antlr.Generated;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Donatello.Services.Parser
{
    public partial class ParseExpressionVisitor : DonatelloBaseVisitor<CSharpSyntaxNode>
    {
        public readonly string ClassName;
        public readonly string NamespaceName;
        public readonly string MainMethodName;

        readonly SyntaxToken[] Public  = { Token(SyntaxKind.PublicKeyword) };
        readonly SyntaxToken[] Static = { Token(SyntaxKind.StaticKeyword) };
        readonly SyntaxToken[] PublicStatic = { Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword) };
        readonly SyntaxToken[] PublicStaticReadonly = { Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.ReadOnlyKeyword) };
        private readonly bool repl;

        public ParseExpressionVisitor(bool repl)
        {
            this.repl = repl;
            //TODO: set namespace name and class name to some default?
        }

        public ParseExpressionVisitor(string namespaceName, string className)
            :this(false)
        {
            this.NamespaceName = namespaceName;
            this.ClassName = className;
        }

        public ParseExpressionVisitor(string namespaceName, string className, string mainMethodName)
            :this(namespaceName, className)
        {
            this.MainMethodName = mainMethodName;
        }

        public override CSharpSyntaxNode VisitFile([NotNull] DonatelloParser.FileContext context)
        {
            var children = context.form()
                .Select(f => this.Visit(f))
                .ToArray();

            if (repl)
            {
                return children.First();
            }

            return CreateClass(children);
        }

        private CSharpSyntaxNode CreateClass(CSharpSyntaxNode[] children)
        {
            var usings = children.OfType<UsingDirectiveSyntax>().ToArray();
            var baseTypes = children.OfType<BaseListSyntax>().SingleOrDefault();

            // the existance of base types means that the user is doing some .NET interop
            bool isInstance = baseTypes != null;

            var expressions = children.OfType<ExpressionSyntax>().ToArray();
            var fields = children.OfType<FieldDeclarationSyntax>()
                .Select(field => field.AddModifiers(PublicStaticReadonly))
                .ToArray();
            var methods = children.OfType<MethodDeclarationSyntax>()
                .Select(method => method.AddModifiers(isInstance ? Public : PublicStatic) as MemberDeclarationSyntax)
                .ToList();

            if (expressions.Any())
            {
                // embed any free-standing expressions in the constructor
                var entryPoint = MainMethodName == null ?
                    CreateConstructor(expressions, isInstance) :
                    CreateMethod(expressions, MainMethodName);
                methods.Add(entryPoint);
            }

            var classDeclaration = ClassDeclaration(ClassName);

            if (isInstance)
            {
                classDeclaration = classDeclaration.WithBaseList(baseTypes);
            }

            var program = CompilationUnit()
                .AddUsings(usings)
                .AddMembers(NamespaceDeclaration(IdentifierName(NamespaceName))
                    .AddMembers(classDeclaration
                        .AddMembers(fields)
                        .AddMembers(methods.ToArray())
                        .AddModifiers(isInstance ? Public : PublicStatic)));

            return program;
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
    }
}
