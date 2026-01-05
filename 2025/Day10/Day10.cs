using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
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
                long minPresses = long.MaxValue;
                Buttons_Mem.Clear();
                var buttons = machine.Item2;
                var joltages = machine.Item3;
                // find the buttons required to generate odd joltages
                var bResults = FindButtonsToPressPhase1(buttons, joltages);
                foreach (var bResult in bResults)
                {
                    var jLeft = bResult.Item1;
                    var presses = bResult.Item2;
                    var pressed = FindButtonsToPressPhase2(buttons, jLeft);
                    minPresses = Math.Min(minPresses, pressed + presses);
                }
                sum += minPresses;
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

        // Dictionary<joltages, (joltage left, presses)>
        Dictionary<List<int>, List<(List<int>, long)>> Buttons_Mem = new Dictionary<List<int>, List<(List<int>, long)>>();

        private List<(List<int>, long)> FindButtonsToPressPhase1(List<List<int>> buttons, List<int> joltages)
        {
            if (Buttons_Mem.ContainsKey(joltages))
            {
                return Buttons_Mem[joltages];
            }
            var diagram = joltages.Select(x => x % 2 == 0 ? Off : On).ToList();
            // PriorityQueue<(expected, joltage left, buttons), presses>
            PriorityQueue<(List<int>, List<int>, List<List<int>>), int> queue = new PriorityQueue<(List<int>, List<int>, List<List<int>>), int>();
            // lights initially on
            List<int> lights = new List<int>();
            for (int i = 0; i < diagram.Count; i++)
            {
                if (diagram[i] == On) { lights.Add(i); }
            }
            queue.Enqueue((lights, joltages.ToList<int>(), new List<List<int>>()), 0);
            while (queue.Count > 0)
            {
                queue.TryDequeue(out var element, out var priority);
                var expected = element.Item1;
                var jLeft = element.Item2;
                var btPressed = element.Item3;
                foreach (var button in buttons)
                {
                    if (!btPressed.Contains(button))
                    {
                        var newExpected = button.Union(expected).Except(button.Intersect(expected)).Order().ToList();
                        // calculate joltages left to reach
                        List<int> newJLeft = jLeft.ToList();
                        for (int i = 0; i < button.Count; i++)
                        {
                            newJLeft[button[i]]--;
                        }
                        var newBtPressed = btPressed.ToList();
                        newBtPressed.Add(button);
                        var newPriority = priority + 1;

                        // same switches to be turned on/off as expected
                        if (button.SequenceEqual(expected))
                        {
                            // is it valid?
                            if (newJLeft.Any(x => x < 0) || newJLeft.Any(x => x % 2 > 0))
                            {
                                continue;
                            }
                            newJLeft = newJLeft.Select(x => x / 2).ToList();
                            if (Buttons_Mem.TryGetValue(joltages, out var dictVal))
                            {
                                if (!dictVal.Any(x => x.Item1.SequenceEqual(newJLeft) && x.Item2 == newPriority))
                                {
                                    dictVal.Add((newJLeft, newPriority));
                                }
                            }
                            else
                            {
                                Buttons_Mem[joltages] = new List<(List<int>, long)>() { (newJLeft, newPriority) };
                            }
                            continue;
                        }
                        else
                        {
                            // apart from what I've turned on/off, what will this button turn on/off?
                            // if I expect 1235 and I get button 01245, after pressing I need 034 --> next expected
                            if (!queue.UnorderedItems.Any(x => x.Element.Item1.SequenceEqual(newExpected) && x.Element.Item2.SequenceEqual(newJLeft) && x.Priority == newPriority))
                            {
                                queue.Enqueue((newExpected, newJLeft, newBtPressed), newPriority);
                            }
                        }
                    }
                }
            }
            return Buttons_Mem[joltages];
        }

        private long FindButtonsToPressPhase2(List<List<int>> buttons, List<int> joltages)
        {
            long total = 0;
            if (Buttons_Mem.ContainsKey(joltages))
            {
                return Buttons_Mem[joltages].Select(x => x.Item2).ToList().Min();
            }
            // reached?
            if (joltages.All(x => x == 0))
            {
                return 0;
            }
            var diagram = joltages.Select(x => x % 2 == 0 ? Off : On).ToList();
            // PriorityQueue<(expected, joltage left, buttons), presses>
            PriorityQueue<(List<int>, List<int>, List<List<int>>), int> queue = new PriorityQueue<(List<int>, List<int>, List<List<int>>), int>();
            // lights initially on
            List<int> lights = new List<int>();
            for (int i = 0; i < diagram.Count; i++)
            {
                if (diagram[i] == On) { lights.Add(i); }
            }
            queue.Enqueue((lights, joltages.ToList<int>(), new List<List<int>>()), 0);
            while (queue.Count > 0)
            {
                queue.TryDequeue(out var element, out var priority);
                var expected = element.Item1;
                var jLeft = element.Item2;
                var btPressed = element.Item3;
                foreach (var button in buttons)
                {
                    if (!btPressed.Contains(button))
                    {
                        var newExpected = button.Union(expected).Except(button.Intersect(expected)).Order().ToList();
                        // calculate joltages left to reach
                        List<int> newJLeft = jLeft.ToList();
                        for (int i = 0; i < button.Count; i++)
                        {
                            newJLeft[button[i]]--;
                        }
                        var newBtPressed = btPressed.ToList();
                        newBtPressed.Add(button);
                        var newPriority = priority + 1;

                        // same switches to be turned on/off as expected
                        if (button.SequenceEqual(expected))
                        {
                            // is it valid?
                            if (newJLeft.Any(x => x < 0) || newJLeft.Any(x => x % 2 > 0))
                            {
                                continue;
                            }
                            newJLeft = newJLeft.Select(x => x / 2).ToList();
                            if (Buttons_Mem.TryGetValue(joltages, out var dictVal))
                            {
                                if (!dictVal.Any(x => x.Item1.SequenceEqual(newJLeft) && x.Item2 == newPriority))
                                {
                                    dictVal.Add((newJLeft, newPriority));
                                }
                            }
                            else
                            {
                                Buttons_Mem[joltages] = new List<(List<int>, long)>() { (newJLeft, newPriority) };
                            }
                            var newRet = FindButtonsToPressPhase2(buttons, newJLeft);
                            total = total == 0 ? Buttons_Mem[joltages].Select(x => x.Item2).ToList().Min() : total;
                            newRet = Buttons_Mem[joltages].Select(x => x.Item2).ToList().Min();
                            total += total * 2 + newRet;
                            return total;
                        }
                        else
                        {
                            // apart from what I've turned on/off, what will this button turn on/off?
                            // if I expect 1235 and I get button 01245, after pressing I need 034 --> next expected
                            if (!queue.UnorderedItems.Any(x => x.Element.Item1.SequenceEqual(newExpected) && x.Element.Item2.SequenceEqual(newJLeft) && x.Priority == newPriority))
                            {
                                queue.Enqueue((newExpected, newJLeft, newBtPressed), newPriority);
                            }
                        }
                    }
                }
            }
            return total;
        }

        #region Backup
        // this is giving 32 for example, taking too long for input, both not gonna work


        //foreach (var machine in input)
        //{
        //    long minPresses = long.MaxValue;
        //    Buttons_Mem.Clear();
        //    var buttons = machine.Item2;
        //    var joltages = machine.Item3;
        //    // Queue<(joltage left, presses, odd/even)>
        //    Queue<(List<int>, long, char)> queue = new Queue<(List<int>, long, char)>();
        //    const char odd = 'O', even = 'E';
        //    queue.Enqueue((joltages, 0, odd));
        //    while (queue.Count > 0)
        //    {
        //        var next = queue.Dequeue();
        //        var jLeft = next.Item1;
        //        var presses = next.Item2;
        //        var isOdd = next.Item3 == odd;
        //        // find the buttons required to generate odd joltages
        //        var bResults = FindButtonsToPress(buttons, jLeft);
        //        foreach (var bResult in bResults)
        //        {
        //            var reached = bResult.Item1;
        //            var pressed = presses + bResult.Item2 * (isOdd ? 1 : 2);
        //            List<int> jNew = new List<int>();
        //            // calculate joltages left to reach
        //            for (int i = 0; i < jLeft.Count; i++)
        //            {
        //                jNew.Add(jLeft[i] - reached[i]);
        //            }
        //            // reached?
        //            if (jNew.All(x => x == 0))
        //            {
        //                minPresses = Math.Min(minPresses, pressed);
        //                continue;
        //            }
        //            // is it valid?
        //            if (jNew.Any(x => x < 0) || jNew.Any(x => x % 2 > 0))
        //            {
        //                continue;
        //            }
        //            // are they all even?
        //            if (jNew.All(x => x % 2 == 0))
        //            {
        //                var jHalved = jNew.Select(x => x / 2).ToList();
        //                queue.Enqueue((jHalved, pressed, even));
        //            }
        //        }
        //    }
        //    sum += minPresses;
        //}

        //private List<(List<int>, long)> FindButtonsToPress(List<List<int>> buttons, List<int> joltages)
        //{
        //    if (Buttons_Mem.ContainsKey(joltages))
        //    {
        //        return Buttons_Mem[joltages];
        //    }
        //    var diagram = joltages.Select(x => x % 2 == 0 ? Off : On).ToList();
        //    // PriorityQueue<(expected, reached, buttons), presses>
        //    PriorityQueue<(List<int>, List<int>, List<List<int>>), int> queue = new PriorityQueue<(List<int>, List<int>, List<List<int>>), int>();
        //    // lights initially on
        //    List<int> lights = new List<int>();
        //    for (int i = 0; i < diagram.Count; i++)
        //    {
        //        if (diagram[i] == On) { lights.Add(i); }
        //    }
        //    queue.Enqueue((lights, Enumerable.Repeat(0, joltages.Count).ToList<int>(), new List<List<int>>()), 0);
        //    while (queue.Count > 0)
        //    {
        //        queue.TryDequeue(out var element, out var priority);
        //        var expected = element.Item1;
        //        var reached = element.Item2;
        //        var btPressed = element.Item3;
        //        foreach (var button in buttons)
        //        {
        //            if (!btPressed.Contains(button))
        //            {
        //                var newExpected = button.Union(expected).Except(button.Intersect(expected)).Order().ToList();
        //                var newReached = reached.ToList();
        //                button.ForEach(x => newReached[x]++);
        //                var newBtPressed = btPressed.ToList();
        //                newBtPressed.Add(button);
        //                var newPriority = priority + 1;

        //                // same switches to be turned on/off as expected
        //                if (button.SequenceEqual(expected))
        //                {
        //                    if (Buttons_Mem.TryGetValue(joltages, out var dictVal))
        //                    {
        //                        if (!dictVal.Any(x => x.Item1.SequenceEqual(newReached) && x.Item2 == newPriority))
        //                        {
        //                            dictVal.Add((newReached, newPriority));
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Buttons_Mem[joltages] = new List<(List<int>, long)>() { (newReached, newPriority) };
        //                    }
        //                    continue;
        //                }
        //                else
        //                {
        //                    // apart from what I've turned on/off, what will this button turn on/off?
        //                    // if I expect 1235 and I get button 01245, after pressing I need 034 --> next expected
        //                    if (!queue.UnorderedItems.Any(x => x.Element.Item1.SequenceEqual(newExpected) && x.Element.Item2.SequenceEqual(newReached) && x.Priority == newPriority))
        //                    {
        //                        queue.Enqueue((newExpected, newReached, newBtPressed), newPriority);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return Buttons_Mem[joltages];
        //}

        #endregion
    }
}
