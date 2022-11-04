using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Helpers
{
    public static class FileExtensions
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
    }
}
