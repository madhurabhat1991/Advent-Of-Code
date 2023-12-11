using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2023.Day10
{
    public class Day10 : Day<char[,], long, long>
    {
        public override string DayNumber { get { return "10"; } }

        public override long PartOne(char[,] input)
        {
            return FindFarthestStep(input).Item1;
        }

        public override long PartTwo(char[,] input)
        {
            long inside = 0;

            var part1 = FindFarthestStep(input);
            var animal = part1.Item2;
            var visited = part1.Item3;

            // count vertical pipes in each row
            var vPos = visited.Select(r => (r.Item2, r.Item3)).ToList();
            for (int r = 0; r < input.GetLength(0); r++)
            {
                long verticals = 0;
                bool lActive = false, fActive = false;
                for (int c = 0; c < input.GetLength(1); c++)
                {
                    var item = input[r, c];
                    if (vPos.Contains((r, c)))                                              // for those visited on animal path
                    {
                        if (item == 'S') { item = animal.Item1; }                           // if animal then use real char what is under
                        if (item != '-') { verticals++; }                                   // any pipe other than '-' counts as vertical pipe
                        if (item == 'L') { lActive = true; }                                // encountered L
                        if (item == 'J' && lActive) { verticals -= 2; lActive = false; }    // if J is after L (even after L*--*J) counts as horizontal, remove 2, 1 for each L & J
                        if (item == '7' && lActive) { verticals -= 1; lActive = false; }    // if 7 is after L (even after L*--*7) counts as 1 vertical, remove 1, any 1 for L or 7
                        if (item == 'F') { fActive = true; }                                // encountered F
                        if (item == '7' && fActive) { verticals -= 2; fActive = false; }    // if 7 is after F (even after F*--*7) counts as horizontal, remove 2, 1 for each F & 7
                        if (item == 'J' && fActive) { verticals -= 1; fActive = false; }    // if J is after F (even after F*--*J) counts as 1 vertical, remove 1, any 1 for F or J
                    }
                    else if (!vPos.Contains((r, c)) && verticals % 2 > 0) { inside++; }     // for anything not on path, inside if odd verticals, outside if even
                }
            }
            return inside;
        }

        public override char[,] ProcessInput(string[] input)
        {
            return input.CreateGrid2D();
        }

        private List<char> Connections(char pipe, char nPos)
        {
            if (pipe == '|')
            {
                if (nPos == 'N') { return new List<char> { '|', '7', 'F' }; }
                if (nPos == 'E' || nPos == 'W') { return new List<char>(); }
                if (nPos == 'S') { return new List<char> { '|', 'J', 'L' }; }
            }
            if (pipe == '-')
            {
                if (nPos == 'N' || nPos == 'S') { return new List<char>(); }
                if (nPos == 'E') { return new List<char> { '-', 'J', '7' }; }
                if (nPos == 'W') { return new List<char> { '-', 'L', 'F' }; }
            }
            if (pipe == 'L')
            {
                if (nPos == 'N') { return new List<char> { '|', '7', 'F' }; }
                if (nPos == 'E') { return new List<char> { '-', 'J', '7' }; }
                if (nPos == 'S' || nPos == 'W') { return new List<char>(); }
            }
            if (pipe == 'J')
            {
                if (nPos == 'N') { return new List<char> { '|', '7', 'F' }; }
                if (nPos == 'E' || nPos == 'S') { return new List<char>(); }
                if (nPos == 'W') { return new List<char> { '-', 'L', 'F' }; }
            }
            if (pipe == '7')
            {
                if (nPos == 'N' || nPos == 'E') { return new List<char>(); }
                if (nPos == 'S') { return new List<char> { '|', 'J', 'L' }; }
                if (nPos == 'W') { return new List<char> { '-', 'L', 'F' }; }
            }
            if (pipe == 'F')
            {
                if (nPos == 'N' || nPos == 'W') { return new List<char>(); }
                if (nPos == 'E') { return new List<char> { '-', 'J', '7' }; }
                if (nPos == 'S') { return new List<char> { '|', 'J', 'L' }; }
            }
            if (pipe == '.') { return new List<char>(); }
            return new List<char>();
        }

        private (char, (char, int, int)) WhatIsAnimal(char[,] input)
        {
            var animal = input.GetCellsEqualToValue('S').First();
            var neighbors = input.GetNeighbors(animal.Item2, animal.Item3, false);
            List<char> possible = new List<char>();
            foreach (var neighbor in neighbors)
            {
                // what is in the south of animal's north neighbor
                if (neighbor.Item2 < animal.Item2) { possible.AddRange(Connections(neighbor.Item1, 'S')); }
                // what is in the west of animal's east neighbor
                if (neighbor.Item3 > animal.Item3) { possible.AddRange(Connections(neighbor.Item1, 'W')); }
                // what is in the north of animal's south neighbor
                if (neighbor.Item2 > animal.Item2) { possible.AddRange(Connections(neighbor.Item1, 'N')); }
                // what is in the east of animal's west neighbor
                if (neighbor.Item3 < animal.Item3) { possible.AddRange(Connections(neighbor.Item1, 'E')); }
            }
            // whatever is under animal will be repeated as possible pipe by 2 of its neighbors - so distinct count > 1 is under animal
            // (actual pipe, (S, row, col))
            return (possible.GroupBy(r => r).Select(r => new { Pipe = r.Key, Count = r.Count() }).Where(r => r.Count > 1).First().Pipe, animal);
        }

        private (long, (char, (char, int, int)), List<(char, int, int)>) FindFarthestStep(char[,] input)
        {
            long sum = 0;
            long[,] trace = new long[input.GetLength(0), input.GetLength(1)];   // can use this to visualize steps
            var animal = WhatIsAnimal(input);
            var animalChar = animal.Item1;
            var animalPos = animal.Item2;
            List<(char, int, int)> visited = new List<(char, int, int)>() { animalPos };    // animal's position
            List<(char, int, int)> connected = new List<(char, int, int)>() { (animalChar, animalPos.Item2, animalPos.Item3) }; // animal's position with real char
            // until the connected points meet
            while (!(connected.Count == 2 && (connected.Select(r => r.Item2).Distinct().Count() == 1) && connected.Select(r => r.Item3).Distinct().Count() == 1))
            {
                var temp = new List<(char, int, int)>();
                foreach (var connect in connected)
                {
                    var neighbors = input.GetNeighbors(connect.Item2, connect.Item3, false);
                    foreach (var neighbor in neighbors)
                    {
                        if (!visited.Contains(neighbor))
                        {
                            if (
                                (neighbor.Item2 < connect.Item2 && Connections(connect.Item1, 'N').Contains(neighbor.Item1)) ||  // north
                                (neighbor.Item3 > connect.Item3 && Connections(connect.Item1, 'E').Contains(neighbor.Item1)) ||  // east
                                (neighbor.Item2 > connect.Item2 && Connections(connect.Item1, 'S').Contains(neighbor.Item1)) ||  // south
                                (neighbor.Item3 < connect.Item3 && Connections(connect.Item1, 'W').Contains(neighbor.Item1))     // west
                                )
                            {
                                temp.Add(neighbor);
                            }
                        }
                    }
                }
                connected = temp.ToList();
                visited.AddRange(connected);
                sum++;
                //trace.SetCellsToValue(connected.Select(r => (r.Item2, r.Item3)).ToList(), sum);
            }
            //trace.Print();
            return (sum, animal, visited);
        }
    }
}
