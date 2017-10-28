using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Scripting.Hosting;

namespace Donatello.StandardLibrary
{
    public static class ReplPrinter
    {
        public static void Print(object obj)
        {
            if (obj == null)
            {
                Console.WriteLine("null");
                return;
            }

            var (type, value) = PrettyPrintObject(obj);
            Console.WriteLine(value + " :" + type);
        }

        private static (string type, string value) PrettyPrintObject(object obj)
        {
            //TODO: can we subclass our own ObjectFormatter, rather than using C#'s?
            string prettyPrinted = CSharpObjectFormatter.Instance.FormatObject(obj);
            if (Aliases.TryGetValue(obj.GetType(), out string type))
            {
                // CSharpObjectFormatter doesn't always output types, so use a
                // dictionary to get the type name in those cases
                return (type, prettyPrinted);
            }
            else
            {
                // we want to swap the order of the value and the type from CSharpObjectFormatter.
                // given 
                // Enumerable.SelectArrayIterator<int, int> { 2, 3, 4 }
                // transform to 
                // { 2, 3, 4 } :Enumerable.SelectArrayIterator<int, int>
                bool inGeneric = false;
                var typeName = new String(prettyPrinted.TakeWhile(x =>
                {
                    if (x == '<') inGeneric = true;
                    if (x == '>') inGeneric = false;
                    return inGeneric || x != ' ';
                }).ToArray());
                return (typeName, prettyPrinted.Substring(typeName.Length + 1));
            }
        }

        // translates unfriendly type names to friendly type names (e.g. 'System.Int32' to 'int')
        private static readonly Dictionary<Type, string> Aliases =
            new Dictionary<Type, string>()
        {
            { typeof(byte), "byte" },
            { typeof(sbyte), "sbyte" },
            { typeof(short), "short" },
            { typeof(ushort), "ushort" },
            { typeof(int), "int" },
            { typeof(uint), "uint" },
            { typeof(long), "long" },
            { typeof(ulong), "ulong" },
            { typeof(float), "float" },
            { typeof(double), "double" },
            { typeof(decimal), "decimal" },
            { typeof(object), "object" },
            { typeof(bool), "bool" },
            { typeof(char), "char" },
            { typeof(string), "string" },
            { typeof(void), "void" },
            { typeof(DateTime), "DateTime" }
        };
    }
}
