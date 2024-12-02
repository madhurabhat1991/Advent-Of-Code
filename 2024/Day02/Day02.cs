using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2024.Day02
{
    public class Day02 : Day<List<List<int>>, long, long>
    {
        public override string DayNumber { get { return "02"; } }

        public override long PartOne(List<List<int>> input)
        {
            long sum = 0;
            foreach (var report in input)
            {
                bool safe = true;
                int sign = 0;
                for (var i = 1; i < report.Count; i++)
                {
                    // determine increasing (1) or decreasing (-1)
                    if (i == 1)
                    {
                        sign = Math.Sign(report[i] - report[i - 1]);
                    }
                    // check for consistent sign and difference
                    if (!(Math.Sign(report[i] - report[i - 1]) == sign && Math.Abs(report[i] - report[i - 1]) <= 3))
                    {
                        safe = false;
                        break;
                    }
                }
                sum += safe ? 1 : 0;
            }
            return sum;
        }

        public override long PartTwo(List<List<int>> input)
        {
            // checking direction based on first 2 integers won't determine direction for whole report
            long sum = 0;
            foreach (var report in input)
            {
                var counter = 0;
                sum += Safe(report, counter) ? 1 : 0;
            }
            return sum;
        }

        public override List<List<int>> ProcessInput(string[] input)
        {
            List<List<int>> reports = new List<List<int>>();
            foreach (var line in input)
            {
                reports.Add(line.Split(" ", StringSplitOptions.RemoveEmptyEntries).StringArrayToIntList());
            }
            return reports;
        }

        /// <summary>
        /// Determines if the report is safe
        /// </summary>
        /// <param name="report"></param>
        /// <param name="counter"></param>
        /// <returns></returns>
        private bool Safe(List<int> report, int counter)
        {
            // find out the differences and different signs
            List<int> diffList = new List<int>();
            List<int> signList = new List<int>();
            for (var i = 1; i < report.Count; i++)
            {
                diffList.Add(Math.Abs(report[i] - report[i - 1]));
                signList.Add(Math.Sign(report[i] - report[i - 1]));
            }
            var signGrouped = signList.GroupBy(g => g)
                .Select(s => new { Sign = s.Key, Count = s.Count() })
                .ToList();

            // good levels
            if (diffList.All(x => x >= 1 && x <= 3) && signGrouped.Count() == 1)
            {
                return true;
            }
            // levels where one of the value is > 3 OR where one of the value has different sign
            else if ((diffList.Count > 1 && diffList.Count(x => x > 3) == 1) || (signGrouped.Count > 1 && signGrouped.Min(x => x.Count) == 1))
            {
                int suspectIndex = 0;
                if (diffList.Count > 1 && diffList.Count(x => x > 3) == 1)
                {
                    suspectIndex = diffList.IndexOf(diffList.Where(x => x > 3).First());
                }
                else if (signGrouped.Count > 1 && signGrouped.Min(x => x.Count) == 1)
                {
                    var diffSign = signGrouped.Where(w => w.Count == signGrouped.Min(m => m.Count)).Select(s => s.Sign).First();
                    suspectIndex = signList.IndexOf(diffSign);
                }
                // remove the suspects and check if any of the reports are safe just 1 more time
                var temp1 = report.ToList();
                var temp2 = report.ToList();
                temp1.RemoveAt(suspectIndex + 1);
                temp2.RemoveAt(suspectIndex);
                counter += 1;                
                if (counter == 1 && (Safe(temp1, counter) || Safe(temp2, counter)))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
