using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;
using Helpers;

namespace _2022.Day06
{
    public class Day06 : Day<String, long, long>
    {
        public override string DayNumber { get { return "06"; } }

        public override long PartOne(String input)
        {
            return DetectMarker(input, PacketMarker);
        }

        public override long PartTwo(String input)
        {
            return DetectMarker(input, MessageMarker);
        }

        public override String ProcessInput(string[] input)
        {
            return input[0];
        }

        private const int PacketMarker = 4;
        private const int MessageMarker = 14;

        private long DetectMarker(string input, int len)
        {
            Dictionary<char, long> dict = new Dictionary<char, long>();
            for (int i = 0; i < input.Length; i++)
            {
                dict.IncrementValue(input[i], 1);
                if (i >= len - 1)
                {
                    if (i > len - 1)
                    {
                        if (dict.ContainsKey(input[i - len]))
                        {
                            dict[input[i - len]]--;
                            if (dict[input[i - len]] == 0) { dict.Remove(input[i - len]); }
                        }
                    }
                    if (dict.Keys.Count == len)
                    {
                        return i + 1;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Sliding window approach but may not be optimized for larger len
        /// </summary>
        /// <param name="input"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        private long DetectMarkerWindow(string input, int len)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (input.Substring(i, len).ToList().Distinct().Count() == len)
                {
                    return i + len;
                }
            }
            return 0;
        }
    }
}
