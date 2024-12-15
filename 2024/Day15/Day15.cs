using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;

namespace _2024.Day15
{
    public class Day15 : Day<(char[,], string), long, long>
    {
        public override string DayNumber { get { return "15"; } }

        public override long PartOne((char[,], string) input)
        {
            var grid = input.Item1;
            var moves = input.Item2;
            // stack any positions to be moved, clear stack when you hit wall
            foreach (var move in moves)
            {
                var moveThis = grid.GetCellsEqualToValue(Robot).First();
                Stack<((char, int, int), (char, int, int))> stack = new Stack<((char, int, int), (char, int, int))>();  // <(moveFrom, moveTo)>
                switch (move)
                {
                    case Up:
                        while (true)
                        {
                            var moveThere = grid.GetTopCell(moveThis.Item2, moveThis.Item3);
                            if (moveThere.Item1 == Empty) { stack.Push((moveThis, moveThere)); break; }
                            if (moveThere.Item1 == Wall) { stack.Clear(); break; }
                            if (moveThere.Item1 == Box)
                            {
                                stack.Push((moveThis, moveThere));
                                moveThis = moveThere;
                            }
                        }
                        break;
                    case Down:
                        while (true)
                        {
                            var moveThere = grid.GetBottomCell(moveThis.Item2, moveThis.Item3);
                            if (moveThere.Item1 == Empty) { stack.Push((moveThis, moveThere)); break; }
                            if (moveThere.Item1 == Wall) { stack.Clear(); break; }
                            if (moveThere.Item1 == Box)
                            {
                                stack.Push((moveThis, moveThere));
                                moveThis = moveThere;
                            }
                        }
                        break;
                    case Left:
                        while (true)
                        {
                            var moveThere = grid.GetLeftCell(moveThis.Item2, moveThis.Item3);
                            if (moveThere.Item1 == Empty) { stack.Push((moveThis, moveThere)); break; }
                            if (moveThere.Item1 == Wall) { stack.Clear(); break; }
                            if (moveThere.Item1 == Box)
                            {
                                stack.Push((moveThis, moveThere));
                                moveThis = moveThere;
                            }
                        }
                        break;
                    case Right:
                        while (true)
                        {
                            var moveThere = grid.GetRightCell(moveThis.Item2, moveThis.Item3);
                            if (moveThere.Item1 == Empty) { stack.Push((moveThis, moveThere)); break; }
                            if (moveThere.Item1 == Wall) { stack.Clear(); break; }
                            if (moveThere.Item1 == Box)
                            {
                                stack.Push((moveThis, moveThere));
                                moveThis = moveThere;
                            }
                        }
                        break;
                }
                while (stack.Count > 0)
                {
                    var s = stack.Pop();
                    var moveFrom = s.Item1;
                    var moveTo = s.Item2;
                    grid[moveTo.Item2, moveTo.Item3] = moveFrom.Item1;
                    grid[moveFrom.Item2, moveFrom.Item3] = Empty;
                }
            }
            //grid.Print(false);
            // calculate GPS coordinates
            long sum = 0;
            var boxes = grid.GetCellsEqualToValue(Box);
            foreach (var box in boxes) { sum += 100 * box.Item2 + box.Item3; }
            return sum;
        }

