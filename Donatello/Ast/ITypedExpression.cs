using Donatello.TypeInference;

namespace Donatello.Ast
{
    internal interface ITypedExpression : IExpression
    {
        IType Type { get; }
        T Accept<T>(IVisitor<T> visitor);
        void Accept(IVisitor visitor);
    }
}