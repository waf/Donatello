using System;
using System.Collections.Generic;
using System.Text;

namespace Donatello.StandardLibrary
{
    public static class NumberFunctions
    {
        public static int incr(int i) => i + 1;
        public static long incr(long i) => i + 1;
        public static double incr(double i) => i + 1;
        public static decimal incr(decimal i) => i + 1;
        public static float incr(float i) => i + 1;

        public static int decr(int i) => i - 1;
        public static long decr(long i) => i - 1;
        public static double decr(double i) => i - 1;
        public static decimal decr(decimal i) => i - 1;
        public static float decr(float i) => i - 1;

        public static bool even(int i) => i % 2 == 0;
        public static bool even(long i) => i % 2 == 0;
        public static bool even(double i) => i % 2 == 0;
        public static bool even(decimal i) => i % 2 == 0;
        public static bool even(float i) => i % 2 == 0;

        public static bool odd(int i) => i % 2 == 1;
        public static bool odd(long i) => i % 2 == 1;
        public static bool odd(double i) => i % 2 == 1;
        public static bool odd(decimal i) => i % 2 == 1;
        public static bool odd(float i) => i % 2 == 1;
    }
}