        public override long PartTwo((char[,], string) input)
        {
            var oldGrid = input.Item1;
            var moves = input.Item2;
            // widen the grid - double the columns
            char[,] grid = new char[oldGrid.GetLength(0), oldGrid.GetLength(1) * 2];
            for (int or = 0, nr = 0; or < oldGrid.GetLength(0); or++, nr++)
            {
                for (int oc = 0, nc = 0; oc < oldGrid.GetLength(1); oc++, nc++)
                {
                    char newTile1 = Empty, newTile2 = Empty;
                    switch (oldGrid[or, oc])
                    {
                        case Wall:
                            newTile1 = newTile2 = Wall;
                            break;
                        case Box:
                            newTile1 = WideBoxLeft;
                            newTile2 = WideBoxRight;
                            break;
                        case Empty:
                            newTile1 = newTile2 = Empty;
                            break;
                        case Robot:
                            newTile1 = Robot;
                            newTile2 = Empty;
                            break;
                    }
                    grid[nr, nc++] = newTile1;
                    grid[nr, nc] = newTile2;
                }
            }
            // Queue if there is another half of box to be moved, stack any positions to be moved, clear stack and queue when you hit wall
            //grid.Print(false);
            foreach (var move in moves)
            {
                Queue<(char, int, int)> moveList = new Queue<(char, int, int)>([grid.GetCellsEqualToValue(Robot).First()]);
                Stack<((char, int, int), (char, int, int))> stack = new Stack<((char, int, int), (char, int, int))>();  // <(moveFrom, moveTo)>
                while (moveList.Count > 0)
                {
                    var moveThis = moveList.Dequeue();
                    switch (move)
                    {
                        case Up:
                            while (true)
                            {
                                var moveThere = grid.GetTopCell(moveThis.Item2, moveThis.Item3);
                                if (moveThere.Item1 == Empty) { stack.Push((moveThis, moveThere)); break; }
                                if (moveThere.Item1 == Wall) { stack.Clear(); moveList.Clear(); break; }
                                if (moveThere.Item1 == WideBoxRight || moveThere.Item1 == WideBoxLeft)
                                {
                                    stack.Push((moveThis, moveThere));
                                    if (moveThere.Item1 == WideBoxRight) { moveList.Enqueue(grid.GetLeftCell(moveThere.Item2, moveThere.Item3)); }
                                    else if (moveThere.Item1 == WideBoxLeft) { moveList.Enqueue(grid.GetRightCell(moveThere.Item2, moveThere.Item3)); }
                                    moveThis = moveThere;
                                }
                            }
                            break;
                        case Down:
                            while (true)
                            {
                                var moveThere = grid.GetBottomCell(moveThis.Item2, moveThis.Item3);
                                if (moveThere.Item1 == Empty) { stack.Push((moveThis, moveThere)); break; }
                                if (moveThere.Item1 == Wall) { stack.Clear(); moveList.Clear(); break; }
                                if (moveThere.Item1 == WideBoxRight || moveThere.Item1 == WideBoxLeft)
                                {
                                    stack.Push((moveThis, moveThere));
                                    if (moveThere.Item1 == WideBoxRight) { moveList.Enqueue(grid.GetLeftCell(moveThere.Item2, moveThere.Item3)); }
                                    else if (moveThere.Item1 == WideBoxLeft) { moveList.Enqueue(grid.GetRightCell(moveThere.Item2, moveThere.Item3)); }
                                    moveThis = moveThere;
                                }
                            }
                            break;
                        case Left:
                            while (true)
                            {
                                var moveThere = grid.GetLeftCell(moveThis.Item2, moveThis.Item3);
                                if (moveThere.Item1 == Empty) { stack.Push((moveThis, moveThere)); break; }
                                if (moveThere.Item1 == Wall) { stack.Clear(); moveList.Clear(); break; }
                                if (moveThere.Item1 == WideBoxRight || moveThere.Item1 == WideBoxLeft)
                                {
                                    stack.Push((moveThis, moveThere));
                                    moveThis = moveThere;
                                }
                            }
                            break;
                        case Right:
                            while (true)
                            {
                                var moveThere = grid.GetRightCell(moveThis.Item2, moveThis.Item3);
                                if (moveThere.Item1 == Empty) { stack.Push((moveThis, moveThere)); break; }
                                if (moveThere.Item1 == Wall) { stack.Clear(); moveList.Clear(); break; }
                                if (moveThere.Item1 == WideBoxRight || moveThere.Item1 == WideBoxLeft)
                                {
                                    stack.Push((moveThis, moveThere));
                                    moveThis = moveThere;
                                }
                            }
                            break;
                    }
                }
                while (stack.Count > 0)
                {
                    var s = stack.Pop();
                    var moveFrom = s.Item1;
                    var moveTo = s.Item2;
                    grid[moveTo.Item2, moveTo.Item3] = moveFrom.Item1;
                    grid[moveFrom.Item2, moveFrom.Item3] = Empty;
                }
            }
            //grid.Print(false);
            // calculate GPS coordinates
            long sum = 0;
            var boxes = grid.GetCellsEqualToValue(WideBoxLeft);
            foreach (var box in boxes) { sum += 100 * box.Item2 + box.Item3; }
            return sum;
        }

        public override (char[,], string) ProcessInput(string[] input)
        {
            var blocks = input.Blocks();
            var grid = blocks[0].ToArray().CreateGrid2D();
            var moves = String.Join("", blocks[1]);
            return (grid, moves);
        }

        private const char Up = '^';
        private const char Down = 'v';
        private const char Left = '<';
        private const char Right = '>';

        private const char Robot = '@';
        private const char Wall = '#';
        private const char Box = 'O';
        private const char Empty = '.';

        private const char WideBoxLeft = '[';
        private const char WideBoxRight = ']';
    }
}
