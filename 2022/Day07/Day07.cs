using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;

namespace _2022.Day07
{
    public class Day07 : Day<List<String>, long, long>
    {
        public override string DayNumber { get { return "07"; } }

        public override long PartOne(List<string> input)
        {
            var directories = Disk(input);

            return directories.Values
                .Where(r => r.Size <= 100000)
                .Sum(r => r.Size);

        }

        public override long PartTwo(List<string> input)
        {
            var directories = Disk(input);

            var unusedNow = TotalSpace - directories["/"].Size;
            var need = UnusedSpace - unusedNow;

            return directories.Values
                .Where(r => need - r.Size < 0)
                .Min(r => r.Size);
        }

        public override List<string> ProcessInput(string[] input)
        {
            return input.ToList();
        }

        private Dictionary<String, Directory> Disk(List<string> input)
        {
            Dictionary<String, Directory> directories = new Dictionary<string, Directory>();

            FileSystem fs = new FileSystem { Root = new Directory("/") };

            var currentDir = fs.Root;
            string path = $"{currentDir.Name}";
            directories[path] = currentDir;

            for (int i = 0; i < input.Count; i++)
            {
                var cmds = input[i].Split(" ");
                if (cmds[0].Equals("$"))
                {
                    if (cmds[1].Equals("cd"))
                    {
                        if (cmds[2].Equals("/"))
                        {
                            currentDir = fs.Root;
                            path = $"{currentDir.Name}";
                        }
                        else if (cmds[2].Equals(".."))
                        {
                            path = path.Substring(0, path.Length - currentDir.Name.Length - 1);
                            currentDir = currentDir.Parent;
                        }
                        else
                        {
                            currentDir = currentDir.Dirs.First(r => r.Name.Equals(cmds[2]));
                            path = $"{path}/{currentDir.Name}";
                        }
                    }
                }
                else if (cmds[0].Equals("dir"))
                {
                    var dir = new Directory(cmds[1], currentDir);
                    currentDir.Dirs.Add(dir);
                    directories[path] = currentDir;
                }
                else
                {
                    var file = new File(cmds[1], Int64.Parse(cmds[0]));
                    currentDir.Files.Add(file);
                    directories[path] = currentDir;
                }
            }

            return directories;
        }

        private const long TotalSpace = 70000000;
        private const long UnusedSpace = 30000000;
    }

    public class FileSystem
    {
        public Directory Root { get; set; }
    }

    public class Directory
    {
        public string Name { get; set; }
        public Directory Parent { get; set; }
        public List<Directory> Dirs { get; set; } = new List<Directory>();
        public List<File> Files { get; set; } = new List<File>();
        public long Size { get { return Files.Sum(r => r.Size) + Dirs.Sum(r => r.Size); } }
        public Directory(string name, Directory parent = null)
        {
            this.Name = name;
            this.Parent = parent;
        }
    }

    public class File
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public File(string name, long size)
        {
            this.Name = name;
            this.Size = size;
        }
    }
}
