using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Convert each sring in an array to int array
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Int32[] StringArrayToIntArray(this String[] input)
        {
            return input.Select(line => Int32.Parse(line)).ToArray();
        }

        /// <summary>
        /// Convert each sring in an array to int list
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<Int32> StringArrayToIntList(this String[] input)
        {
            return input.Select(line => Int32.Parse(line)).ToList();
        }
    }
}
