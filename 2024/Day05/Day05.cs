using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;

namespace _2024.Day05
{
    public class Day05 : Day<(Dictionary<int, List<int>>, List<List<int>>), long, long>
    {
        public override string DayNumber { get { return "05"; } }

        public override long PartOne((Dictionary<int, List<int>>, List<List<int>>) input)
        {
            var rules = input.Item1;
            var updates = input.Item2;
            long sum = 0;
            foreach (var pages in updates)
            {
                bool order = true;
                // starting from the last of the pages, check if page before it is present in the rules, it is in order
                for (int i = pages.Count - 1; i > 0; i--)
                {
                    if (!rules.ContainsKey(pages[i]) || !rules[pages[i]].Contains(pages[i - 1])) { order = false; break; }
                }
                sum += order ? pages[pages.Count / 2] : 0;
            }
            return sum;
        }

        public override long PartTwo((Dictionary<int, List<int>>, List<List<int>>) input)
        {
            var rules = input.Item1;
            var updates = input.Item2;
            long sum = 0;
            foreach (var pages in updates)
            {
                bool order = true;
                for (int i = pages.Count - 1; i > 0; i--)
                {
                    if (!rules.ContainsKey(pages[i]) || !rules[pages[i]].Contains(pages[i - 1])) { order = false; break; }
                }
                int[] correctOrder = new int[pages.Count];
                if (!order)
                {
                    int counter = correctOrder.Length - 1;
                    while (counter >= 0)
                    {
                        List<int> allValues = new List<int>();
                        var remPages = pages.Except(correctOrder).ToList();
                        foreach (var p in remPages)
                        {
                            if (rules.ContainsKey(p)) allValues.AddRange(rules[p]);
                        }
                        // find a page that doesn't exist in other pages' rules - it appears last, exclude it next
                        correctOrder[counter--] = remPages.First(x => !allValues.Contains(x));
                    }
                }
                sum += order ? 0 : correctOrder[correctOrder.Length / 2];
            }
            return sum;
        }

        public override (Dictionary<int, List<int>>, List<List<int>>) ProcessInput(string[] input)
        {
            var blocks = input.Blocks();
            // rules = Dictionary<key, List<pages prior to key>>
            Dictionary<int, List<int>> rules = new Dictionary<int, List<int>>();
            foreach (var line in blocks[0])
            {
                var nums = line.Split('|', StringSplitOptions.RemoveEmptyEntries).StringArrayToIntList();
                if (rules.ContainsKey(nums[1]))
                {
                    rules[nums[1]].Add(nums[0]);
                }
                else
                {
                    rules[nums[1]] = new List<int>() { nums[0] };
                }
            }
            List<List<int>> updates = new List<List<int>>();
            foreach (var line in blocks[1])
            {
                var pages = line.Split(',', StringSplitOptions.RemoveEmptyEntries).StringArrayToIntList();
                updates.Add(pages);
            }
            return (rules, updates);
        }
    }
}
