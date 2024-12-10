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
            // Cannot use Queue as I need to know available leftmost space for full block anywhere not just first available space
            // performance = 8s, could be better by managing index and find toMoveNode based on valid index only?
            LinkedList<(int, int)> ll = new LinkedList<(int, int)>();   // LinkedList<(file id, count)>
            for (int i = 0; i < input.Length; i++)
            {
                var block = Int32.Parse(input[i].ToString());
                ll.AddLast((i % 2 == 0 ? i / 2 : -1, block));
            }
            for (var node = ll.Last; node != null; node = node.Previous)
            {
                if (node.Value.Item1 != -1)     // file block
                {
                    int nodeindex = ll.TakeWhile(n => n != node.Value).Count();
                    var moveList = ll.Where(r => r.Item1 == -1 && r.Item2 >= node.Value.Item2).ToList();
                    if (moveList != null && moveList.Any())
                    {
                        var toMoveNode = ll.Find(moveList.First());     // find a node that can accomodate current node
                        int toMoveNodeindex = ll.TakeWhile(n => n != toMoveNode.Value).Count();
                        if (toMoveNodeindex > nodeindex) { continue; }  // move only to the left of the node
                        if (toMoveNode.Value.Item2 > node.Value.Item2)  // account for extra space available
                        {
                            ll.AddAfter(toMoveNode, (-1, toMoveNode.Value.Item2 - node.Value.Item2));
                        }
                        toMoveNode.Value = node.Value;
                        node.Value = (-1, node.Value.Item2);
                    }
                }
            }
            // calculate checksum
            long checksum = 0;
            var currentNode = ll.First;
            int counter = 0;
            while (currentNode != null)
            {
                for (int x = 0; x < currentNode.Value.Item2; x++) { checksum += counter++ * (currentNode.Value.Item1 == -1 ? 0 : currentNode.Value.Item1); }
                currentNode = currentNode.Next;
            }
            return checksum;
        }

        public override string ProcessInput(string[] input)
        {
            return input[0];
        }
    }
}
