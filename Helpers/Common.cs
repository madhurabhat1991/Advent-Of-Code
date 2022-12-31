using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Helpers
{
    public static class Common
    {
        /// <summary>
        /// Read all lines in the file of given path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static String[] ReadFile(this String path)
        {
            return File.ReadAllLines(path);
        }

        /// <summary>
        /// Deep Copy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T DeepClone<T>(this T source)
        {
            IFormatter formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Extract blocks of input separated by blank line
        /// </summary>
        /// <param name="input"></param>
        /// <returns>(List<List<String>>)</returns>
        public static List<List<String>> Blocks(this String[] input)
        {
            List<List<String>> blocks = new List<List<String>>();
            while (input.Any())
            {
                List<String> block = new List<String>();
                while (input.Any() && input[0].Any())
                {
                    block.Add(input[0]);
                    input = input.Skip(1).ToArray();
                }
                blocks.Add(block);
                input = input.Skip(1).ToArray();
            }
            return blocks;
        }
    }
}
