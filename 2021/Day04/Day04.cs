using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;
using Helpers;

namespace _2021.Day04
{
    public class Day04 : Day<(List<int>, List<Board>), long, long>
    {
        public override string DayNumber { get { return "04"; } }

        public override long PartOne((List<int>, List<Board>) input)
        {
            var draws = input.Item1.DeepClone();
            var boards = input.Item2.DeepClone();

            return PlayBingo(draws, boards).FirstOrDefault();
        }

        public override long PartTwo((List<int>, List<Board>) input)
        {
            var draws = input.Item1.DeepClone();
            var boards = input.Item2.DeepClone();

            return PlayBingo(draws, boards).LastOrDefault();
        }

        public override (List<int>, List<Board>) ProcessInput(string[] input)
        {
            List<int> draws = input[0].Split(",", StringSplitOptions.RemoveEmptyEntries).StringArrayToIntList();

            input = input.Skip(2).ToArray();
            var blocks = input.Blocks();
            List<Board> boards = new List<Board>();
            int bNum = 0;
            foreach (var block in blocks)
            {
                var board = new Board()
                {
                    Number = bNum,
                    Rows = new List<List<int>>(),
                    Columns = new List<List<int>>(),
                    Bingo = false
                };
                block.ForEach(r =>
                {
                    var row = r.Split(" ", StringSplitOptions.RemoveEmptyEntries).StringArrayToIntList();
                    board.Rows.Add(row);
                });
                for (int index = 0; index < BoardDimension; index++)
                {
                    var col = new List<int>();
                    for (int row = 0; row < BoardDimension; row++)
                    {
                        col.Add(board.Rows[row][index]);
                    }
                    board.Columns.Add(col);
                }
                boards.Add(board);
                bNum++;
            }

            return (draws, boards);
        }

        private int BoardDimension = 5;

        private List<long> PlayBingo(List<int> draws, List<Board> boards)
        {
            List<long> winSums = new List<long>();
            foreach (var draw in draws)
            {
                foreach (var board in boards)
                {
                    if (board.Bingo) { continue; }
                    foreach (var row in (board.Rows.Concat(board.Columns)))
                    {
                        if (board.Bingo) { break; }
                        if (row.Contains(draw)) { row.RemoveAll(x => x == draw); }
                        if (row.Count == 0)
                        {
                            board.Bingo = true;
                            winSums.Add(CalculateWinSum(board, draw));
                        }
                    }
                }
            }
            return winSums;
        }

        private long CalculateWinSum(Board winner, long lastDraw)
        {
            long sum = 0;
            winner.Rows.ForEach(w => sum += w.Sum());
            return sum * lastDraw;
        }
    }

    [Serializable]
    public class Board
    {
        public int Number;
        public List<List<int>> Rows;
        public List<List<int>> Columns;
        public bool Bingo;
    }
}
