using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetLisp.Util
{
    static class Extensions
    {
        public static IEnumerable<IParseTree> Children(this IParseTree tree)
        {
            for (int i = 0; i < tree.ChildCount; i++)
            {
                yield return tree.GetChild(i);
            }
        }
    }
}
