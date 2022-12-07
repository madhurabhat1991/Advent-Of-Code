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

            FileSystem fs = new FileSystem
            {
                Root = new Directory
                {
                    Name = "/",
                    Dirs = new List<Directory>(),
                    Files = new List<File>()
                }
            };

            var currentDir = fs.Root;
            string path = $"{currentDir.Name}";
            directories[currentDir.Name] = currentDir;

            for (int i = 0; i < input.Count; i++)
            {
                var cmds = input[i].Split(" ");
                if (cmds[0].StartsWith("$"))
                {
                    if (cmds[1].StartsWith("cd"))
                    {
                        if (cmds[2].StartsWith("/"))
                        {
                            currentDir = fs.Root;
                            path = $"{currentDir.Name}";
                        }
                        else if (cmds[2].StartsWith(".."))
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
                else if (cmds[0].StartsWith("dir"))
                {
                    var dir = new Directory
                    {
                        Name = cmds[1],
                        Dirs = new List<Directory>(),
                        Files = new List<File>(),
                        Parent = currentDir
                    };
                    currentDir.Dirs.Add(dir);
                    directories[path] = currentDir;
                }
                else
                {
                    var file = new File
                    {
                        Name = cmds[1],
                        Size = Int64.Parse(cmds[0])
                    };
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
        public List<Directory> Dirs { get; set; }
        public List<File> Files { get; set; }
        public long Size { get { return Files.Sum(r => r.Size) + Dirs.Sum(r => r.Size); } }
    }

    public class File
    {
        public string Name { get; set; }
        public long Size { get; set; }
    }
}
