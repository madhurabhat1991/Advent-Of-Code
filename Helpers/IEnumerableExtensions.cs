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

        /// <summary>
        /// Extract blocks of input separated by blank line
        /// </summary>
        /// <param name="input"></param>
        /// <returns>(List<List<String>>)</returns>
        public static List<List<String>> Blocks(this String[] input)
        {
            List<List<String>> blocks = new List<List<String>>();
            while (input.Any())
            {
                List<String> block = new List<String>();
                while (input.Any() && input[0].Any())
                {
                    block.Add(input[0]);
                    input = input.Skip(1).ToArray();
                }
                blocks.Add(block);
                input = input.Skip(1).ToArray();
            }
            return blocks;
        }
    }
}
