using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Template;
using static _2025.Day11.Day11;

namespace _2025.Day11
{
    public class Day11 : Day<Dictionary<string, List<string>>, long, long>
    {
        public override string DayNumber { get { return "11"; } }

        public override long PartOne(Dictionary<string, List<string>> input)
        {
            // use example1.txt input, skip Part2
            long paths = 0;
            Queue<List<string>> queue = new Queue<List<string>>();
            queue.Enqueue(new List<string> { Start1 });
            while (queue.Count > 0)
            {
                var path = queue.Dequeue();
                var last = path.Last();
                var connections = input[last];
                foreach (var connection in connections)
                {
                    if (connection == End)
                    {
                        paths++;
                    }
                    else
                    {
                        var newPath = path.ToList();
                        newPath.Add(connection);
                        queue.Enqueue(newPath);
                    }
                }
            }
            return paths;
        }

        public override long PartTwo(Dictionary<string, List<string>> input)
        {
            // use example2.txt input, skip Part1

            // finding all paths from svr to out and checking if they have both dac and fft will take long time
            // tried to find all paths from fft to out that contains dac using graph and queue. works for example, took long for input

            // used reddit to find out fft always appear before dac
            // find paths between svr and fft, fft and dac, dac and out. multiply to find all combinations.
            // use DFS recursive to find paths and memoize found paths

            long svrToFft = DFS(input, Start2, FFT);
            Path_Memoize.Clear();
            long FftToDac = DFS(input, FFT, DAC);
            Path_Memoize.Clear();
            long DacToOut = DFS(input, DAC, End);
            Path_Memoize.Clear();

            return svrToFft * FftToDac * DacToOut;
        }

        public override Dictionary<string, List<string>> ProcessInput(string[] input)
        {
            Dictionary<string, List<string>> connections = new Dictionary<string, List<string>>();
            foreach (var line in input)
            {
                var splits = line.Split(':', StringSplitOptions.RemoveEmptyEntries);
                string from = splits[0].Trim();
                List<string> to = splits[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
                connections[from] = to;
            }
            return connections;
        }

        private const string Start1 = "you";
        private const string End = "out";
        private const string Start2 = "svr";
        private const string FFT = "fft";
        private const string DAC = "dac";

        /// <summary>
        /// Memoization to store the number of paths calculated
        /// </summary>
        private Dictionary<string, long> Path_Memoize = new Dictionary<string, long>();

        /// <summary>
        /// Depth First Search DFS Recursive to find all paths from start to end
        /// </summary>
        /// <param name="input"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private long DFS(Dictionary<string, List<string>> input, string start, string end)
        {
            long paths = 0;
            // Destination reached - results in a path
            if (start == end)
            {
                return 1;
            }
            // Check if memoized
            if (Path_Memoize.TryGetValue(start, out long memPaths))
            {
                return memPaths;
            }
            // Recurse for all adjacent vertices
            if (input.ContainsKey(start))
            {
                foreach (string next in input[start])
                {
                    paths += DFS(input, next, end);
                }
            }
            // Memoize
            Path_Memoize.Add(start, paths);
            return paths;
        }
    }
}
