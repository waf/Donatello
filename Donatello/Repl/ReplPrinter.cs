using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

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
            string output = JsonConvert.SerializeObject(obj, Formatting.None);
            string type = PrettyPrintTypeName(obj.GetType());
            Console.WriteLine($"{output} :{type}");
        }

        private static string PrettyPrintTypeName(Type type)
        {
            if (Aliases.TryGetValue(type, out string name))
            {
                return name;
            }

            string typeName = type.FullName.Replace(type.Namespace + ".", "");//Removing the namespace
            using (var provider = CodeDomProvider.CreateProvider("CSharp"))
            {
                var reference = new CodeTypeReference(typeName);
                return provider.GetTypeOutput(reference);
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
            { typeof(void), "void" }
        };
    }
}
