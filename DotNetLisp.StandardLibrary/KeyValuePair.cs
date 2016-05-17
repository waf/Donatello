using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetLisp.StandardLibrary
{
    public static class KeyValuePair
    {
        /// <summary>
        /// Creates a KeyValuePair with inferred types.
        /// </summary>
        /// <remarks>
        /// Type inference does not work with constructors, so we need this static method if want to create
        /// key value pairs without specifying the types.
        /// </remarks>
        public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value)
        {
            return new KeyValuePair<TKey, TValue>(key, value);
        }
    }
}
