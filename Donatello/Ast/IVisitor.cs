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

    interface IVisitor<T>
    {
        T Visit(FloatExpression expr);
        T Visit(LongExpression expr);
        T Visit(BooleanExpression expr);
        T Visit(StringExpression expr);
        T Visit(SymbolExpression expr);
        T Visit(ListExpression expr);
        T Visit(VectorExpression expr);
        T Visit(SetExpression expr);
        T Visit(DefExpression expr);
        T Visit(FunctionExpression expr);
        T Visit(FileExpression expr);
    }
}
