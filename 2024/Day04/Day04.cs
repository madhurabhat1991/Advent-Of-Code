using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Text.RegularExpressions;

namespace _2024.Day04
{
    public class Day04 : Day<char[,], long, long>
    {
        public override string DayNumber { get { return "04"; } }

        public override long PartOne(char[,] input)
        {
            // scan for X and check if there is word in any direction
            long sum = 0;
            string searchWord = XMAS;
            for (int r = 0; r < input.GetLength(0); r++)        // row
            {
                for (int c = 0; c < input.GetLength(1); c++)    // col
                {
                    if (input[r, c] == searchWord[0])           // scan for first letter
                    {
                        sum += WordToTop(input, r, c, searchWord) ? 1 : 0;          // to top
                        sum += WordToTopRight(input, r, c, searchWord) ? 1 : 0;     // to top right                        
                        sum += WordToRight(input, r, c, searchWord) ? 1 : 0;        // to right                        
                        sum += WordToBottomRight(input, r, c, searchWord) ? 1 : 0;  // to bottom right
                        sum += WordToBottom(input, r, c, searchWord) ? 1 : 0;       // to bottom                        
                        sum += WordToBottomLeft(input, r, c, searchWord) ? 1 : 0;   // to bottom left                        
                        sum += WordToLeft(input, r, c, searchWord) ? 1 : 0;         // to left                        
                        sum += WordToTopLeft(input, r, c, searchWord) ? 1 : 0;      // to top left
                    }
                }
            }
            return sum;
        }

        public override long PartTwo(char[,] input)
        {
            // scan for A and check if there is X-word
            long sum = 0;
            string searchWord = XMAS[1..];
            int searchHalfLen = searchWord.Length / 2;
            for (int r = searchHalfLen; r < input.GetLength(0) - searchHalfLen; r++)        // row
            {
                for (int c = searchHalfLen; c < input.GetLength(1) - searchHalfLen; c++)    // col
                {
                    if (input[r, c] == searchWord[searchHalfLen])                           // scan for middle letter
                    {
                        // check if word is from top left to bottom right or vice versa  AND if word is from top right to bottom left or vice versa
                        if ((WordToBottomRight(input, r - searchHalfLen, c - searchHalfLen, searchWord) || WordToTopLeft(input, r + searchHalfLen, c + searchHalfLen, searchWord))
                            && (WordToBottomLeft(input, r - searchHalfLen, c + searchHalfLen, searchWord) || WordToTopRight(input, r + searchHalfLen, c - searchHalfLen, searchWord)))
                        {
                            sum++;
                        }
                    }
                }
            }
            return sum;
        }

        public override char[,] ProcessInput(string[] input)
        {
            return input.CreateGrid2D();
        }

        private readonly string XMAS = "XMAS";

        public static bool WordToTop(char[,] input, int r, int c, string searchWord)
        {
            if (input[r,c] == searchWord[0] && r - searchWord.Length + 1 >= 0)
            {
                for (int i = 1, wr = r; i < searchWord.Length; i++, wr--)
                {
                    if (input.GetTopCell(wr, c).Item1 != searchWord[i]) { return false; }
                }
                return true;
            }
            return false;
        }

        private static bool WordToTopRight(char[,] input, int r, int c, string searchWord)
        {
            if (input[r, c] == searchWord[0] && r - searchWord.Length + 1 >= 0 && c + searchWord.Length <= input.GetLength(1))
            {
                for (int i = 1, wr = r, wc = c; i < searchWord.Length; i++, wr--, wc++)
                {
                    if (input.GetTopRightCell(wr, wc).Item1 != searchWord[i]) { return false; }
                }
                return true;
            }
            return false;
        }

        private static bool WordToRight(char[,] input, int r, int c, string searchWord)
        {
            if (input[r, c] == searchWord[0] && c + searchWord.Length <= input.GetLength(1))
            {
                for (int i = 1, wc = c; i < searchWord.Length; i++, wc++)
                {
                    if (input.GetRightCell(r, wc).Item1 != searchWord[i]) { return false; }
                }
                return true;
            }
            return false;
        }

        private static bool WordToBottomRight(char[,] input, int r, int c, string searchWord)
        {
            if (input[r, c] == searchWord[0] && r + searchWord.Length <= input.GetLength(0) && c + searchWord.Length <= input.GetLength(1))
            {
                for (int i = 1, wr = r, wc = c; i < searchWord.Length; i++, wr++, wc++)
                {
                    if (input.GetBottomRightCell(wr, wc).Item1 != searchWord[i]) { return false; }
                }
                return true;
            }
            return false;
        }

        private static bool WordToBottom(char[,] input, int r, int c, string searchWord)
        {
            if (input[r, c] == searchWord[0] && r + searchWord.Length <= input.GetLength(0))
            {
                for (int i = 1, wr = r; i < searchWord.Length; i++, wr++)
                {
                    if (input.GetBottomCell(wr, c).Item1 != searchWord[i]) { return false; }
                }
                return true;
            }
            return false;
        }

        private static bool WordToBottomLeft(char[,] input, int r, int c, string searchWord)
        {
            if (input[r, c] == searchWord[0] && r + searchWord.Length <= input.GetLength(0) && c - searchWord.Length + 1 >= 0)
            {
                for (int i = 1, wr = r, wc = c; i < searchWord.Length; i++, wr++, wc--)
                {
                    if (input.GetBottomLeftCell(wr, wc).Item1 != searchWord[i]) { return false; }
                }
                return true;
            }
            return false;
        }

        private static bool WordToLeft(char[,] input, int r, int c, string searchWord)
        {
            if (input[r, c] == searchWord[0] && c - searchWord.Length + 1 >= 0)
            {
                for (int i = 1, wc = c; i < searchWord.Length; i++, wc--)
                {
                    if (input.GetLeftCell(r, wc).Item1 != searchWord[i]) { return false; }
                }
                return true;
            }
            return false;
        }

        private static bool WordToTopLeft(char[,] input, int r, int c, string searchWord)
        {
            if (input[r, c] == searchWord[0] && r - searchWord.Length + 1 >= 0 && c - searchWord.Length + 1 >= 0)
            {
                for (int i = 1, wr = r, wc = c; i < searchWord.Length; i++, wr--, wc--)
                {
                    if (input.GetTopLeftCell(wr, wc).Item1 != searchWord[i]) { return false; }
                }
                return true;
            }
            return false;
        }
    }
}
