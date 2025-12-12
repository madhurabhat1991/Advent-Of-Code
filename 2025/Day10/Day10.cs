using Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Template;

namespace _2025.Day10
{
    public class Day10 : Day<List<(List<char>, List<List<int>>, List<int>)>, long, long>
    {
        public override string DayNumber { get { return "10"; } }

        public override long PartOne(List<(List<char>, List<List<int>>, List<int>)> input)
        {
            long sum = 0;
            foreach (var machine in input)
            {
                var diagram = machine.Item1;
                var buttons = machine.Item2;
                // PriorityQueue<expected, presses>
                PriorityQueue<List<int>, int> queue = new PriorityQueue<List<int>, int>();
                // lights initially on
                List<int> lights = new List<int>();
                for (int i = 0; i < diagram.Count; i++)
                {
                    if (diagram[i] == On) { lights.Add(i); }
                }
                queue.Enqueue(lights, 0);
                long presses = 0;
                while (presses == 0)
                {
                    queue.TryDequeue(out var expected, out var priority);
                    foreach (var button in buttons)
                    {
                        // same switches to be turned on/off as expected
                        if (button.SequenceEqual(expected))
                        {
                            presses = priority + 1;
                            break;
                        }
                        else
                        {
                            // apart from what I've turned on/off, what will this button turn on/off?
                            // if I expect 1235 and I get button 01245, after pressing I need 034 --> next expected
                            var newExp = button.Union(expected).Except(button.Intersect(expected)).Order().ToList();
                            var newPri = priority + 1;
                            queue.Enqueue(newExp, newPri);
                        }
                    }
                }
                sum += presses;
            }
            return sum;
        }

        public override long PartTwo(List<(List<char>, List<List<int>>, List<int>)> input)
        {
            long sum = 0;
            foreach (var machine in input)
            {
                var buttons = machine.Item2;
                var joltages = machine.Item3;
                // PriorityQueue<(current, processed, available), presses>
                PriorityQueue<(List<int>, List<bool>, List<List<int>>), int> queue = new PriorityQueue<(List<int>, List<bool>, List<List<int>>), int>();
                List<int> current = Enumerable.Repeat(0, joltages.Count).ToList();
                List<bool> processed = Enumerable.Repeat(false, joltages.Count).ToList();
                List<List<int>> available = buttons.ToList();
                queue.Enqueue((current, processed, available), 0);
                long presses = 0;
                while (presses == 0)
                {
                    queue.TryDequeue(out var element, out var priority);
                    current = element.Item1;
                    processed = element.Item2;
                    available = element.Item3;
                    // find the least joltage to be calculated from expected, that is not yet obtained in current
                    int minJ = Int32.MaxValue;
                    int minJIndex = 0;
                    for (var i = 0; i < joltages.Count; i++)
                    {
                        if (joltages[i] < minJ && !processed[i])
                        {
                            minJ = joltages[i];
                            minJIndex = i;
                        }
                    }
                    for (var b = 0; b < available.Count; b++)
                    {
                        var button = available[b];
                        // will pressing the current button can affect that switch?
                        if (button.Contains(minJIndex))
                        {
                            List<int> newState = current.ToList();
                            List<bool> newProcessed = processed.ToList();
                            List<List<int>> newAvailable = available.ToList();
                            newProcessed[minJIndex] = true;
                            newAvailable.RemoveAt(b);
                            // how many times can this button be pressed?
                            for (var i = 1; i <= minJ; i++)
                            {
                                // while pressing current button what other joltages will change?
                                foreach (var num in button)
                                {
                                    newState[num] += 1;
                                }
                                // did we achieve the expected joltage?
                                if (newState.SequenceEqual(joltages))
                                {
                                    presses = priority + 1;
                                    break;
                                }
                                // are other joltages still under or equal expected?
                                bool outOfRange = false;
                                for (var j = 0; j < newState.Count; j++)
                                {
                                    if (newState[j] == joltages[j])
                                    {
                                        newProcessed[j] = true;
                                    }
                                    if (newState[j] > joltages[j])
                                    {
                                        outOfRange = true;
                                        break;
                                    }
                                }
                                if (!outOfRange)
                                {
                                    queue.Enqueue((newState.ToList(), newProcessed.ToList(), newAvailable.ToList()), priority + i);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                sum += presses;
            }
            return sum;
        }

        public override List<(List<char>, List<List<int>>, List<int>)> ProcessInput(string[] input)
        {
            List<(List<char>, List<List<int>>, List<int>)> machines = new List<(List<char>, List<List<int>>, List<int>)>();
            foreach (var line in input)
            {
                var lights = line.Substring(line.IndexOf('[') + 1, line.IndexOf(']') - line.IndexOf('[') - 1).ToCharArray().ToList();
                var buttons = line.Substring(line.IndexOf(']') + 2, line.IndexOf('{') - line.IndexOf(']') - 2)
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Replace("(", "").Replace(")", ""))
                    .Select(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries).StringArrayToIntList()).ToList();
                var joltages = line.Substring(line.IndexOf('{') + 1, line.IndexOf('}') - line.IndexOf('{') - 1)
                    .Split(',', StringSplitOptions.RemoveEmptyEntries).StringArrayToIntList();
                machines.Add((lights, buttons, joltages));
            }
            return machines;
        }

        private const char Off = '.';
        private const char On = '#';
    }
}
