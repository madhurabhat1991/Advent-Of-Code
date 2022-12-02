using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;

namespace _2022.Day02
{
    public class Day02 : Day<List<Tuple<Char, Char>>, long, long>
    {
        public override string DayNumber { get { return "02"; } }

        public override long PartOne(List<Tuple<char, char>> input)
        {
            return Play(input, false);
        }

        public override long PartTwo(List<Tuple<char, char>> input)
        {
            return Play(input, true);
        }

        public override List<Tuple<char, char>> ProcessInput(string[] input)
        {
            List<Tuple<Char, Char>> guide = new List<Tuple<char, char>>();
            foreach (var line in input)
            {
                var both = line.Split(" ");
                guide.Add(new Tuple<char, char>(both[0][0], both[1][0]));
            }
            return guide;
        }

        private const string Rock = "Rock";
        private const string Paper = "Paper";
        private const string Scissors = "Scissors";

        private const string Lose = "Lose";
        private const string Draw = "Draw";
        private const string Win = "Win";

        private Dictionary<Char, String> Opponent = new Dictionary<char, string>()
        {
            {'A', Rock },
            {'B', Paper},
            {'C', Scissors}
        };

        private Dictionary<Char, String> PlayerAssumption = new Dictionary<char, string>()
        {
            {'X', Rock },
            {'Y', Paper},
            {'Z', Scissors}
        };

        private Dictionary<Char, String> PlayerReality = new Dictionary<char, string>()
        {
            {'X', Lose },
            {'Y', Draw},
            {'Z', Win}
        };

        private Dictionary<String, String> Rules = new Dictionary<string, string>()
        {
            {Rock, Scissors },
            {Scissors, Paper},
            {Paper, Rock}
        };

        private Dictionary<String, Int32> ChoiceScore = new Dictionary<string, int>()
        {
            {Rock, 1 },
            {Paper, 2},
            {Scissors, 3}
        };

        private Dictionary<String, Int32> OutcomeScore = new Dictionary<string, int>()
        {
            {Lose, 0 },
            {Draw, 3},
            {Win, 6}
        };

        private long Play(List<Tuple<char, char>> input, bool reality)
        {
            long totalScore = 0;
            foreach (var round in input)
            {
                totalScore += GameRound(round.Item1, round.Item2, reality);
            }
            return totalScore;
        }

        private long GameRound(char opponent, char player, bool reality)
        {
            var oppChoice = Opponent[opponent];
            var plChoice = !reality ? PlayerAssumption[player] : PlayerReality[player];

            String plOutcome = "";
            if (Rules.Keys.Any(r => r.Equals(plChoice)))
            {
                plOutcome = Rules[plChoice].Equals(oppChoice) ? Win : oppChoice.Equals(plChoice) ? Draw : Lose;
            }
            else
            {
                plOutcome = plChoice;
                plChoice = plOutcome.Equals(Win)
                    ? Rules.Where(r => r.Value.Equals(oppChoice)).FirstOrDefault().Key
                    : plOutcome.Equals(Draw)
                    ? oppChoice
                    : Rules[oppChoice];
            }

            var score = ChoiceScore[plChoice] + OutcomeScore[plOutcome];
            return score;
        }
    }
}
