using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.Util
{
    static class AssemblyRunner
    {
        internal static void RunClassConstructor(byte[] bytes, string namespaceName, string className)
        {
            string FullyQualifiedClass = $"{namespaceName}.{className}";
            Assembly assembly = Assembly.Load(bytes);
            Type type = assembly.GetType(FullyQualifiedClass);
            RuntimeHelpers.RunClassConstructor(type.TypeHandle);
        }

        internal static T Run<T>(byte[] bytes, string namespaceName, string className, string methodName)
        {
            string FullyQualifiedClass = $"{namespaceName}.{className}";
            Assembly assembly = Assembly.Load(bytes);
            Type type = assembly.GetType(FullyQualifiedClass);
            return (T)type.InvokeMember(methodName, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
        }
    }
}
