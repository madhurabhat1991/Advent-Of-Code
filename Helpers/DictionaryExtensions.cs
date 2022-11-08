using System;
using System.Collections.Generic;
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
    }
}
