using Donatello.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.Services
{
    abstract class AstOperation<TReturn>
    {
        public TReturn Apply(ITypedExpression expression)
        {
            switch (expression)
            {
                case BooleanExpression booleanLiteral: return BooleanLiteral(booleanLiteral);
                case LongExpression longLiteral: return LongLiteral(longLiteral);
                case FloatExpression floatLiteral: return FloatLiteral(floatLiteral);
                case StringExpression stringLiteral: return StringLiteral(stringLiteral);
                case FileExpression file: return File(file);
                case DefExpression def: return Def(def);
                case SymbolExpression symbol: return Symbol(symbol);
                case FunctionExpression function: return Function(function);
                case ListExpression list: return List(list);
                case VectorExpression vector: return Vector(vector);
                case SetExpression set: return Set(set);
                default: throw new ArgumentException("unexpected type " + expression.GetType().Name);
            }
        }

        protected abstract TReturn Set(SetExpression set);
        protected abstract TReturn Vector(VectorExpression vector);
        protected abstract TReturn List(ListExpression list);
        protected abstract TReturn Function(FunctionExpression function);
        protected abstract TReturn Symbol(SymbolExpression symbol);
        protected abstract TReturn Def(DefExpression def);
        protected abstract TReturn File(FileExpression file);
        protected abstract TReturn StringLiteral(StringExpression stringLiteral);
        protected abstract TReturn FloatLiteral(FloatExpression floatLiteral);
        protected abstract TReturn LongLiteral(LongExpression longLiteral);
        protected abstract TReturn BooleanLiteral(BooleanExpression booleanLiteral);
    }

    abstract class UntypedAstOperation<TReturn>
    {
        public TReturn Apply(IExpression expression)
        {
            switch (expression)
            {
                case BooleanExpression booleanLiteral: return BooleanLiteral(booleanLiteral);
                case LongExpression longLiteral: return LongLiteral(longLiteral);
                case FloatExpression floatLiteral: return FloatLiteral(floatLiteral);
                case StringExpression stringLiteral: return StringLiteral(stringLiteral);
                case FileUntypedExpression file: return File(file);
                case DefUntypedExpression def: return Def(def);
                case SymbolUntypedExpression symbol: return Symbol(symbol);
                case FunctionUntypedExpression function: return Function(function);
                case ListUntypedExpression list: return List(list);
                case VectorUntypedExpression vector: return Vector(vector);
                case SetUntypedExpression set: return Set(set);
                default: throw new ArgumentException("unexpected type " + expression.GetType().Name);
            }
        }

        protected abstract TReturn Set(SetUntypedExpression set);
        protected abstract TReturn Vector(VectorUntypedExpression vector);
        protected abstract TReturn List(ListUntypedExpression list);
        protected abstract TReturn Function(FunctionUntypedExpression function);
        protected abstract TReturn Symbol(SymbolUntypedExpression symbol);
        protected abstract TReturn Def(DefUntypedExpression def);
        protected abstract TReturn File(FileUntypedExpression file);
        protected abstract TReturn StringLiteral(StringExpression stringLiteral);
        protected abstract TReturn FloatLiteral(FloatExpression floatLiteral);
        protected abstract TReturn LongLiteral(LongExpression longLiteral);
        protected abstract TReturn BooleanLiteral(BooleanExpression booleanLiteral);
    }
}
