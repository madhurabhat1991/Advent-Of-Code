using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;

namespace _2022.Day09
{
    public class Day09 : Day<List<String>, long, long>
    {
        public override string DayNumber { get { return "09"; } }

        public override long PartOne(List<string> input)
        {
            Rope rope = new Rope
            {
                Head = new Knot(Head),
                Tails = new List<Knot>() { new Knot(Tail) }
            };
            return RopeModel(input, rope).Count;
        }

        public override long PartTwo(List<string> input)
        {
            Rope rope = new Rope
            {
                Head = new Knot(Head),
            };
            Enumerable.Range(1, 9).ToList().ForEach(r => rope.Tails.Add(new Knot(r.ToString())));
            return RopeModel(input, rope).Count;
        }

        public override List<string> ProcessInput(string[] input)
        {
            return input.ToList();
        }

        private const char Up = 'U';
        private const char Right = 'R';
        private const char Down = 'D';
        private const char Left = 'L';

        private const string Head = "H";
        private const string Tail = "T";

        private HashSet<(int, int)> RopeModel(List<string> input, Rope rope)
        {
            HashSet<(int, int)> tailEnds = new HashSet<(int, int)>() { (0, 0) };
            foreach (var line in input)
            {
                var cmd = line.Split(" ");
                var dir = cmd[0][0];
                var step = Int32.Parse(cmd[1]);
                Move(dir, step, rope).ToList().ForEach(r => tailEnds.Add(r));
            }
            return tailEnds;
        }

        private HashSet<(int, int)> Move(char dir, int step, Rope rope)
        {
            HashSet<(int, int)> tailEnds = new HashSet<(int, int)>();
            for (int i = 0; i < step; i++)
            {
                // move Head
                switch (dir)
                {
                    case Up:
                        rope.Head.Position.Item1 -= 1;
                        break;
                    case Right:
                        rope.Head.Position.Item2 += 1;
                        break;
                    case Down:
                        rope.Head.Position.Item1 += 1;
                        break;
                    case Left:
                        rope.Head.Position.Item2 -= 1;
                        break;
                    default:
                        break;
                }
                // check if tails are touching
                var prev = rope.Head;
                foreach (var current in rope.Tails)
                {
                    // move current - if prev and current do not overlap and are not adjacent - distance > 1
                    if (Math.Abs(prev.Position.Item1 - current.Position.Item1) > 1
                        || Math.Abs(prev.Position.Item2 - current.Position.Item2) > 1)
                    {
                        // move 1 step towards prev - sign determines direction
                        current.Position.Item1 += Math.Sign(prev.Position.Item1 - current.Position.Item1);
                        current.Position.Item2 += Math.Sign(prev.Position.Item2 - current.Position.Item2);
                    }
                    prev = current;
                }
                tailEnds.Add(rope.Tails.Last().Position);
            }
            return tailEnds;
        }
    }

    public class Rope
    {
        public Knot Head { get; set; }
        public List<Knot> Tails { get; set; } = new List<Knot>();
    }

    public class Knot
    {
        public string Name { get; set; }
        public (int, int) Position = (0, 0);
        public Knot(string name)
        {
            this.Name = name;
        }
    }
}
