using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotNetLisp.Util
{
    class AssemblyRunner
    {
        internal static T Run<T>(byte[] bytes, string namespaceName, string className, string methodName)
        {
            string FullyQualifiedClass = $"{namespaceName}.{className}";
            Assembly assembly = Assembly.Load(bytes);
            Type type = assembly.GetType(FullyQualifiedClass);
            return (T)type.InvokeMember(methodName, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
        }
    }
}
