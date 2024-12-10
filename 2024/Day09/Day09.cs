using System;
using System.Collections.Generic;
using System.Text;
using Template;

namespace _2024.Day09
{
    public class Day09 : Day<string, long, long>
    {
        public override string DayNumber { get { return "09"; } }

        public override long PartOne(string input)
        {
            Queue<(int, int)> q = new Queue<(int, int)>();                              // Queue<(file id, count)>
            int i = 0;                                                                  // current to queue
            int j = (input.Length - 1) % 2 == 0 ? input.Length - 1 : input.Length - 2;  // counter to fragment
            var toMove = Int32.Parse(input[j].ToString());                              // amount to fragment
            while (i < j)
            {
                var block = Int32.Parse(input[i].ToString());                           // current block
                if (i % 2 == 0)                                                         // file block
                {
                    q.Enqueue((i / 2, block));
                }
                else if (block > 0)                                                     // spaces left to fill
                {
                    while (block > 0 && i < j)                                          // fill until available to fragment
                    {
                        var fill = block >= toMove ? toMove : block;
                        q.Enqueue((j / 2, fill));
                        block -= fill;
                        toMove -= fill;
                        if (block >= 0 && toMove == 0)
                        {
                            j -= 2;
                            toMove = Int32.Parse(input[j].ToString());
                        }
                    }
                }
                i++;
                if (i == j && toMove > 0)                                               // remaining ones
                {
                    q.Enqueue((j / 2, toMove));
                }
            }
            // calculate checksum
            long checksum = 0;
            int counter = 0;
            while (q.Count > 0)
            {
                var block = q.Dequeue();
                for (int x = 0; x < block.Item2; x++) { checksum += counter++ * block.Item1; }
            }
            return checksum;
        }

        public override long PartTwo(string input)
        {
            Queue<(int, int)> q = new Queue<(int, int)>();                              // Queue<(file id, count)>
            Stack<(int, int)> s = new Stack<(int, int)>();                              // Stack<(file id, count)>
            int i = 0;                                                                  // current to queue
            int j = (input.Length - 1) % 2 == 0 ? input.Length - 1 : input.Length - 2;  // counter to fragment
            var toMove = Int32.Parse(input[j].ToString());                              // amount to fragment
            while (i < j)
            {
                var block = Int32.Parse(input[i].ToString());                           // current block
                if (i % 2 == 0)                                                         // file block
                {
                    q.Enqueue((i / 2, block));
                }
                else if (block > 0)                                                     // spaces left to fill
                {
                    while (toMove < block && i < j)                                     // fill only that is available to fragment
                    {
                        q.Enqueue((j / 2, toMove));
                        block -= toMove;
                        toMove = 0;
                        if (block >= 0 && toMove == 0)
                        {
                            j -= 2;
                            toMove = Int32.Parse(input[j].ToString());
                        }
                    }
                }
                i++;
                if (i == j && toMove > 0)                                               // remaining ones
                {
                    q.Enqueue((j / 2, toMove));
                }
            }
            // calculate checksum
            long checksum = 0;
            int counter = 0;
            while (q.Count > 0)
            {
                var block = q.Dequeue();
                for (int x = 0; x < block.Item2; x++) { checksum += counter++ * block.Item1; }
            }
            return checksum;
        }

        public override string ProcessInput(string[] input)
        {
            return input[0];
        }
    }
}
