using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace UnitTest
{
    public static class MacroTest
    {
        public static CSharpSyntaxNode unless(ExpressionSyntax test, 
            ExpressionSyntax thenBranch, 
            ExpressionSyntax elseBranch)
        {
            return ParenthesizedExpression(
                ConditionalExpression(
                    PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, test), 
                    thenBranch, 
                    elseBranch));
        }

        static MacroTest()
        {
            (!false ? 1 : 0);
        }
    }
}