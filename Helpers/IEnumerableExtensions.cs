using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers
{
    public static class IEnumerableExtensions
    {
        public static Int32[] StringArrayToIntArray(this String[] input)
        {
            return input.Select(line => Int32.Parse(line)).ToArray();
        }
        public static List<Int32> StringArrayToIntList(this String[] input)
        {
            return input.Select(line => Int32.Parse(line)).ToList();
        }
    }
}
