using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetLisp.Util
{
    static class Utility
    {
        public static T Throw<T>(Exception ex)
        {
            throw ex;
        }
    }
}
