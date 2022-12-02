using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2021.Day15
{
    public class Day15 : Day<int[,], long, long>
    {
        public override string DayNumber { get { return "15"; } }

        public override long PartOne(int[,] input)
        {
            return FindPath(input);
        }

        public override long PartTwo(int[,] input)
        {
            return 0;
        }

        public override int[,] ProcessInput(string[] input)
        {
            return input.CreateGrid2D().CharToIntGrid2D();
        }

        private long FindPath(int[,] input)
        {
            // start at top left position
            int x = 0, y = 0, risk = input[x, y];
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(new Node(x, y, risk));
            HashSet<long> risks = new HashSet<long>();

            while (queue.Any())
            {
                var node = queue.Dequeue();
                // return if bottom right position is reached
                if (node.X == input.GetLength(0) - 1 && node.Y == input.GetLength(1) - 1)
                {
                    risks.Add(node.Risk);
                    continue;
                }
                // queue the position below the current position
                if (node.X < input.GetLength(0) - 1)
                {
                    x = node.X + 1;
                    y = node.Y;
                    risk = (node.X == 0 && node.Y == 0 ? 0 : node.Risk) + input[x, y];
                    queue.Enqueue(new Node(x, y, risk));
                }
                // queue the position right to the current position
                if (node.Y < input.GetLength(1) - 1)
                {
                    x = node.X;
                    y = node.Y + 1;
                    risk = (node.X == 0 && node.Y == 0 ? 0 : node.Risk) + input[x, y];
                    queue.Enqueue(new Node(x, y, risk));
                }
            }
            return risks.Min();
        }
    }

    class Node
    {
        public int X, Y, Risk;

        public Node(int x, int y, int risk)
        {
            this.X = x;
            this.Y = y;
            this.Risk = risk;
        }
    }
}
