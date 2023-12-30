using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers
{
    public static class MathExtensions
    {
        /// <summary>
        /// Find GCD of 2 numbers
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static long GCD(long a, long b)
        {
            if (b == 0) { return a; }
            return GCD(b, a % b);
        }

        /// <summary>
        /// Find GCD of List of numbers
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static long GCD(List<long> numbers)
        {
            var gcd = numbers[0];
            for (int i = 1; i < numbers.Count; i++)
            {
                gcd = GCD(numbers[i], gcd);
                if (gcd == 1) { return 1; }
            }
            return gcd;
        }

        /// <summary>
        /// Find LCM of 2 numbers
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static long LCM(long a, long b)
        {
            return (a / GCD(a, b)) * b;
        }

        /// <summary>
        /// Find LCM of List of numbers
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static long LCM(List<long> numbers)
        {
            var lcm = numbers[0];
            for (int i = 1; i < numbers.Count; i++)
            {
                lcm = ((numbers[i] * lcm) / (GCD(numbers[i], lcm)));
            }
            return lcm;
        }
    }
}
