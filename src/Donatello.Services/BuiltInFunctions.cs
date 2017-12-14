using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Tree;
using Microsoft.CodeAnalysis.CSharp;
using Donatello.Services.Parser;
using Donatello.Services.BuiltIns;

namespace Donatello.Services
{
    internal static class BuiltInFunctions
    {
        static readonly IReadOnlyDictionary<string, IBuiltIn> BuiltIns = new Dictionary<string, IBuiltIn>
        {
            { "def", new Def() },
            { "defn", new Defn() },
            { "defmacro", new DefMacro() },
            { "fn", new Fn() },
            { "if", new If() },
            { "let", new Let() },
            { "use", new Use() },
            { "usemacro", new UseMacro() },
            { "instance", new Instance() },
            { "new", new New() },
            { "pipe", new Pipe(Enumerable.Append) },
            { "pipel", new Pipe(Enumerable.Append) },
            { "pipef", new Pipe(Enumerable.Prepend) },
            { "+", new MathOperation(SyntaxKind.AddExpression) },
            { "-", new MathOperation(SyntaxKind.SubtractExpression) },
            { "*", new MathOperation(SyntaxKind.MultiplyExpression) },
            { "/", new MathOperation(SyntaxKind.DivideExpression) },
            { "%", new MathOperation(SyntaxKind.ModuloExpression) },
            { "<", new EqualityOperation(SyntaxKind.LessThanExpression) },
            { ">", new EqualityOperation(SyntaxKind.GreaterThanExpression) },
            { "<=", new EqualityOperation(SyntaxKind.LessThanOrEqualExpression) },
            { ">=", new EqualityOperation(SyntaxKind.GreaterThanOrEqualExpression) },
            { "=", new EqualityOperation(SyntaxKind.EqualsExpression) },
        };

        internal static CSharpSyntaxNode Run(ParseExpressionVisitor visitor, IList<IParseTree> children)
        {
            string name = children[0].GetText();
            if (!BuiltIns.TryGetValue(name, out var builtIn))
            {
                return null;
            }
            return builtIn.Invoke(visitor, children);
        }
    }
}
