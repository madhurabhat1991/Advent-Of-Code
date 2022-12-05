using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;
using Helpers;

namespace _2022.Day05
{
    public class Day05 : Day<(List<Stack<Char>>, List<(int, int, int)>), String, String>
    {
        public override string DayNumber { get { return "05"; } }

        public override string PartOne((List<Stack<Char>>, List<(int, int, int)>) input)
        {
            var stacks = UseCrateMover(input, false);   // CrateMover 9000
            return TopCrates(stacks);
        }

        public override string PartTwo((List<Stack<Char>>, List<(int, int, int)>) input)
        {
            var stacks = UseCrateMover(input, true);    // CrateMover 9001
            return TopCrates(stacks);
        }

        /// <summary>
        /// Parse input
        /// </summary>
        /// <param name="input"></param>
        /// <returns>(List<Stack<Crate>>, List<(qty, from, to)>)</returns>
        public override (List<Stack<Char>>, List<(int, int, int)>) ProcessInput(string[] input)
        {
            // find the line number of stacks
            int stackLine = 0;
            for (int l = 0; l < input.Length; l++)
            {
                if (input[l][1] == '1') { stackLine = l; break; }
            }
            int stackCount = input[stackLine][input[stackLine].Length - 2].CharToInt();

            // find which stack each crate belongs to
            Stack<Char>[] stacks = new Stack<char>[stackCount];
            for (int l = stackLine - 1; l >= 0; l--)
            {
                var line = input[l];
                for (int i = 0; i < line.Length; i++)
                {
                    if (!Char.IsWhiteSpace(line[i]) && !new char[] { '[', ']' }.Contains(line[i]))
                    {
                        var stackNum = input[stackLine][i].CharToInt() - 1;
                        if (stacks[stackNum] == null) { stacks[stackNum] = new Stack<char>(); }
                        stacks[stackNum].Push(line[i]);
                    }
                }
            }

            // find instructions
            List<(int, int, int)> instructions = new List<(int, int, int)>();
            for (int l = stackLine + 2; l < input.Length; l++)
            {
                var ins = input[l]
                    .Split(new string[] { "move", "from", "to" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(r => Int32.Parse(r)).ToList();
                instructions.Add((ins[0], ins[1] - 1, ins[2] - 1));
            }

            return (stacks.ToList(), instructions);
        }

        /// <summary>
        /// Use crate mover
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isMover9001">true if cratemover 9001 is used, false otherwise</param>
        /// <returns></returns>
        private List<Stack<char>> UseCrateMover((List<Stack<Char>>, List<(int, int, int)>) input, bool isMover9001)
        {
            var stacks = input.Item1.DeepClone();
            var instr = input.Item2;

            foreach (var ins in instr)
            {
                var qty = ins.Item1;
                var from = ins.Item2;
                var to = ins.Item3;

                if (isMover9001) { UseCrateMover9001(stacks, qty, from, to); }
                else { UseCrateMover9000(stacks, qty, from, to); }
            }
            return stacks;
        }

        private List<Stack<char>> UseCrateMover9000(List<Stack<char>> stacks, int qty, int from, int to)
        {
            for (int m = 0; m < qty; m++)
            {
                stacks[to].Push(stacks[from].Pop());
            }
            return stacks;
        }

        private List<Stack<char>> UseCrateMover9001(List<Stack<char>> stacks, int qty, int from, int to)
        {
            Stack<char> temp = new Stack<char>();
            for (int m = 0; m < qty; m++)
            {
                temp.Push(stacks[from].Pop());
            }
            for (int m = 0; m < qty; m++)
            {
                stacks[to].Push(temp.Pop());
            }
            return stacks;
        }

        private string TopCrates(List<Stack<char>> stacks)
        {
            string code = "";
            foreach (var stack in stacks)
            {
                code += stack.Peek();
            }
            return code;
        }
    }
}
