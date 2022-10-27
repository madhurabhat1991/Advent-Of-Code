using System;
using System.Collections.Generic;
using System.Text;

namespace Template
{
    public interface IDay<T, T1>
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
        void Main(String[] example, String[] input);

        /// <summary>
        /// Process input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        T ProcessInput(String[] input);

        /// <summary>
        /// Solution to Part One of the question
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        T1 PartOne(T input);

        /// <summary>
        /// Solution to Part Two of the question
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        T1 PartTwo(T input);
    }
}
