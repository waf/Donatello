using System.Collections.Generic;
using System.Linq.Expressions;

namespace DotNetLisp
{
    internal class Scope
    {
        public IDictionary<string, Expression> Variables { get; }
            = new Dictionary<string, Expression>();

        //TODO: scope resolution chain
    }
}