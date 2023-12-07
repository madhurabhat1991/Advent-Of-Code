using System;
using System.Collections.Generic;
using System.Text;
using Template;
using System.Linq;

namespace _2023.Day07
{
    public class Day07 : Day<List<Hand>, long, long>
    {
        public override string DayNumber { get { return "07"; } }

        public override long PartOne(List<Hand> input)
        {
            return CalculateRanks(input);
        }

        public override long PartTwo(List<Hand> input)
        {
            input.ForEach(r => r.UseJoker());
            return CalculateRanks(input);
        }

        public override List<Hand> ProcessInput(string[] input)
        {
            List<Hand> hands = new List<Hand>();
            foreach (var line in input)
            {
                var info = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                hands.Add(new Hand(info[0], Int32.Parse(info[1])));
            }
            return hands;
        }

        private long CalculateRanks(List<Hand> input)
        {
            long sum = 0;
            int rank = 1;
            var hands = input.ToList();
            hands.Sort();       // uses default comparer of Hand
            hands.ForEach(r =>
            {
                r.Rank = rank;
                sum += r.Bid * r.Rank;
                rank++;
            });
            return sum;
        }
    }

    public class Hand : IComparable<Hand>
    {
        public List<char> Cards { get; set; }
        public int Bid { get; set; }
        public bool UsingJoker { get; set; }
        public int Type
        {
            get
            {
                var grouped = Cards.GroupBy(r => r).Select(r => new { Card = r.Key, Count = r.Count() }).ToList();
                if (UsingJoker)
                {
                    if (grouped.Any(r => r.Card == 'J'))
                    {
                        var joker = grouped.Where(r => r.Card == 'J').First();              // joker in hand
                        var others = grouped.Where(r => r.Card != 'J').ToList();            // other cards in hand
                        var highScores = others.Any() ? others.Max(r => r.Count) : 0;       // max count of other cards
                        var matches = others.Where(r => r.Count == highScores).ToList();    // best cards in hand except joker
                        var best = matches.Any() ? matches.First() : joker;                 // first if many matches, o/w joker
                        grouped = Cards.GroupBy(r => r)
                            .Select(r => new
                            {
                                Card = r.Key,
                                Count = best.Card != 'J' && best.Card == r.Key ? r.Count() + joker.Count    // add joker to the best card if joker is not best
                                    : best.Card != 'J' && r.Key == 'J' ? 0                                  // remove joker if it is not the best card
                                    : r.Count()                                                             // no changes for other cards or if joker is best
                            }).ToList();
                    }
                }
                if (grouped.Any(r => r.Count == 5))
                {
                    return (int)TypeRank.FiveOfAKind;
                }
                else if (grouped.Any(r => r.Count == 4))
                {
                    return (int)TypeRank.FourOfAKind;
                }
                else if (grouped.Any(r => r.Count == 3) && grouped.Any(r => r.Count == 2))
                {
                    return (int)TypeRank.FullHouse;
                }
                else if (grouped.Any(r => r.Count == 3))
                {
                    return (int)TypeRank.ThreeOfAKind;
                }
                else if (grouped.Where(r => r.Count == 2).Count() == 2)
                {
                    return (int)TypeRank.TwoPair;
                }
                else if (grouped.Where(r => r.Count == 2).Count() == 1)
                {
                    return (int)TypeRank.OnePair;
                }
                else if (grouped.Count() == 5)
                {
                    return (int)TypeRank.HighCard;
                }
                return 99;
            }
        }
        public int Rank { get; set; }

        public Hand(string cards, int bid)
        {
            this.Cards = new List<char>(cards.ToCharArray());
            this.Bid = bid;
        }

        public void UseJoker()
        {
            UsingJoker = true;
            CardRank['J'] = CardRank['2'] - 1;
        }

        // default comparer for Hand type
        public int CompareTo(Hand compareHand)
        {
            if (this == compareHand) { return 0; }
            // compare types
            if (this.Type == compareHand.Type)
            {
                // compare cards
                int index = 0;
                while (true)
                {
                    if (CardRank[this.Cards[index]] == CardRank[compareHand.Cards[index]])
                    {
                        index++;
                    }
                    else
                    {
                        return CardRank[this.Cards[index]] - CardRank[compareHand.Cards[index]];
                    }
                }
            }
            else
            {
                return this.Type - compareHand.Type;
            }
        }

        private enum TypeRank
        {
            HighCard, OnePair, TwoPair, ThreeOfAKind, FullHouse, FourOfAKind, FiveOfAKind
        }

        private Dictionary<char, int> CardRank = new Dictionary<char, int>()
        {
            { '2', 1 }, { '3', 2 }, { '4', 3 }, { '5', 4 }, { '6', 5 }, { '7', 6 }, { '8', 7 }, { '9', 8 }, { 'T', 9 }, { 'J', 10 }, { 'Q', 11 }, { 'K', 12 }, { 'A', 13 }
        };
    }
}
