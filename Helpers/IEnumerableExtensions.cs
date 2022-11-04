using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers
{
    public static class IEnumerableExtensions
    {
        public static List<int> StringToInt(this String[] input)
        {
            return input.Select(line => Int32.Parse(line)).ToList();
        }
    }
}
