namespace Donatello.Ast
{
	class SymbolUntypedExpression : IExpression
    {
        public SymbolUntypedExpression(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
