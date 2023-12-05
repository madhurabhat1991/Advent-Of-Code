using System;
using System.Collections.Generic;
using System.Text;
using Helpers;
using System.Linq;
using System.Diagnostics;

namespace Template
{
    /// <summary>
    /// Abstract Day
    /// </summary>
    /// <typeparam name="T">Input</typeparam>
    /// <typeparam name="T1">Output of PartOne</typeparam>
    /// <typeparam name="T2">Output of PartTwo</typeparam>
    public abstract class Day<T, T1, T2>
    {
        /// <summary>
        /// Day Number
        /// </summary>
        public abstract String DayNumber { get; }

        /// <summary>
        /// An example of input
        /// </summary>
        T Example { get; set; }

        /// <summary>
        /// Real input of problem
        /// </summary>
        T Input { get; set; }

        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="skipPart1">true if the inputs of Part 1 should not be run, false by default</param>
        /// <param name="skipPart2">true if the inputs of Part 2 should not be run, false by default</param>
        /// <param name="skipExample">true if the examples should not be run, false by default</param>
        /// <param name="skipInput">true if the inputs should not be run, false by default</param>
        public void Main(String ex, bool skipPart1 = false, bool skipPart2 = false, bool skipExample = false, bool skipInput = false)
        {
            // Read file
            var exPath = $"Day{DayNumber}\\example{ex}.txt";
            var inPath = $"Day{DayNumber}\\input.txt";
            var example = exPath.ReadFile();
            var input = inPath.ReadFile();

            // Process input
            Example = ProcessInput(example);
            Input = ProcessInput(input);

            // Call and display output, log time taken
            if (!skipPart1)
            {
                if (!skipExample)
                {
                    Console.WriteLine("Part One Example " + ex);
                    LogTime();
                    Console.WriteLine(PartOne(Example));
                    LogTime();
                    Console.WriteLine();
                }

                if (!skipInput)
                {
                    Console.WriteLine("Part One Input");
                    LogTime();
                    Console.WriteLine(PartOne(Input));
                    LogTime();
                    Console.WriteLine();
                }
            }

            if (!skipPart2)
            {
                if (!skipExample)
                {
                    Console.WriteLine("Part Two Example " + ex);
                    LogTime();
                    Console.WriteLine(PartTwo(Example));
                    LogTime();
                    Console.WriteLine();
                }

                if (!skipInput)
                {
                    Console.WriteLine("Part Two Input");
                    LogTime();
                    Console.WriteLine(PartTwo(Input));
                    LogTime();
                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// Process input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract T ProcessInput(String[] input);

        /// <summary>
        /// Solution to Part One of the question
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract T1 PartOne(T input);

        /// <summary>
        /// Solution to Part Two of the question
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract T2 PartTwo(T input);

        private Stopwatch stopwatch { get; set; } = new Stopwatch();
        private void LogTime()
        {
            if (!stopwatch.IsRunning)
            {
                stopwatch.Start();
            }
            else
            {
                stopwatch.Stop();
                Console.WriteLine("\n" + "Time : " + stopwatch.ElapsedMilliseconds + " ms");
                stopwatch.Reset();
            }
        }
    }
}
