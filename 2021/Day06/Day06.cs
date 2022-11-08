using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2021.Day06
{
    public class Day06 : Day<List<int>, long, long>
    {
        public override string DayNumber { get { return "06"; } }

        public override long PartOne(List<int> input)
        {
            return FishLife(input, DaysOne);
        }

        public override long PartTwo(List<int> input)
        {
            return FishLife(input, DaysTwo);
        }

        public override List<int> ProcessInput(string[] input)
        {
            return input[0].Split(",", StringSplitOptions.RemoveEmptyEntries).StringArrayToIntList();
        }

        private int DaysOne = 80;
        private int DaysTwo = 256;

        private int LifeSpan = 7;
        private int TimerLow = 0;

        private long FishLife(List<int> input, int days)
        {
            Dictionary<int, long> fishes = new Dictionary<int, long>();    // <Timer, no. of fishes having that timer>
            input.ForEach(timer => fishes.IncrementValue(timer, 1));

            for (int day = 0; day < days; day++)
            {
                Dictionary<int, long> newFishes = new Dictionary<int, long>();
                foreach (var kvp in fishes)
                {
                    var currentTimer = kvp.Key;
                    var currentCount = kvp.Value;

                    var newTimer = currentTimer == TimerLow ? LifeSpan - 1 : currentTimer - 1;
                    var newCount = currentCount;
                    newFishes.IncrementValue(newTimer, newCount);

                    var newBornTimer = currentTimer == TimerLow ? LifeSpan + 1 : 0;
                    var newBornCount = currentCount;
                    if (newBornTimer > 0)
                    {
                        newFishes.IncrementValue(newBornTimer, newBornCount);
                    }
                }
                fishes = newFishes;
            }

            return fishes.Values.Sum();
        }
    }
}
