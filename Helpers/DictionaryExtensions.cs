using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Add the value to a key, creates key if unavailable
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddValue<T1, T2>(this Dictionary<T1, T2> input, T1 key, T2 value)
        {
            input[key] = input.ContainsKey(key) ? value : value;
        }

        /// <summary>
        /// Increment the value of key by given value, creates key if unavailable
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void IncrementValue<T1>(this Dictionary<T1, long> input, T1 key, long value)
        {
            input[key] = input.ContainsKey(key) ? input[key] + value : value;
        }

        /// <summary>
        /// Increment all values of the dictionary by given value
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="input"></param>
        /// <param name="value"></param>
        public static void IncrementValue<T1>(this Dictionary<T1, long> input, long value)
        {
            foreach (var k in input.Keys.ToList()) { input[k] += 2; }
        }

        /// <summary>
        /// Divides the value of key by given value, no action taken if key is unavailable
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void DivideValue<T1>(this Dictionary<T1, long> input, T1 key, long value)
        {
            if (input.ContainsKey(key)) { input[key] = input[key] / value; }
        }

        /// <summary>
        /// Divide all values of the dictionary by given value
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="input"></param>
        /// <param name="value"></param>
        public static void DivideValue<T1>(this Dictionary<T1, long> input, long value)
        {
            foreach (var k in input.Keys.ToList()) { input[k] /= value; }
        }

        /// <summary>
        /// Find the maximum of the keys
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T1 MaxKey<T1, T2>(this Dictionary<T1, T2> input)
        {
            return input.Keys.Max();
        }

        /// <summary>
        /// Find the minimum of the keys
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T1 MinKey<T1, T2>(this Dictionary<T1, T2> input)
        {
            return input.Keys.Min();
        }

        /// <summary>
        /// Find the maximum of the values
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T2 MaxValue<T1, T2>(this Dictionary<T1, T2> input)
        {
            return input.Values.Max();
        }

        /// <summary>
        /// Find the minimum of the values
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T2 MinValue<T1, T2>(this Dictionary<T1, T2> input)
        {
            return input.Values.Min();
        }
    }
}
