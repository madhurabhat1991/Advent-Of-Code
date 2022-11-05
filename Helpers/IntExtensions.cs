using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers
{
    public static class IntExtensions
    {
        /// <summary>
        /// Convery binary string to decimal number
        /// </summary>
        /// <param name="binary"></param>
        /// <returns></returns>
        public static Int64 BinaryToDecimal(this String binary)
        {
            return Convert.ToInt64(binary, 2);
        }
    }
}
