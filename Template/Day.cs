using System;
using System.Collections.Generic;
using System.Text;
using Helpers;
using System.Linq;

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
        /// <param name="example"></param>
        /// <param name="input"></param>
        public void Main(String day, String ex)
        {
            // Read file
            var exPath = $"Day{day}\\example{ex}.txt";
            var inPath = $"Day{day}\\input.txt";
            var example = exPath.ReadFile();
            var input = inPath.ReadFile();

            // Process input
            Example = ProcessInput(example);
            Input = ProcessInput(input);

            // Call and display output
            Console.WriteLine("Part One Example " + ex);
            Console.WriteLine(PartOne(Example));
            Console.WriteLine();

            Console.WriteLine("Part One Input");
            Console.WriteLine(PartOne(Input));
            Console.WriteLine();

            Console.WriteLine("Part Two Example " + ex);
            Console.WriteLine(PartTwo(Example));
            Console.WriteLine();

            Console.WriteLine("Part Two Input");
            Console.WriteLine(PartTwo(Input));
            Console.WriteLine();
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
    }
}
