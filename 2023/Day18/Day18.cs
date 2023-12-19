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
            var trench = (0, 0);
            SortedSet<(int, int)> positions = new SortedSet<(int, int)>() { trench };   // SortedSet<(row, col)>
            foreach (var line in input)
            {
                for (int i = 1; i <= line.Item2; i++)
                {
                    switch (line.Item1)
                    {
                        case 'U':
                            trench = (trench.Item1 - 1, trench.Item2);
                            break;
                        case 'D':
                            trench = (trench.Item1 + 1, trench.Item2);
                            break;
                        case 'R':
                            trench = (trench.Item1, trench.Item2 + 1);
                            break;
                        case 'L':
                            trench = (trench.Item1, trench.Item2 - 1);
                            break;
                        default:
                            break;
                    }
                    positions.Add(trench);
                }
            }
            //positions.Select(r => (r.Item2, r.Item1)).ToList().MarkGrid('#','.').Print(false);
            long trenches = 0;
            long row = 0;
            while (true)
            {
                bool inside = true;
                if (positions.Any(r => r.Item1 == row))
                {
                    var rows = positions.Where(r => r.Item1 == row).ToList();
                    trenches += 1;  // left most boundary
                    for (int i = 0; i < rows.Count - 1; i++)
                    {
                        // right next to each otheer
                        if (rows[i + 1].Item2 - rows[i].Item2 == 1)
                        {
                            trenches += 1;
                        }
                        // gap between boundaries
                        else
                        {
                            if (inside)
                            {
                                trenches += rows[i + 1].Item2 - rows[i].Item2;
                                inside = false;
                            }
                            else
                            {
                                inside = true;
                            }
                        }
                    }
                }
                else
                {
                    break;
                }
                row++;
            }
            return trenches;
        }

        public override long PartTwo(List<(char, int, string)> input)
        {
            throw new NotImplementedException();
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

        private const char Up = 'U';
        private const char Down = 'D';
        private const char Left = 'L';
        private const char Right = 'R';
    }
}
