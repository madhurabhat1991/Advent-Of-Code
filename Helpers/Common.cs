using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;

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
            using var stream = new MemoryStream();
            Serializer.Serialize(stream, source);
            stream.Seek(0, SeekOrigin.Begin);
            return Serializer.Deserialize<T>(stream);

            #region deprecated
            // DataContractSerializer - does not support multi-dimensional arrays
            //var formatter = new DataContractSerializer(typeof(T));
            //using (var stream = new MemoryStream())
            //{
            //    formatter.WriteObject(stream, source);
            //    stream.Seek(0, SeekOrigin.Begin);
            //    return (T)formatter.ReadObject(stream);
            //}

            // XmlSerializer - correct output but slow 14037 ms vs 71 ms
            //var serailizer = new XmlSerializer(typeof(T));
            //String sb = "path.xml";
            //using (StreamWriter writer = new StreamWriter(sb))
            //{
            //    serailizer.Serialize(writer, source);
            //}
            //using (StreamReader reader = new StreamReader(sb))
            //{
            //    return (T)serailizer.Deserialize(reader);
            //}

            // JsonSerializer - produces wrong output
            //var resultBytes = JsonSerializer.Serialize(source);
            //var data = JsonSerializer.Deserialize<T>(resultBytes);
            //return data;

            // BinaryFormatter - obsolete in .NET 8
            //IFormatter formatter = new BinaryFormatter();
            //using (var stream = new MemoryStream())
            //{
            //    formatter.Serialize(stream, source);
            //    stream.Seek(0, SeekOrigin.Begin);
            //    return (T)formatter.Deserialize(stream);
            //}
            #endregion
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
