using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers
{
    public static class PacketParser
    {
        /// <summary>
        /// Convert List of non-homogenous objects to a string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String ObjectListToString(List<object> input)
        {
            List<string> strings = new List<string>();
            foreach (var obj in input)
            {
                string s = "";
                switch (obj)
                {
                    case List<object> list:
                        s = ObjectListToString(list);
                        break;
                    default:
                        s = $"{obj}";
                        break;
                }
                strings.Add(s);
            }
            return "[" + String.Join(",", strings) + "]";
        }

        /// <summary>
        /// Compare elements
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns>-1 if first < second, 0 if they are equal, 1 if first > second</second></returns>
        public static int CompareElements(object first, object second)
        {
            switch ((first, second))
            {
                case (int f, int s):
                    return Math.Sign(f - s);
                case (List<object> f, List<object> s):
                    return CompareLists(f, s);
                case (int f, List<object> s):
                    return CompareLists(new List<object>() { f }, s);
                case (List<object> f, int s):
                    return CompareLists(f, new List<object>() { s });
                default:
                    return -1;
            }
        }

        public static int CompareLists(List<object> first, List<object> second)
        {
            int maxI = Math.Min(first.Count, second.Count);
            for (int i = 0; i < maxI; i++)
            {
                object f = first[i];
                object s = second[i];
                int diff = CompareElements(f, s);
                if (diff != 0) { return diff; }
            }
            return Math.Sign(first.Count - second.Count);
        }

        /// <summary>
        /// Conver a string to Queue of char
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Queue<Char> StringToQueue(String input)
        {
            Queue<char> queue = new Queue<char>();
            foreach (var ch in input)
            {
                if (!Char.IsWhiteSpace(ch)) { queue.Enqueue(ch); }
            }
            return queue;
        }

        public static List<object> Parse(string input, char start, char end)
        {
            return ParseList(StringToQueue(input), start, end);
        }

        /// <summary>
        /// Parse first int from Queue
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Int32 ParseInt(Queue<char> input)
        {
            string s = "";
            while (Char.IsDigit(input.Peek()))
            {
                s += input.Dequeue();
            }
            return Int32.Parse(s);
        }

        /// <summary>
        /// Parse next int from queue
        /// </summary>
        /// <param name="input"></param>
        /// <param name="right">True if search to the right, false otherwise</param>
        /// <returns>Int32 or KeyNotFoundException</returns>
        public static Int32 ParseNextInt(Queue<char> input, bool right = true)
        {
            var queue = right ? input : new Queue<char>(input.Reverse());
            string s = "";
            while (queue.Any() && !Char.IsDigit(queue.Peek()))
            {
                queue.Dequeue();
            }
            while (queue.Any() && Char.IsDigit(queue.Peek()))
            {
                var ch = queue.Dequeue();
                s = right ? $"{s}{ch}" : $"{ch}{s}";
            }
            if (String.IsNullOrEmpty(s))
            {
                throw new KeyNotFoundException("No int found");
            }
            return Int32.Parse(s);
        }

        /// <summary>
        /// Add int to next int in string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="add"></param>
        /// <param name="right">True if add to the right, false otherwise</param>
        /// <returns></returns>
        public static string AddToNextInt(string input, int add, bool right = true)
        {
            var queue = right ? StringToQueue(input) : new Queue<char>(StringToQueue(input).Reverse());
            string s = "";
            Queue<char> removed = new Queue<char>();
            string result = "";

            // remove char before getting to first int
            while (queue.Any() && !Char.IsDigit(queue.Peek()))
            {
                removed.Enqueue(queue.Dequeue());
            }
            // remove first int chars
            while (queue.Any() && Char.IsDigit(queue.Peek()))
            {
                var ch = queue.Dequeue();
                s = right ? $"{s}{ch}" : $"{ch}{s}";
            }
            // convert first int
            if (!String.IsNullOrEmpty(s))
            {
                var addTo = Int32.Parse(s);
                var sum = add + addTo;
                s = sum.ToString();
            }
            // add char before getting to first int
            result = right
                ? removed.Any() ? String.Join("", removed) : ""
                : queue.Any() ? String.Join("", new Queue<char>(queue).Reverse()) : "";
            // replace first int with sum
            result += s;
            // add remaining chars
            result += right
                ? queue.Any() ? String.Join("", queue) : ""
                : removed.Any() ? String.Join("", new Queue<char>(removed).Reverse()) : "";
            return result;
        }

        /// <summary>
        /// Parse list from Queue
        /// </summary>
        /// <param name="input"></param>
        /// <param name="start">Start of list</param>
        /// <param name="end">End of list</param>
        /// <returns></returns>
        public static List<object> ParseList(Queue<char> input, char start, char end)
        {
            List<object> rtnList = new List<object>();
            // dequeue start
            if (input != null && input.Any()) { input.Dequeue(); }
            // dequeue until end of list is encountered
            while (!input.Peek().Equals(end))
            {
                // parse first object
                rtnList.Add(ParseElement(input, start, end));
                // remove if there is seperator
                if (input.Peek().Equals(',')) { input.Dequeue(); }
            }
            // dequeue end
            if (input != null && input.Any()) { input.Dequeue(); }
            return rtnList;
        }

        /// <summary>
        /// Parse int or list from Queue
        /// </summary>
        /// <param name="input"></param>
        /// <param name="start">Start of list</param>
        /// <param name="end">End of list</param>
        /// <returns></returns>
        public static object ParseElement(Queue<char> input, char start, char end)
        {
            char next = input.Peek();
            if (Char.IsDigit(next))
            {
                return ParseInt(input);
            }
            else if (next.Equals(start))
            {
                return ParseList(input, start, end);
            }
            return null;
        }

        /// <summary>
        /// Check if input can explode (2021 Day 18)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="explodeLimit"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool CheckExplode(string input, char start, char end, int explodeLimit, out int index)
        {
            index = 0;
            Stack<char> stack = new Stack<char>();
            for (int i = 0; i < input.Length; i++)
            {
                var ch = input[i];
                if (ch.Equals(start))
                {
                    stack.Push(ch);
                    if (stack.Count > explodeLimit)
                    {
                        index = i;
                        return true;
                    }
                }
                else if (ch.Equals(end))
                {
                    stack.Pop();
                }
            }
            return false;
        }

        /// <summary>
        /// Explode the input (2021 Day 18)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string Explode(string input, char start, char end, int index)
        {
            int startIndex = index, endIndex = input.IndexOf(end, index);

            string toExplode = input[startIndex..(endIndex + 1)];
            var explodeNumbers = toExplode
                .Replace(start.ToString(), "")
                .Replace(end.ToString(), "")
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .StringArrayToIntList();

            string left = input[0..startIndex];
            left = AddToNextInt(left, explodeNumbers[0], false);

            string right = input[(endIndex + 1)..];
            right = AddToNextInt(right, explodeNumbers[1]);

            return $"{left}0{right}";
        }

        /// <summary>
        /// Check if input can split (2021 Day 18)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="splitLimit"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool CheckSplit(string input, int splitLimit, out int index, out int length)
        {
            index = 0;
            length = 0;
            Stack<char> stack = new Stack<char>();
            for (int i = 0; i < input.Length; i++)
            {
                var ch = input[i];
                if (!Char.IsDigit(ch))
                {
                    stack.Push(ch);
                }
                else
                {
                    var next = ParseNextInt(StringToQueue(input.Substring(i)));
                    if (next >= splitLimit)
                    {
                        index = i;
                        length = next.ToString().Length;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Split the input (2021 Day 18)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Split(string input, char start, char end, int index, int length)
        {
            int startIndex = index, endIndex = startIndex + length - 1;

            string toSplit = input[startIndex..(endIndex + 1)];
            int splitNumber = Int32.Parse(toSplit);
            int[] splitNumbers = new int[] { (int)Math.Floor((decimal)splitNumber / 2), (int)Math.Ceiling((decimal)splitNumber / 2) };

            string left = input[0..startIndex];
            string right = input[(endIndex + 1)..];

            return $"{left}{start}{splitNumbers[0]},{splitNumbers[1]}{end}{right}";
        }

        /// <summary>
        /// Get the magnitude of the input (2021 Day 18)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="start"></param>
        /// <param name="magnitude"></param>
        /// <returns></returns>
        public static long GetMagnitude(string input, char start, long magnitude = 0)
        {
            if (input[0] != start) { return magnitude; }
            while (true)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    var ch = input[i];
                    if (Char.IsDigit(ch))
                    {
                        var left = ParseNextInt(StringToQueue(input.Substring(i)));
                        if (Char.IsDigit(input[i + left.ToString().Length + 1]))
                        {
                            var right = ParseNextInt(StringToQueue(input.Substring(i + left.ToString().Length + 1)));
                            magnitude = left * 3 + right * 2;
                            input = input[0..(i - 1)] + magnitude.ToString() + input[(i + left.ToString().Length + 1 + right.ToString().Length + 1)..];
                            i = i + left.ToString().Length + 1 + right.ToString().Length + 1;
                            return GetMagnitude(input, start, magnitude);
                        }
                    }
                }
            }
        }
    }
}
