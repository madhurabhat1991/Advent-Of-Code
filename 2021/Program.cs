using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace _2021
{
    class Program
    {
        public static void Main(string[] args)
        {
            var ex = "";

            var challenge = new Day01.Day01();
            var day = "01";


            var example = File.ReadAllLines($"Day{day}\\example{ex}.txt");
            var input = File.ReadAllLines($"Day{day}\\input.txt");
            challenge.Main(example, input);
        }
    }
}
