using Antlr4.Runtime.Tree;
using Donatello.StandardLibrary;
using Donatello.Util;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Donatello
{
    internal static class BuiltInFunctions
    {
        delegate CSharpSyntaxNode BuiltIn(IParseTreeVisitor<CSharpSyntaxNode> visitor, IList<IParseTree> children);

        static readonly IDictionary<string, BuiltIn> BuiltIns = new Dictionary<string, BuiltIn>
        {
            { "def", Def },
            { "defn", Defn },
            { "fn", Fn },
            { "if", If },
            { "let", Let },
            { "use", Use },
            { "instance", Instance },
            { "new", New },
            { "+", Add },
            { "-", Subtract },
            { "*", Multiply },
            { "/", Divide },
        };

        internal static CSharpSyntaxNode Run(
            IParseTreeVisitor<CSharpSyntaxNode> visitor,
            IList<IParseTree> children)
        {
            string name = children[0].GetText();
            BuiltIn builtIn = null;
            if(!BuiltIns.TryGetValue(name, out builtIn))
            {
                return null;
            }
            return builtIn(visitor, children);
        }

        private static CSharpSyntaxNode New(IParseTreeVisitor<CSharpSyntaxNode> visitor, IList<IParseTree> children)
        {
            var type = children[1].GetText();
            var constructorParameters = children
                .Skip(2)
                .Select(child => Argument(visitor.Visit(child) as ExpressionSyntax));
            return ObjectCreationExpression(IdentifierName(type))
                .WithArgumentList(ArgumentList(SeparatedList(constructorParameters)));
        }

        private static CSharpSyntaxNode Instance(IParseTreeVisitor<CSharpSyntaxNode> visitor, IList<IParseTree> children)
        {
            var baseTypesAndInterfaces = children
                .Skip(1)
                .Select(child => SimpleBaseType(ParseTypeName(child.GetText())))
                .ToArray();
            return BaseList(SeparatedList<BaseTypeSyntax>(baseTypesAndInterfaces));
        }

        private static CSharpSyntaxNode Fn(IParseTreeVisitor<CSharpSyntaxNode> visitor, IList<IParseTree> children)
        {
            /*
                (fn [a b] (+ a b))
             */

            var bindings = children[1].GetChild(0).Children().ToList();
            var parameters = bindings.Skip(1).Take(bindings.Count - 2)
                                    .Select(var => Parameter(Identifier(var.GetText())));
            var expressions = children.Skip(2).Select(statement => visitor.Visit(statement)).ToArray();
            int finalElement = expressions.Length - 1;
            var statements = expressions
                .Select((expression, index) =>
                            index == finalElement ?
                            ReturnStatement(expression as ExpressionSyntax) :
                            ExpressionStatement(expression as ExpressionSyntax) as StatementSyntax)
                .ToArray();
            return ParenthesizedLambdaExpression(Block(statements))
                    .WithParameterList(ParameterList(SeparatedList(parameters)));
        }

        private static CSharpSyntaxNode Let(IParseTreeVisitor<CSharpSyntaxNode> visitor, IList<IParseTree> children)
        {
            var bindings = children[1].GetChild(0);
            var expressions = children.Skip(2).Select(statement => visitor.Visit(statement)).ToArray();
            int finalElement = expressions.Length - 1;
            var statements = expressions
                .Select((expression, index) =>
                            index == finalElement ?
                            ReturnStatement(expression as ExpressionSyntax) :
                            ExpressionStatement(expression as ExpressionSyntax) as StatementSyntax)
                .ToArray();

            List<StatementSyntax> variables = PairwiseListVisit<StatementSyntax>(bindings, (name, value) =>
            {
                return LocalDeclarationStatement(
                        VariableDeclaration(IdentifierName("var"))
                        .WithVariables(SingletonSeparatedList(
                            VariableDeclarator(Identifier(name.GetText()))
                            .WithInitializer(EqualsValueClause(visitor.Visit(value) as ExpressionSyntax)))));
            });

            var lambda = ParenthesizedLambdaExpression(Block(variables.Union(statements)));

            return InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(nameof(Constructors)),
                        IdentifierName(nameof(Constructors.CreateLet))))
                    .WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(lambda))));
        }

        private static List<T> PairwiseListVisit<T>(IParseTree tree, Func<IParseTree, IParseTree, T> pairwiseOperation)
        {
            var list = new List<T>();
            for (int i = 1; i < tree.ChildCount - 1; i += 2)
            {
                list.Add(
                    pairwiseOperation(tree.GetChild(i), tree.GetChild(i + 1))
                );
            }

            return list;
        }

        private static CSharpSyntaxNode Use(IParseTreeVisitor<CSharpSyntaxNode> visitor, IList<IParseTree> children)
        {
            var target = children[1].GetText().Split('.');
            if(target.Last() == "*")
            {
                return UsingDirective(
                    Token(SyntaxKind.StaticKeyword),
                    null,
                    ParseName(string.Join(".", target.Take(target.Length - 1))));
            }
            return UsingDirective(
                NameEquals(IdentifierName(target.Last()),
                Token(SyntaxKind.EqualsToken)),
                ParseName(string.Join(".", target)));
        }

        private static CSharpSyntaxNode Defn(
            IParseTreeVisitor<CSharpSyntaxNode> visitor,
            IList<IParseTree> children)
        {
            var methodName = children[1].GetText();
            var parameters = children[2].GetChild(0);

            IList<ParameterSyntax> parameterList = PairwiseListVisit(parameters, (name, type) =>
            {
                return Parameter(Identifier(name.GetText()))
                    .WithType(visitor.Visit(type) as TypeSyntax);
            });

            var returnType = visitor.Visit(children[3]) as TypeSyntax;
            var statements = children.Skip(4).Select(statement => visitor.Visit(statement)).ToArray();
            int finalElement = statements.Length - 1;
            var body = statements
                .Select((expression, index) =>
                            index == finalElement && !IsVoid(returnType) ?
                            ReturnStatement(expression as ExpressionSyntax) :
                            ExpressionStatement(expression as ExpressionSyntax) as StatementSyntax)
                .ToArray();
            return MethodDeclaration(returnType, methodName)
                    .WithParameterList(ParameterList(SeparatedList(parameterList)))
                    .WithBody(Block(body));
        }

        private static bool IsVoid(TypeSyntax returnType)
        {
            var voidType = returnType as PredefinedTypeSyntax;
            return voidType != null && voidType.Keyword.Kind() == SyntaxKind.VoidKeyword;
        }

        private static CSharpSyntaxNode If(
            IParseTreeVisitor<CSharpSyntaxNode> visitor,
            IList<IParseTree> children)
        {
            // (if condition then-statement else-statement)
            var condition = visitor.Visit(children[1]) as ExpressionSyntax;
            var thenStatement = visitor.Visit(children[2]) as ExpressionSyntax;
            var elseStatement = visitor.Visit(children[3]) as ExpressionSyntax;
            return ConditionalExpression(condition, thenStatement, elseStatement);
        }

        private static CSharpSyntaxNode Def(
            IParseTreeVisitor<CSharpSyntaxNode> visitor,
            IList<IParseTree> children)
        {
            // (def a:int 5)
            var name = children[1].GetText();
            var type = visitor.Visit(children[2]) as TypeSyntax;
            var value = visitor.Visit(children[3]) as ExpressionSyntax;
            return FieldDeclaration(
                VariableDeclaration(type, SingletonSeparatedList(
                    VariableDeclarator(name).WithInitializer(EqualsValueClause(value)))));
        }

        private static CSharpSyntaxNode Add(
            IParseTreeVisitor<CSharpSyntaxNode> visitor,
            IList<IParseTree> children)
        {
            // (+ a b c ...)
            return MathOperation(SyntaxKind.AddExpression, visitor, children);
        }

        private static CSharpSyntaxNode Subtract(IParseTreeVisitor<CSharpSyntaxNode> visitor, IList<IParseTree> children)
        {
            // (- a b c ...)
            return MathOperation(SyntaxKind.SubtractExpression, visitor, children);
        }

        private static CSharpSyntaxNode Multiply(IParseTreeVisitor<CSharpSyntaxNode> visitor, IList<IParseTree> children)
        {
            // (* a b c ...)
            return MathOperation(SyntaxKind.MultiplyExpression, visitor, children);
        }

        private static CSharpSyntaxNode Divide(IParseTreeVisitor<CSharpSyntaxNode> visitor, IList<IParseTree> children)
        {
            // (/ a b c ...)
            return MathOperation(SyntaxKind.DivideExpression, visitor, children);
        }

        private static CSharpSyntaxNode MathOperation(SyntaxKind operation, IParseTreeVisitor<CSharpSyntaxNode> visitor, IList<IParseTree> children)
        {
            var values = children.Skip(1).Select(child => visitor.Visit(child) as ExpressionSyntax);
            return values.Aggregate((a, b) => BinaryExpression(operation, a, b));
        }

    }
}
