using System;
using System.Collections.Generic;
using System.Text;
using Template;

namespace _2021.Day02
{
    public class Day02 : Day<string[], long, long>
    {
        public override string DayNumber { get { return "02"; } }

        public override long PartOne(string[] input)
        {
            long horizontal = 0;
            long depth = 0;
            foreach (var line in input)
            {
                var command = line.Split(" ");
                var x = Int32.Parse(command[1]);
                switch (command[0].ToLower())
                {
                    case "forward":
                        horizontal += x;
                        break;
                    case "down":
                        depth += x;
                        break;
                    case "up":
                        depth -= x;
                        break;
                    default:
                        break;
                }
            }
            return horizontal * depth;
        }

        public override long PartTwo(string[] input)
        {
            long horizontal = 0;
            long depth = 0;
            long aim = 0;
            foreach (var line in input)
            {
                var command = line.Split(" ");
                var x = Int32.Parse(command[1]);
                switch (command[0].ToLower())
                {
                    case "forward":
                        horizontal += x;
                        depth += (aim * x);
                        break;
                    case "down":
                        aim += x;
                        break;
                    case "up":
                        aim -= x;
                        break;
                    default:
                        break;
                }
            }
            return horizontal * depth;
        }

        public override string[] ProcessInput(string[] input)
        {
            return input;
        }
    }
}
