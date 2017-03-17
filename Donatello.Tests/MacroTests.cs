using System;
using Xunit;
using static Donatello.Tests.TestExtensions;

namespace DotNetLisp.Tests
{
    public class MacroTests
    {
        [Fact]
        public void MacroTest()
        {
            const string code = @"
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
            AssertOutput(code, 1);
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
