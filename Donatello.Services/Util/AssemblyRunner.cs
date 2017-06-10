using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.Services.Util
{
    public static class AssemblyRunner
    {

        public static T Run<T>(Stream assembly, string namespaceName, string className, string methodName, object[] args = null)
        {
            Type type = GetTypeFromAssemblyStream(assembly, namespaceName, className);
            return (T)type.GetTypeInfo().GetDeclaredMethod(methodName).Invoke(null, args);
        }

        internal static MethodInfo GetFunction(Stream assembly, string namespaceName, string className, string methodName)
        {
            Type type = GetTypeFromAssemblyStream(assembly, namespaceName, className);
            var macroMethodInfo = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
            return macroMethodInfo;
            //var macroParam = Expression.Parameter(typeof(TArg), "arg");
            //var lambda = Expression.Lambda<Func<TArg, TReturn>>(Expression.Call(macroMethodInfo, macroParam), macroParam);
            //return lambda.Compile();
        }

        private static Type GetTypeFromAssemblyStream(Stream assemblyStream, string namespaceName, string className)
        {
            string FullyQualifiedClass = $"{namespaceName}.{className}";
            Assembly assembly = AssemblyLoadContext.Default.LoadFromStream(assemblyStream);
            Type type = assembly.GetType(FullyQualifiedClass);
            return type;
        }
    }
}
