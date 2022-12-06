using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;

namespace _2022.Day06
{
    public class Day06 : Day<String, long, long>
    {
        public override string DayNumber { get { return "06"; } }

        public override long PartOne(String input)
        {
            return DetectMarker(input, PacketMarker);
        }

        public override long PartTwo(String input)
        {
            return DetectMarker(input, MessageMarker);
        }

        public override String ProcessInput(string[] input)
        {
            return input[0];
        }

        private const int PacketMarker = 4;
        private const int MessageMarker = 14;

        private long DetectMarker(string input, int len)
        {
            int marker = 0;
            string message = "";
            for (int i = 0; i < input.Length; i++)
            {
                message += input[i];
                if (i >= len - 1)
                {
                    if (i != len - 1)
                    {
                        message = message.Substring(1);
                    }
                    if (message.ToList().All(x => message.ToList().Count(y => y == x) == 1))
                    {
                        marker = i + 1;
                        break;
                    }
                }
            }
            return marker;
        }
    }
}
