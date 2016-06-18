using Antlr4.Runtime.Misc;
using Donatello.Antlr.Generated;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.Parser
{
    public partial class ParseExpressionVisitor : DonatelloBaseVisitor<CSharpSyntaxNode>
    {
        public override CSharpSyntaxNode VisitType([NotNull] DonatelloParser.TypeContext context)
        {
            var type = context.GetText().Substring(1); // given `:int`, we want `int`
            return type == "void" ?
                PredefinedType(Token(SyntaxKind.VoidKeyword)) :
                ParseTypeName(type);
        }
    }
}
