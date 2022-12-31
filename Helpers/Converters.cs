using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers
{
    public static class Converters
    {
        /// <summary>
        /// Convert char to int
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Int32 CharToInt(this Char input)
        {
            return input - '0';
        }

        /// <summary>
        /// Convery binary string to decimal number
        /// </summary>
        /// <param name="binary"></param>
        /// <returns></returns>
        public static Int64 BinaryStringToDecimal(this String binary)
        {
            return Convert.ToInt64(binary, 2);
        }

        /// <summary>
        /// Convert each string in an array to int array
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Int32[] StringArrayToIntArray(this String[] input)
        {
            return input.Select(line => Int32.Parse(line)).ToArray();
        }

        /// <summary>
        /// Convert each string in an array to int list
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<Int32> StringArrayToIntList(this String[] input)
        {
            return input.Select(line => Int32.Parse(line)).ToList();
        }
    }
}
