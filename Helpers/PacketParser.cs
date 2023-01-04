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
    }
}
