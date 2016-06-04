using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetLisp.StandardLibrary
{
    public static class ReplUtil
    {
        public static string Print<T>(Func<T> func)
        {
            return Print((dynamic)func());
        }

        public static string Print(Action func)
        {
            func();
            return "void";
        }

        public static string Print(object obj)
        {
            if(obj == null)
            {
                return "null";
            }
            string output = JsonConvert.SerializeObject(obj, Formatting.Indented);
            string type = obj.GetType().Name;
            return $"{output} :{type}";
        }
    }
}
