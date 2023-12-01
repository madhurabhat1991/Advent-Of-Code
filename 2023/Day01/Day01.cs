using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;

namespace _2023.Day01
{
    public class Day01 : Day<string[], long, long>
    {
        public override string DayNumber { get { return "01"; } }

        public override long PartOne(string[] input)
        {
            List<SortedDictionary<int, string>> values = new List<SortedDictionary<int, string>>();
            foreach (var line in input)
            {
                values.Add(GetDigitIndices(line));
            }
            return SumOfValues(values);
        }

        public override long PartTwo(string[] input)
        {
            List<SortedDictionary<int, string>> values = new List<SortedDictionary<int, string>>();
            foreach (var line in input)
            {
                var indexValues = GetDigitIndices(line);
                values.Add(GetWordIndices(line, indexValues));
            }
            return SumOfValues(values);
        }

        public override string[] ProcessInput(string[] input)
        {
            return input;
        }

        private Dictionary<String, int> StringValues = new Dictionary<string, int>()
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 }
        };

        /// <summary>
        /// Get indices of digits in a string - used for both part 1 and 2
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private SortedDictionary<int, string> GetDigitIndices(string line)
        {
            SortedDictionary<int, string> indexValues = new SortedDictionary<int, string>();
            var chars = line.ToCharArray().ToList();
            for (int i = 0; i < chars.Count; i++)
            {
                if (Char.IsDigit(chars[i]))
                {
                    indexValues.Add(i, chars[i].ToString());
                }
            }
            return indexValues;
        }

        /// <summary>
        /// Get indices of words in a string - used for part 2 only
        /// </summary>
        /// <param name="line"></param>
        /// <param name="indexValues"></param>
        /// <returns></returns>
        private SortedDictionary<int, string> GetWordIndices(string line, SortedDictionary<int, string> indexValues)
        {
            var words = StringValues.Keys.Where(key => line.Contains(key)).Select(key => key).ToList();
            foreach (var word in words)
            {
                for (int i = 0; i < line.Length && (i + word.Length <= line.Length); i++)
                {
                    var lineWord = line.Substring(i, word.Length);
                    if (word.Equals(lineWord))
                    {
                        indexValues.Add(i, StringValues[word].ToString());
                        i += word.Length;
                    }
                }
            }
            return indexValues;
        }

        /// <summary>
        /// Sum of all calibration values
        /// </summary>
        /// <param name="indexValues"></param>
        /// <returns></returns>
        private long SumOfValues(List<SortedDictionary<int, string>> indexValues)
        {
            long sum = 0;
            foreach (var line in indexValues)
            {
                var value = "";
                value += line[line.Keys.First()];
                value += line[line.Keys.Last()];
                sum += Int64.Parse(value);
            }
            return sum;
        }
    }
}
