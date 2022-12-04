using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;

namespace _2022.Day02
{
    public class Day02 : Day<List<(Char, Char)>, long, long>
    {
        public override string DayNumber { get { return "02"; } }

        public override long PartOne(List<(Char, Char)> input)
        {
            return Play(input, false);  // assumption
        }

        public override long PartTwo(List<(Char, Char)> input)
        {
            return Play(input, true);   // reality
        }

        public override List<(Char, Char)> ProcessInput(string[] input)
        {
            List<(Char, Char)> guide = new List<(Char, Char)>();
            foreach (var line in input)
            {
                var both = line.Split(" ");
                guide.Add((both[0][0], both[1][0]));
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

        private Dictionary<Char, String> MyAssumptionGuide = new Dictionary<char, string>()
        {
            { 'X', Rock },
            { 'Y', Paper },
            { 'Z', Scissors }
        };

        private Dictionary<Char, String> MyRealityGuide = new Dictionary<char, string>()
        {
            { 'X', Lose },
            { 'Y', Draw },
            { 'Z', Win }
        };

        /// <summary>
        /// Dictionary<(I, Opponent), Outcome>
        /// </summary>
        private Dictionary<(String, String), String> Rules = new Dictionary<(string, string), string>()
        {
            // I chose key.item1, opponent chose key.item2 - I win
            { (Rock, Scissors), Win },
            { (Scissors, Paper), Win },
            { (Paper, Rock), Win },
            // I and opponent chose same - draw
            { (Rock, Rock), Draw },
            { (Scissors, Scissors), Draw },
            { (Paper, Paper), Draw },
            // I chose key.item1, opponent chose key.item2 - I lose
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
        /// <param name="input">List<(Opponent, I)></param>
        /// <param name="reality">true if it is reality, false if I assumed my guide</param>
        /// <returns></returns>
        private long Play(List<(Char, Char)> input, bool reality)
        {
            long totalScore = 0;
            foreach (var round in input)
            {
                totalScore += GameRound(round.Item1, round.Item2, reality);
            }
            return totalScore;
        }

        private long GameRound(char opponent, char me, bool reality)
        {
            var opponentChoice = OpponentGuide[opponent];
            string myChoice, myOutcome = "";

            if (!reality)                                       // assumption
            {
                myChoice = MyAssumptionGuide[me];
                myOutcome = Rules[(myChoice, opponentChoice)];
            }
            else                                                // reality
            {
                myOutcome = MyRealityGuide[me];
                myChoice = Rules.Where(r => r.Key.Item2.Equals(opponentChoice) && r.Value.Equals(myOutcome)).FirstOrDefault().Key.Item1;
            }

            return ChoiceScore[myChoice] + OutcomeScore[myOutcome];
        }
    }
}
