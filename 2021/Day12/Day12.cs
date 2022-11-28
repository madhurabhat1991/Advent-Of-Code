using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;

namespace _2021.Day12
{
    public class Day12 : Day<Dictionary<String, List<String>>, long, long>
    {
        public override string DayNumber { get { return "12"; } }

        public override long PartOne(Dictionary<String, List<String>> input)
        {
            Input = input;
            Paths = new List<List<string>>();
            CreatePath(Start, new List<string>(), EntryLimitOne);

            return Paths.Count();
        }

        public override long PartTwo(Dictionary<String, List<String>> input)
        {
            Input = input;
            Paths = new List<List<string>>();
            CreatePath(Start, new List<string>(), EntryLimitTwo);

            return Paths.Count();
        }

        public override Dictionary<String, List<String>> ProcessInput(string[] input)
        {
            Dictionary<String, List<String>> caves = new Dictionary<String, List<String>>();
            foreach (var line in input)
            {
                var nodes = line.Split("-").ToArray();
                for (int node = 0; node < nodes.Length; node++)
                {
                    string nodeFrom = nodes[node], nodeTo = nodes[1 - node];
                    if (!nodeFrom.Equals(End) && !nodeTo.Equals(Start))
                    {
                        List<string> value;
                        if (caves.TryGetValue(nodeFrom, out value))
                        {
                            value.Add(nodeTo);
                        }
                        else
                        {
                            value = new List<string>() { nodeTo };
                        }
                        caves[nodeFrom] = value;
                    }
                }
            }
            return caves;
        }

        private Dictionary<String, List<String>> Input = new Dictionary<string, List<string>>();
        private List<List<String>> Paths = new List<List<string>>();

        private const int EntryLimitOne = 1;
        private const int EntryLimitTwo = 2;

        private const String Start = "start";
        private const String End = "end";

        private bool SmallCave(String cave) { return cave.ToLower() == cave; }

        private void CreatePath(string from, List<String> visited, int entryLimit)
        {
            visited.Add(from);
            if (!Input.ContainsKey(from))
            {
                Paths.Add(visited);
                visited = new List<string>();
                return;
            }

            var next = DenyEntry(visited, entryLimit)
                ? Input[from].Where(x => !SmallCave(x) || x == End || !visited.Contains(x)).ToList()
                : Input[from];

            foreach (var x in next)
            {
                CreatePath(x, visited.ToList(), entryLimit);
            }
        }

        /// <summary>
        /// Deny entry if small cave has already been visited
        /// </summary>
        /// <param name="visited"></param>
        /// <param name="entryLimit"></param>
        /// <returns></returns>
        private bool DenyEntry(List<String> visited, int entryLimit)
        {
            if (entryLimit == EntryLimitOne) { return true; }
            if (visited.Any(x => SmallCave(x) && visited.Count(y => y == x) == EntryLimitTwo)) { return true; }
            return false;
        }
    }
}
