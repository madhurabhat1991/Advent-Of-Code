using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;

namespace _2022.Day03
{
    public class Day03 : Day<List<String>, long, long>
    {
        public override string DayNumber { get { return "03"; } }

        public override long PartOne(List<string> input)
        {
            long sum = 0;
            foreach (var rs in input)
            {
                var half1 = rs.Substring(0, rs.Length / 2);
                var half2 = rs.Substring(rs.Length / 2, rs.Length / 2);
                var common = FindCommonItemType(new List<string>() { half1, half2 });
                sum += AssignPriority(common);
            }
            return sum;
        }

        public override long PartTwo(List<string> input)
        {
            long sum = 0;
            for (int i = 0; i < input.Count - 2; i = i + 3)
            {
                var common = FindCommonItemType(new List<string>() { input[i], input[i + 1], input[i + 2] });
                sum += AssignPriority(common);
            }
            return sum;

        }

        public override List<string> ProcessInput(string[] input)
        {
            return input.ToList();
        }

        /// <summary>
        /// Find common item type in a list of strings
        /// </summary>
        /// <param name="rucksacks"></param>
        /// <returns></returns>
        private char FindCommonItemType(List<string> rucksacks)
        {
            List<char> commons = new List<char>();
            for (int i = 0; i < rucksacks.Count - 1; i++)
            {
                var rs1 = rucksacks[i];
                var rs2 = rucksacks[i + 1];
                var temp = rs1.ToList().Intersect(rs2.ToList()).ToList();
                commons = commons.Any() ? temp.Intersect(commons).ToList() : temp;
            }
            return commons.FirstOrDefault();
        }

        /// <summary>
        /// Assign priority to item type - a-z = 1-26, A-Z = 27-52
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        private int AssignPriority(char itemType)
        {
            return Char.IsLower(itemType) ? (itemType - 'a' + 1) : (itemType - 'A' + 27);
        }
    }
}
