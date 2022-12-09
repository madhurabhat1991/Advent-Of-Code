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
                Tails = new List<Knot>() { }
            };
            Enumerable.Range(1, 9).ToList().ForEach(r => rope.Tails.Add(new Knot(Char.Parse(r.ToString()))));
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

        private const char Head = 'H';
        private const char Tail = 'T';

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
                    // if prev and current do not overlap
                    if (prev.Position != current.Position)
                    {
                        // if prev and current are not adjacent
                        if (current.Position != (prev.Position.Item1 - 1, prev.Position.Item2)         // current at top
                            && current.Position != (prev.Position.Item1 - 1, prev.Position.Item2 + 1)  // current at top right
                            && current.Position != (prev.Position.Item1, prev.Position.Item2 + 1)      // current at right
                            && current.Position != (prev.Position.Item1 + 1, prev.Position.Item2 + 1)  // current at bottom right
                            && current.Position != (prev.Position.Item1 + 1, prev.Position.Item2)      // current at bottom
                            && current.Position != (prev.Position.Item1 + 1, prev.Position.Item2 - 1)  // current at bottom left
                            && current.Position != (prev.Position.Item1, prev.Position.Item2 - 1)      // current at left
                            && current.Position != (prev.Position.Item1 - 1, prev.Position.Item2 - 1)) // current at top left
                        {
                            // move current
                            // if prev and current are on same row
                            if (prev.Position.Item1 == current.Position.Item1)
                            {
                                if (prev.Position.Item2 > current.Position.Item2) { current.Position.Item2 += 1; }
                                else { current.Position.Item2 -= 1; }
                            }
                            // if prev and current are on same col
                            else if (prev.Position.Item2 == current.Position.Item2)
                            {
                                if (prev.Position.Item1 > current.Position.Item1) { current.Position.Item1 += 1; }
                                else { current.Position.Item1 -= 1; }
                            }
                            // if prev and current are far
                            else
                            {
                                // prev is on top right
                                if (prev.Position.Item1 < current.Position.Item1 && prev.Position.Item2 > current.Position.Item2)
                                {
                                    current.Position = (current.Position.Item1 - 1, current.Position.Item2 + 1);
                                }
                                // prev is on bottom right
                                else if (prev.Position.Item1 > current.Position.Item1 && prev.Position.Item2 > current.Position.Item2)
                                {
                                    current.Position = (current.Position.Item1 + 1, current.Position.Item2 + 1);
                                }
                                // prev is on bottom left
                                else if (prev.Position.Item1 > current.Position.Item1 && prev.Position.Item2 < current.Position.Item2)
                                {
                                    current.Position = (current.Position.Item1 + 1, current.Position.Item2 - 1);
                                }
                                // prev is on top left
                                else
                                {
                                    current.Position = (current.Position.Item1 - 1, current.Position.Item2 - 1);
                                }
                            }
                        }
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
        public List<Knot> Tails { get; set; }
    }

    public class Knot
    {
        public char Name { get; set; }
        public (int, int) Position = (0, 0);
        public Knot(char name)
        {
            this.Name = name;
        }
    }
}
