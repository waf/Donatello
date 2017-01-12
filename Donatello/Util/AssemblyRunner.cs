using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.Util
{
    static class AssemblyRunner
    {

        internal static T Run<T>(byte[] bytes, string namespaceName, string className, string methodName, object[] args = null)
        {
            Type type = GetTypeFromAssemblyBytes(bytes, namespaceName, className);
            return (T)type.InvokeMember(methodName, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, args);
        }

        internal static MethodInfo GetFunction(byte[] bytes, string namespaceName, string className, string methodName)
        {
            Type type = GetTypeFromAssemblyBytes(bytes, namespaceName, className);
            var macroMethodInfo = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
            return macroMethodInfo;
            //var macroParam = Expression.Parameter(typeof(TArg), "arg");
            //var lambda = Expression.Lambda<Func<TArg, TReturn>>(Expression.Call(macroMethodInfo, macroParam), macroParam);
            //return lambda.Compile();
        }

        private static Type GetTypeFromAssemblyBytes(byte[] bytes, string namespaceName, string className)
        {
            string FullyQualifiedClass = $"{namespaceName}.{className}";
            Assembly assembly = Assembly.Load(bytes);
            Type type = assembly.GetType(FullyQualifiedClass);
            return type;
        }
    }
}
