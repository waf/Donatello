using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.Ast
{
    interface IVisitor
    {
        void Visit(FloatExpression expr);
        void Visit(DefTypeExpression expr);
        void Visit(LongExpression expr);
        void Visit(BooleanExpression expr);
        void Visit(StringExpression expr);
        void Visit(SymbolExpression expr);
        void Visit(ListExpression expr);
        void Visit(VectorExpression expr);
        void Visit(SetExpression expr);
        void Visit(DefExpression expr);
        void Visit(FunctionExpression expr);
        void Visit(FileExpression expr);
    }

    interface IVisitor<TReturn>
    {
        TReturn Visit(FloatExpression expr);
        TReturn Visit(LongExpression expr);
        TReturn Visit(BooleanExpression expr);
        TReturn Visit(StringExpression expr);
        TReturn Visit(SymbolExpression expr);
        TReturn Visit(ListExpression expr);
        TReturn Visit(VectorExpression expr);
        TReturn Visit(SetExpression expr);
        TReturn Visit(DefExpression expr);
        TReturn Visit(DefTypeExpression expr);
        TReturn Visit(FunctionExpression expr);
        TReturn Visit(FileExpression expr);
    }
}
