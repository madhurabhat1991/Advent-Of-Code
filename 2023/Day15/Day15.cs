using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2023.Day15
{
    public class Day15 : Day<List<string>, long, long>
    {
        public override string DayNumber { get { return "15"; } }

        public override long PartOne(List<string> input)
        {
            long sum = 0;
            foreach (var line in input)
            {
                sum += HashAlgorithm(line);
            }
            return sum;
        }

        public override long PartTwo(List<string> input)
        {
            long sum = 0;
            Dictionary<int, LinkedList<Lens>> boxes = new Dictionary<int, LinkedList<Lens>>();
            foreach (var line in input)
            {
                int opIndex = line.IndexOfAny(new char[] { '=', '-' });
                var label = line.Substring(0, opIndex).Trim();
                char op = line[opIndex];
                int focalLength = !string.IsNullOrEmpty(line[(opIndex + 1)..]) ? Int32.Parse(line[(opIndex + 1)..].Trim()) : 0;
                var boxNum = HashAlgorithm(label);

                var lenses = boxes.ContainsKey(boxNum) ? boxes[boxNum] : new LinkedList<Lens>();
                var lens = new Lens(label, focalLength);
                switch (op)
                {
                    // Contains() uses Lens' Equals method, implements Equatable<Lens>
                    case '-':
                        if (lenses.Contains(lens)) { lenses.Remove(lens); }
                        break;
                    case '=':
                        if (!lenses.Any()) { lenses.AddFirst(lens); }
                        else if (lenses.Contains(lens)) { lenses.Find(lens).Value.FocalLength = focalLength; }
                        else { lenses.AddLast(lens); }
                        break;
                    default:
                        break;
                }
                boxes[boxNum] = lenses;
            }
            foreach (var box in boxes.Keys)
            {
                int i = 1;
                foreach (var lens in boxes[box])
                {
                    sum += (box + 1) * (i) * (lens.FocalLength);
                    i++;
                }
            }
            return sum;
        }

        public override List<string> ProcessInput(string[] input)
        {
            return input[0].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        private int HashAlgorithm(string line)
        {
            int sum = 0;
            foreach (var c in line)
            {
                sum += (int)c;
                sum *= 17;
                sum %= 256;
            }
            return sum;
        }
    }

    public class Box
    {
        public int Number { get; set; }
        public LinkedList<Lens> Lenses { get; set; }
        public Box(int number)
        {
            this.Number = number;
            Lenses = new LinkedList<Lens>();
        }
    }

    public class Lens : IEquatable<Lens>
    {
        public string Name { get; set; }
        public int FocalLength { get; set; }
        public Lens(string name, int focalLength = 0)
        {
            this.Name = name;
            this.FocalLength = focalLength;
        }
        public bool Equals(Lens other)
        {
            return !string.IsNullOrEmpty(this.Name) && !string.IsNullOrEmpty(other.Name) && this.Name.Equals(other.Name);
        }
    }
}
