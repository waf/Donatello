using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.StandardLibrary
{
    public static class Constructors
    {
        /// <summary>
        /// Creates a KeyValuePair with inferred types.
        /// </summary>
        /// <remarks>
        /// Type inference does not work with constructors, so we need this static method if want to create
        /// key value pairs without specifying the types.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] // this seems like a good idea, but there hasn't been any perf testing.
        public static KeyValuePair<TKey, TValue> CreateKeyValuePair<TKey, TValue>(TKey key, TValue value)
        {
            return new KeyValuePair<TKey, TValue>(key, value);
        }

        /// <summary>
        /// Execute a Let function with inferred types.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] // this seems like a good idea, but there hasn't been any perf testing.
        public static T CreateLet<T>(Func<T> letFunc)
        {
            return letFunc();
        }

        /// <summary>
        /// Execute a Let function with inferred types.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] // this seems like a good idea, but there hasn't been any perf testing.
        public static T CreateLet<T>(Action letFunc)
        {
            letFunc();
            return default(T);
        }
    }
}
