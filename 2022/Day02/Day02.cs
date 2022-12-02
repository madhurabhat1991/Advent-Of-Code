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
            return Play(input, false);  // assumption
        }

        public override long PartTwo(List<Tuple<char, char>> input)
        {
            return Play(input, true);   // reality
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

        private Dictionary<Char, String> OpponentGuide = new Dictionary<char, string>()
        {
            { 'A', Rock },
            { 'B', Paper },
            { 'C', Scissors }
        };

        private Dictionary<Char, String> PlayerGuideAssumption = new Dictionary<char, string>()
        {
            { 'X', Rock },
            { 'Y', Paper },
            { 'Z', Scissors }
        };

        private Dictionary<Char, String> PlayerGuideReality = new Dictionary<char, string>()
        {
            { 'X', Lose },
            { 'Y', Draw },
            { 'Z', Win }
        };

        /// <summary>
        /// Dictionary<(Player, Opponent), Outcome>
        /// </summary>
        private Dictionary<(String, String), String> Rules = new Dictionary<(string, string), string>()
        {
            // player chose key, opponent chose value - win
            { (Rock, Scissors), Win },
            { (Scissors, Paper), Win },
            { (Paper, Rock), Win },
            // player and opponent chose same - draw
            { (Rock, Rock), Draw },
            { (Scissors, Scissors), Draw },
            { (Paper, Paper), Draw },
            // player chose value, opponent chose key - lose
            { (Scissors, Rock), Lose },
            { (Paper, Scissors), Lose },
            { (Rock, Paper), Lose }
        };

        private Dictionary<String, Int32> ChoiceScore = new Dictionary<string, int>()
        {
            { Rock, 1 },
            { Paper, 2 },
            { Scissors, 3 }
        };

        private Dictionary<String, Int32> OutcomeScore = new Dictionary<string, int>()
        {
            { Lose, 0 },
            { Draw, 3 },
            { Win, 6 }
        };

        /// <summary>
        /// Play game
        /// </summary>
        /// <param name="input"></param>
        /// <param name="reality">true if it is reality, false if player assumed</param>
        /// <returns></returns>
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
            var opponentChose = OpponentGuide[opponent];
            var playerChose = !reality ? PlayerGuideAssumption[player] : PlayerGuideReality[player];

            String playerOutcome = "";
            if (!reality)                                       // assumption
            {
                playerOutcome = Rules[(playerChose, opponentChose)];
            }
            else                                                // reality
            {
                playerOutcome = playerChose;
                playerChose = Rules.Where(r => r.Key.Item2.Equals(opponentChose) && r.Value.Equals(playerOutcome)).FirstOrDefault().Key.Item1;
            }

            return ChoiceScore[playerChose] + OutcomeScore[playerOutcome];
        }
    }
}
