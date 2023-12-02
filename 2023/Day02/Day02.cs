using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;

namespace _2023.Day02
{
    public class Day02 : Day<List<Game>, long, long>
    {
        public override string DayNumber { get { return "02"; } }

        public override long PartOne(List<Game> input)
        {
            return input.Where(g => !(g.Sets.Any(s => s.Red > 12 || s.Green > 13 || s.Blue > 14))).Sum(g => g.Id);
        }

        public override long PartTwo(List<Game> input)
        {
            return input.Sum(g => g.Sets.Max(s => s.Red) * g.Sets.Max(s => s.Green) * g.Sets.Max(s => s.Blue));
        }

        public override List<Game> ProcessInput(string[] input)
        {
            List<Game> games = new List<Game>();
            foreach (var line in input)
            {
                var splits = line.Split(':', StringSplitOptions.RemoveEmptyEntries);
                int gameNumber = Int32.Parse(splits[0].Replace("Game ", ""));
                var subsets = splits[1].Split(';').ToList();
                List<Set> sets = new List<Set>();
                foreach (var subset in subsets)
                {
                    var set = new Set();
                    var cubes = subset.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var cube in cubes)
                    {
                        var info = cube.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        int count = Int32.Parse(info[0]);
                        string color = info[1];
                        switch (color.ToLower())
                        {
                            case "red":
                                set.Red = count;
                                break;
                            case "green":
                                set.Green = count;
                                break;
                            case "blue":
                                set.Blue = count;
                                break;
                            default:
                                break;
                        }
                    }
                    sets.Add(set);
                }
                games.Add(new Game()
                {
                    Id = gameNumber,
                    Sets = sets
                });
            }
            return games;
        }
    }

    public class Game
    {
        public int Id { get; set; }
        public List<Set> Sets { get; set; }
    }

    public class Set
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
    }
}
