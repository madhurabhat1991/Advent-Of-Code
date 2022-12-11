using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2022.Day11
{
    public class Day11 : Day<List<Monkey>, long, long>
    {
        public override string DayNumber { get { return "11"; } }

        public override long PartOne(List<Monkey> input)
        {
            var monkeys = input.DeepClone();
            return MonkeyBusiness(monkeys, true);
        }

        public override long PartTwo(List<Monkey> input)
        {
            var monkeys = input.DeepClone();
            return MonkeyBusiness(monkeys, false);
        }

        public override List<Monkey> ProcessInput(string[] input)
        {
            List<Monkey> monkeys = new List<Monkey>();
            var blocks = input.Blocks();
            foreach (var block in blocks)
            {
                Monkey monkey = new Monkey { };
                foreach (var line in block)
                {
                    if (line.StartsWith("Monkey"))
                    {
                        monkey.Number = Int32.Parse(line.Replace("Monkey ", "").Trim(':'));
                    }
                    else if (line.TrimStart().StartsWith("Starting items"))
                    {
                        monkey.Items = new Queue<long>(line.Replace("Starting items: ", "").Split(",", StringSplitOptions.RemoveEmptyEntries).Select(r => Int64.Parse(r)));
                    }
                    else if (line.TrimStart().StartsWith("Operation"))
                    {
                        monkey.Operation = line.TrimStart().Replace("Operation: new = ", "");
                    }
                    else if (line.TrimStart().StartsWith("Test"))
                    {
                        monkey.Test.Item1 = Int32.Parse(line.Replace("Test: divisible by ", ""));
                    }
                    else if (line.TrimStart().StartsWith("If true"))
                    {
                        monkey.Test.Item2 = Int32.Parse(line.Replace("If true: throw to monkey ", ""));
                    }
                    else if (line.TrimStart().StartsWith("If false"))
                    {
                        monkey.Test.Item3 = Int32.Parse(line.Replace("If false: throw to monkey ", ""));
                    }
                }
                monkeys.Add(monkey);
            }
            return monkeys;
        }

        private const int RoundsOne = 20;
        private const int RoundsTwo = 10000;

        /// <summary>
        /// Calculate Monkey Business
        /// </summary>
        /// <param name="monkeys"></param>
        /// <param name="part1">true for part1, false otherwise</param>
        /// <returns></returns>
        private long MonkeyBusiness(List<Monkey> monkeys, bool part1)
        {
            int rounds = part1 ? RoundsOne : RoundsTwo;
            var relief = monkeys.Aggregate(1, (a, x) => a * x.Test.Item1);
            while (rounds > 0)
            {
                foreach (var monkey in monkeys)
                {
                    while (monkey.Items != null && monkey.Items.Any())
                    {
                        monkey.Inspect();
                        monkey.Relief(part1, relief);
                        (int, long) next = monkey.Throw();
                        monkeys.First(r => r.Number == next.Item1).Items.Enqueue(next.Item2);
                    }
                }
                rounds--;
            }
            return monkeys.Select(r => r.Inspections)
                .OrderByDescending(r => r)
                .Take(2)
                .Aggregate((a, x) => a * x);

        }
    }

    [Serializable]
    public class Monkey
    {
        public int Number { get; set; }
        public Queue<long> Items { get; set; } = new Queue<long>();
        public string Operation { get; set; }
        public (int, int, int) Test = (0, 0, 0);    // (div, true, false)

        public long Inspections { get; set; }
        private long itemInspecting = 0;

        public void Inspect()
        {
            Inspections++;
            itemInspecting = Items.Dequeue();
            var cmd = Operation.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var op = cmd[1][0];
            var val = cmd[2].Equals("old") ? itemInspecting : Int32.Parse(cmd[2]);
            switch (op)
            {
                case '+':
                    itemInspecting += val;
                    break;
                case '-':
                    itemInspecting -= val;
                    break;
                case '*':
                    itemInspecting *= val;
                    break;
                case '/':
                    itemInspecting /= val;
                    break;
                default:
                    break;
            }
        }

        public void Relief(bool part1, long relief)
        {
            itemInspecting = part1 ? (long)Math.Floor(itemInspecting / 3d) : (itemInspecting % relief);
        }

        public (int, long) Throw()
        {
            if (itemInspecting % Test.Item1 == 0)
            {
                return (Test.Item2, itemInspecting);
            }
            else
            {
                return (Test.Item3, itemInspecting);
            }

        }
    }
}
