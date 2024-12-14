using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;

namespace _2024.Day14
{
    public class Day14 : Day<List<((int, int), (int, int))>, long, long>
    {
        public override string DayNumber { get { return "14"; } }

        public override long PartOne(List<((int, int), (int, int))> input)
        {
            var robots = input.ToList();
            int maxX = input.Select(x => x.Item1).Max(x => x.Item1) + 1;
            int maxY = input.Select(x => x.Item1).Max(x => x.Item2) + 1;
            int timer = 1;
            while (timer <= 100)
            {
                for (int i = 0; i < robots.Count; i++)
                {
                    var robot = robots[i];
                    var position = robot.Item1;
                    var velocity = robot.Item2;
                    int px = position.Item1, py = position.Item2;
                    int vx = velocity.Item1, vy = velocity.Item2;
                    if (vx >= 0) { px = px + vx > maxX - 1 ? px + vx - maxX : px + vx; }
                    if (vx < 0) { px = px + vx < 0 ? px + vx + maxX : px + vx; }
                    if (vy >= 0) { py = py + vy > maxY - 1 ? py + vy - maxY : py + vy; }
                    if (vy < 0) { py = py + vy < 0 ? py + vy + maxY : py + vy; }
                    robot.Item1 = (px, py);
                    robots[i] = robot;
                }
                timer++;
            }
            var first = robots.Where(r => r.Item1.Item1 >= 0 && r.Item1.Item1 <= ((maxX / 2) - 1) && r.Item1.Item2 >= 0 && r.Item1.Item2 <= ((maxY / 2) - 1)).Count();
            var second = robots.Where(r => r.Item1.Item1 >= ((maxX / 2) + 1) && r.Item1.Item1 <= maxX - 1 && r.Item1.Item2 >= 0 && r.Item1.Item2 <= ((maxY / 2) - 1)).Count();
            var third = robots.Where(r => r.Item1.Item1 >= 0 && r.Item1.Item1 <= ((maxX / 2) - 1) && r.Item1.Item2 >= ((maxY / 2) + 1) && r.Item1.Item2 <= maxY - 1).Count();
            var fourth = robots.Where(r => r.Item1.Item1 >= ((maxX / 2) + 1) && r.Item1.Item1 <= maxX - 1 && r.Item1.Item2 >= ((maxY / 2) + 1) && r.Item1.Item2 <= maxY - 1).Count();
            return first * second * third * fourth;
        }

        public override long PartTwo(List<((int, int), (int, int))> input)
        {
            var robots = input.ToList();
            int maxX = input.Select(x => x.Item1).Max(x => x.Item1) + 1;
            int maxY = input.Select(x => x.Item1).Max(x => x.Item2) + 1;
            int timer = 1;
            while (true)
            {
                for (int i = 0; i < robots.Count; i++)
                {
                    var robot = robots[i];
                    var position = robot.Item1;
                    var velocity = robot.Item2;
                    int px = position.Item1, py = position.Item2;
                    int vx = velocity.Item1, vy = velocity.Item2;
                    if (vx >= 0) { px = px + vx > maxX - 1 ? px + vx - maxX : px + vx; }
                    if (vx < 0) { px = px + vx < 0 ? px + vx + maxX : px + vx; }
                    if (vy >= 0) { py = py + vy > maxY - 1 ? py + vy - maxY : py + vy; }
                    if (vy < 0) { py = py + vy < 0 ? py + vy + maxY : py + vy; }
                    robot.Item1 = (px, py);
                    robots[i] = robot;
                }
                // check if robots formed a xmas tree by forming grid and flattening it to find 20 consecutive robots
                // 20 is an assumption given the size of the grid
                // this takes 57s, should probably optimize but this prints out nice picture so keeping it for now
                if (XmasTree(robots)) { break; }
                timer++;
            }
            return timer;
        }

        public override List<((int, int), (int, int))> ProcessInput(string[] input)
        {
            List<((int, int), (int, int))> robots = new List<((int, int), (int, int))>();
            foreach (var line in input)
            {
                var info = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var ps = info[0].Substring(2, info[0].Length - 2).Split(',').StringArrayToIntArray();
                (int, int) position = (ps[0], ps[1]);
                var vs = info[1].Substring(2, info[1].Length - 2).Split(',').StringArrayToIntArray();
                (int, int) velocity = (vs[0], vs[1]);
                robots.Add((position, velocity));
            }
            return robots;
        }

        private bool XmasTree(List<((int, int), (int, int))> input)
        {
            char[,] grid = new char[input.Select(x => x.Item1).Max(x => x.Item2) + 1, input.Select(x => x.Item1).Max(x => x.Item1) + 1];
            grid.MarkGrid(' ');
            foreach (var robot in input)
            {
                var position = robot.Item1;
                var velocity = robot.Item2;
                grid[position.Item2, position.Item1] = 'x';
            }
            var flat = grid.FlattenGrid2D();
            if (flat.Contains("xxxxxxxxxxxxxxxxxxxx"))
            {
                grid.Print(false);
                return true;
            }
            return false;
        }
    }
}
