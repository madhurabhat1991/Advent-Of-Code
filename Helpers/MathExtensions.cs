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

        /// <summary>
        /// Calculate Taxicab geometry or Manhattan distance
        /// https://en.wikipedia.org/wiki/Taxicab_geometry
        /// Manhattan distance between (x1, y1) and (x2, y2) = | x1 - x2 | + | y1 - y2 |
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static long ManhattanDistance(long x1, long y1, long x2, long y2)
        {
            return (Math.Abs(x1 - x2) + Math.Abs(y1 - y2));
        }

        /// <summary>
        /// 2X2 Determinant
        /// https://en.wikipedia.org/wiki/Determinant
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static long Determinant(long x1, long y1, long x2, long y2)
        {
            return x1 * y2 - x2 * y1;
        }

        /// <summary>
        /// Calculate area of irregular polygon using Shoelace formula
        /// https://en.wikipedia.org/wiki/Shoelace_formula
        /// 2A = Sum (x1 * yn - xn * y1)
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static long ShoelaceFormula(List<(long, long)> points)
        {
            long area = 0;
            for (int i = 0; i < points.Count - 1; i++)
            {
                area += Determinant(points[i].Item1, points[i].Item2, points[i + 1].Item1, points[i + 1].Item2);
            }
            area = Math.Abs(area) / 2;
            return area;
        }

        /// <summary>
        /// Calculate area using Pick's theorem
        /// https://en.wikipedia.org/wiki/Pick%27s_theorem
        /// A = i + (b/2) + 1
        /// </summary>
        /// <param name="inside"></param>
        /// <param name="boundry"></param>
        /// <returns></returns>
        public static long PicksTheorem(long inside, long boundry)
        {
            return inside + (boundry / 2) + 1;
        }
    }
}
