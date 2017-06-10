using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Reflection;
using Xunit;
using static Donatello.Tests.Integration.TestExtensions;

namespace Donatello.Tests.Integration
{
    public class MacroTests
    {
        static string[] references = new[]
        {
            typeof(SyntaxNode).GetTypeInfo().Assembly.Location,
            typeof(SyntaxFactory).GetTypeInfo().Assembly.Location,
        };

        [Fact]
        public void MacroTest()
        {
            const string code = @"
                (use Microsoft.CodeAnalysis.CSharp)
                (use Microsoft.CodeAnalysis.CSharp.Syntax)
                (use Microsoft.CodeAnalysis.CSharp.SyntaxFactory.*)

                (defmacro unless [test:ExpressionSyntax
                                  thenBranch:ExpressionSyntax
                                  elseBranch:ExpressionSyntax]
                          :CSharpSyntaxNode
                    (ParenthesizedExpression
                        (ConditionalExpression
                            (PrefixUnaryExpression SyntaxKind.LogicalNotExpression test)
                            thenBranch
                            elseBranch)))
                (unless false 1 0)
            ";
            AssertOutput(code, 1, references);
        }
    }
}
    /* Macro syntax ideas for IParseTree manipulation?
       (defmacro unless [condition:IParseTree
                         falseBranch:IParseTree
                         trueBranch:IParseTree]
                 :IParseTree
            (let [list (new ListContext)
                  not  (new ListContext)]
                (.AddChild list (quote ""if""))
                (.AddChild list not)
                (.AddChild not (quote ""not""))
                (.AddChild not condition)
                (.AddChild list falseBranch)
                (.AddChild list trueBranch)))

       (defmacro unless [condition:IParseTree
                         falseBranch:IParseTree
                         trueBranch:IParseTree]
                 :IParseTree
        (quote (if (not ~condition)
             ~falseBranch
             ~trueBranch))
       (unless false 1 0)
   */
