using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2021.Day03
{
    public class Day03 : Day<List<String>, long, long>
    {
        public override string DayNumber { get { return "03"; } }

        public override long PartOne(List<String> input)
        {
            string gamma = "", epsilon = "";
            for (var i = 0; i < input[0].Length; i++)
            {
                var (most, least) = FindMostAndLeastCommonBit(input, i);
                gamma += most;
                epsilon += least;
            }

            var gammaRate = gamma.BinaryStringToDecimal();
            var epsilonRate = epsilon.BinaryStringToDecimal();

            return gammaRate * epsilonRate;
        }

        public override long PartTwo(List<String> input)
        {
            var ogr = FindRating(input, One).BinaryStringToDecimal();
            var csr = FindRating(input, Zero).BinaryStringToDecimal();

            return ogr * csr;
        }

        public override List<String> ProcessInput(string[] input)
        {
            return input.ToList();
        }

        private const char Zero = '0';
        private const char One = '1';

        /// <summary>
        /// Find the most and least common bit for the given index
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <returns>Most common bit, Least common bit</returns>
        private (char, char) FindMostAndLeastCommonBit(List<String> input, int index)
        {
            var zeros = 0;
            var ones = 0;

            for (int i = 0; i < input.Count; i++)
            {
                switch (input[i][index])
                {
                    case Zero:
                        zeros++;
                        break;
                    case One:
                        ones++;
                        break;
                    default:
                        break;
                }
            }

            char most = zeros > ones ? Zero : One;
            char least = zeros <= ones ? Zero : One;
            return (most, least);
        }

        /// <summary>
        /// Slice the input list based on mode
        /// </summary>
        /// <param name="input"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        private String FindRating(List<String> input, Char mode)
        {
            int index = 0;
            while (input.Count > 1)
            {
                var (most, least) = FindMostAndLeastCommonBit(input, index);
                input = input.Where(r => r[index] == (mode == One ? most : least)).ToList();
                index++;
            }
            return input.FirstOrDefault() ?? "";
        }
    }
}
