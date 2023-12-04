using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;
using Helpers;

namespace _2023.Day04
{
    public class Day04 : Day<List<(int, List<int>, List<int>)>, long, long>
    {
        public override string DayNumber { get { return "04"; } }

        public override long PartOne(List<(int, List<int>, List<int>)> input)
        {
            long sum = 0;
            foreach (var card in input)
            {
                sum += CalculatePoints(FindWinNumbers((card.Item2, card.Item3)));
            }
            return sum;
        }

        public override long PartTwo(List<(int, List<int>, List<int>)> input)
        {
            var cards = CardWinCount(input);
            foreach (var key in cards.Keys.OrderBy(k => k).ToList())
            {
                var card = cards[key];
                var winNum = card.Item1;
                var count = card.Item2;
                for (int i = key + 1, j = 0; j < winNum; i++, j++)
                {
                    cards[i] = (cards[i].Item1, cards[i].Item2 + count);
                }
            }
            return cards.Sum(kvp => kvp.Value.Item2);
        }

        public override List<(int, List<int>, List<int>)> ProcessInput(string[] input)
        {
            List<(int, List<int>, List<int>)> cards = new List<(int, List<int>, List<int>)>();
            foreach (var line in input)
            {
                int card = Int32.Parse(line.Substring(5, line.IndexOf(':') - 5));
                var nums = line.Substring(line.IndexOf(':') + 1).Split('|', StringSplitOptions.RemoveEmptyEntries);
                var winNums = nums[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).StringArrayToIntList();
                var myNums = nums[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).StringArrayToIntList();
                cards.Add((card, winNums, myNums));
            }
            return cards;
        }

        /// <summary>
        /// Find card's winning number count and number of occurrences of the card (initialize with 1)
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Dictionary<card, (winning number, # of occurrences)></returns>
        private SortedDictionary<int, (int, int)> CardWinCount(List<(int, List<int>, List<int>)> input)
        {
            SortedDictionary<int, (int, int)> cardWinCount = new SortedDictionary<int, (int, int)>();
            foreach (var card in input)
            {
                var winNums = FindWinNumbers((card.Item2, card.Item3));
                cardWinCount[card.Item1] = (winNums, 1);
            }
            return cardWinCount;
        }

        /// <summary>
        /// Find the count of winning numbers in a card
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private int FindWinNumbers((List<int>, List<int>) input)
        {
            return input.Item1.Where(r => input.Item2.Contains(r)).Count();
        }

        /// <summary>
        /// Calculate points for the winning numbers - start with 1, double onwards next
        /// </summary>
        /// <param name="winNumbers"></param>
        /// <returns></returns>
        private long CalculatePoints(int winNumbers)
        {
            // 2^(n-1)
            return (long)Math.Pow(2, (winNumbers - 1));
        }
    }
}
