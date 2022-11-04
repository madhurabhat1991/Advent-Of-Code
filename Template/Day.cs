using System;
using System.Collections.Generic;
using System.Text;
using Helpers;

namespace Template
{
    public abstract class Day<T, T1>
    {
        /// <summary>
        /// An example of input
        /// </summary>
        public T Example { get; set; }

        /// <summary>
        /// Real input of problem
        /// </summary>
        public T Input { get; set; }

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
            Console.WriteLine("PartOne Example");
            Console.WriteLine(PartOne(Example));
            Console.WriteLine();

            Console.WriteLine("PartOne Input");
            Console.WriteLine(PartOne(Input));
            Console.WriteLine();

            Console.WriteLine("PartTwo Example");
            Console.WriteLine(PartTwo(Example));
            Console.WriteLine();

            Console.WriteLine("PartTwo Input");
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
        public abstract T1 PartTwo(T input);
    }
}
