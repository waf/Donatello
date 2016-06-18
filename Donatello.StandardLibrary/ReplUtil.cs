using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.StandardLibrary
{
    public static class ReplUtil
    {
        public static void Print<T>(Func<T> func)
        {
            Print((dynamic)func());
        }

        public static void Print(Action func)
        {
            func();
            Console.WriteLine("void");
        }

        public static void Print(object obj)
        {
            if(obj == null)
            {
                Console.WriteLine("null");
            }
            string output = JsonConvert.SerializeObject(obj, Formatting.Indented);
            string type = obj.GetType().Name;
            Console.WriteLine($"{output} :{type}");
        }
    }
}
