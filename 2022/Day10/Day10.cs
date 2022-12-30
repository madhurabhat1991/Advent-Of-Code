using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;
using Helpers;

namespace _2022.Day10
{
    public class Day10 : Day<List<String>, long, object>
    {
        public override string DayNumber { get { return "10"; } }

        public override long PartOne(List<string> input)
        {
            long cycles = 1;
            int step = 20;
            long strength = 0;

            int X = 1;
            foreach (var line in input)
            {
                var cmd = line.Split(" ");
                var ins = cmd[0];
                var V = cmd.Length > 1 ? Int32.Parse(cmd[1]) : 0;

                var localCycle = instructions[ins];
                while (localCycle > 0)
                {
                    if (cycles > 0 && cycles <= 220 && cycles == step)
                    {
                        strength += X * cycles;
                        step += 40;
                    }
                    if (ins.Equals("addx"))
                    {
                        X = localCycle == 1 ? X + V : X;
                    }
                    localCycle--;
                    cycles++;
                }
            }
            return strength;
        }

        public override object PartTwo(List<string> input)
        {
            var crt = new char[6, 40];
            int row = 0, col = 0;

            var sprite = new char[1, 40];
            sprite.MarkGrid(new List<(int, int)>() { (0, 0), (1, 0), (2, 0) }, lit, dark);
            //sprite.Print();

            int X = 1;
            foreach (var line in input)
            {
                var cmd = line.Split(" ");
                var ins = cmd[0];
                var val = cmd.Length > 1 ? Int32.Parse(cmd[1]) : 0;

                var localCycle = instructions[ins];
                while (localCycle > 0)
                {
                    crt[row, col] = sprite[0, col];
                    //crt.Print();

                    if (ins.Equals("addx"))
                    {
                        if (localCycle == 1)
                        {
                            X += val;

                            sprite = new char[1, 40];
                            List<(int, int)> spritePixels = new List<(int, int)>();
                            if (X - 1 >= 0 && X - 1 < sprite.GetLength(1)) { spritePixels.Add((X - 1, 0)); }
                            if (X >= 0 && X < sprite.GetLength(1)) { spritePixels.Add((X, 0)); }
                            if (X + 1 >= 0 && X + 1 < sprite.GetLength(1)) { spritePixels.Add((X + 1, 0)); }
                            sprite.MarkGrid(spritePixels, lit, dark);
                            //sprite.Print();
                        }
                    }
                    localCycle--;
                    row = (col == crt.GetLength(1) - 1) ? row + 1 : row;
                    col = (col == crt.GetLength(1) - 1) ? 0 : col + 1;
                }

            }
            crt.Print(false);
            return null;
        }

        public override List<string> ProcessInput(string[] input)
        {
            return input.ToList();
        }

        private Dictionary<string, int> instructions = new Dictionary<string, int>
        {
            { "noop", 1}, {"addx", 2}
        };

        private const char lit = '#';
        private const char dark = '.';
    }
}
