using System;
using System.Collections.Generic;
using System.Text;
using Template;
using System.Linq;
using Helpers;

namespace _2023.Day18
{
    public class Day18 : Day<List<(char, int, string)>, long, long>
    {
        public override string DayNumber { get { return "18"; } }

        public override long PartOne(List<(char, int, string)> input)
        {
            // similar to 2023 Day 10 - even/odd rule. Each direction is marked with pipes - |, -, F, 7, J, L
            var trench = (0, 0);
            Dictionary<(int, int), char> positions = new Dictionary<(int, int), char>();   // Dictionary<(row, col), pipe>
            for (int l = 0; l < input.Count; l++)
            {
                var line = input[l];
                var turn = line.Item1;
                var distance = line.Item2;
                var nextTurn = l < input.Count - 1 ? input[l + 1].Item1 : l == input.Count - 1 ? input[0].Item1 : Char.MinValue;
                Dig(turn, distance, nextTurn, ref trench, ref positions);
            }
            return CalculateCapacity(positions);
        }

        public override long PartTwo(List<(char, int, string)> input)
        {
            // using even/odd rule takes very long time to run
            //var trench = (0, 0);
            //Dictionary<(int, int), char> positions = new Dictionary<(int, int), char>();   // Dictionary<(row, col), pipe>
            //for (int l = 0; l < input.Count; l++)
            //{
            //    var line = input[l].Item3;
            //    var turn = DetermineTurn(line);
            //    var distance = line.Substring(1, 5).HexToDecimal();
            //    var nextTurn = l < input.Count - 1 ? DetermineTurn(input[l + 1].Item3) : l == input.Count - 1 ? DetermineTurn(input[0].Item3) : Char.MinValue;
            //    Dig(turn, distance, nextTurn, ref trench, ref positions);
            //}
            //return CalculateCapacity(positions);

            return 0;
        }

        public override List<(char, int, string)> ProcessInput(string[] input)
        {
            List<(char, int, string)> plan = new List<(char, int, string)>();
            foreach (var line in input)
            {
                var info = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                plan.Add((info[0][0], Int32.Parse(info[1]), info[2].Trim(new char[] { '(', ')' })));
            }
            return plan;
        }

        private enum Direction
        {
            Up = 'U',
            Down = 'D',
            Left = 'L',
            Right = 'R'
        }

        private readonly Dictionary<char, Direction> HexCodes = new Dictionary<char, Direction>()
        {
            { '0', Direction.Right },
            { '1', Direction.Down },
            { '2', Direction.Left },
            { '3', Direction.Up }
        };

        private void Dig(char turn, int distance, char nextTurn, ref (int, int) trench, ref Dictionary<(int, int), char> positions)
        {
            for (int i = 1; i <= distance; i++)
            {
                char path = '-';
                switch (turn)
                {
                    case (char)Direction.Up:
                        trench = (trench.Item1 - 1, trench.Item2);
                        path = '|';
                        if (i == distance && nextTurn != Char.MinValue)
                        {
                            if (nextTurn == (char)Direction.Right) { path = 'F'; }
                            else if (nextTurn == (char)Direction.Left) { path = '7'; }
                        }
                        break;
                    case (char)Direction.Down:
                        trench = (trench.Item1 + 1, trench.Item2);
                        path = '|';
                        if (i == distance && nextTurn != Char.MinValue)
                        {
                            if (nextTurn == (char)Direction.Right) { path = 'L'; }
                            else if (nextTurn == (char)Direction.Left) { path = 'J'; }
                        }
                        break;
                    case (char)Direction.Right:
                        trench = (trench.Item1, trench.Item2 + 1);
                        path = '-';
                        if (i == distance && nextTurn != Char.MinValue)
                        {
                            if (nextTurn == (char)Direction.Up) { path = 'J'; }
                            else if (nextTurn == (char)Direction.Down) { path = '7'; }
                        }
                        break;
                    case (char)Direction.Left:
                        trench = (trench.Item1, trench.Item2 - 1);
                        path = '-';
                        if (i == distance && nextTurn != Char.MinValue)
                        {
                            if (nextTurn == (char)Direction.Up) { path = 'L'; }
                            else if (nextTurn == (char)Direction.Down) { path = 'F'; }
                        }
                        break;
                    default:
                        break;
                }
                positions.Add(trench, path);
            }
        }

        private long CalculateCapacity(Dictionary<(int, int), char> positions)
        {
            // count vertical pipes in each row
            long inside = 0;
            int rMin = positions.Keys.Min(k => k.Item1), rMax = positions.Keys.Max(k => k.Item1);
            int cMin = positions.Keys.Min(k => k.Item2), cMax = positions.Keys.Max(k => k.Item2);
            int rLen = rMax + 1, cLen = cMax + 1;
            for (int r = rMin; r < rLen; r++)
            {
                long verticals = 0;
                bool lActive = false, fActive = false;
                for (int c = cMin; c < cLen; c++)
                {
                    if (positions.ContainsKey((r, c)))
                    {
                        inside++;
                        var item = positions[(r, c)];
                        if (item != '-') { verticals++; }                                   // any pipe other than '-' counts as vertical pipe
                        if (item == 'L') { lActive = true; }                                // encountered L
                        if (item == 'J' && lActive) { verticals -= 2; lActive = false; }    // if J is after L (even after L*--*J) counts as horizontal, remove 2, 1 for each L & J
                        if (item == '7' && lActive) { verticals -= 1; lActive = false; }    // if 7 is after L (even after L*--*7) counts as 1 vertical, remove 1, any 1 for L or 7
                        if (item == 'F') { fActive = true; }                                // encountered F
                        if (item == '7' && fActive) { verticals -= 2; fActive = false; }    // if 7 is after F (even after F*--*7) counts as horizontal, remove 2, 1 for each F & 7
                        if (item == 'J' && fActive) { verticals -= 1; fActive = false; }    // if J is after F (even after F*--*J) counts as 1 vertical, remove 1, any 1 for F or J
                    }
                    else if (!positions.ContainsKey((r, c)) && verticals % 2 > 0) { inside++; } // for anything not on path, inside if odd verticals, outside if even
                }
            }
            return inside;
        }

        private char DetermineTurn(string hexCode)
        {
            return (char)HexCodes[hexCode[hexCode.Length - 1]];
        }
    }
}
