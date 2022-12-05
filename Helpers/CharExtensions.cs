using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers
{
    public static class CharExtensions
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
    }
}
