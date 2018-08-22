using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Donatello.Ast;

namespace Donatello.Services
{
	class SymbolLinker
	{
		public static object Link(ITypedExpression typedAst)
		{
			// traverse ast, looking up unknown symbols and constraint solving for the types.
			// notes: we do this for only external symbols, or all symbols so the compiler has semantic understanding.
			throw new NotImplementedException();
		}
	}
}
