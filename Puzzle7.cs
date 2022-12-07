using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022
{
    public class Puzzle7 : IPuzzle<long>
    {
        private IList<string> allLines;
        public Puzzle7()
        {
            this.allLines = File.ReadAllLines(path);
        }
        public static string path => "input/Puzzle7.txt";
        public long FirstResult => 1348005;
        public long SecondResult => 12785886;

        public long Solve()
        {   
           var dir = this.GenrateDirectory();
            return this.calculateTotal(dir);
        }
        private Directory GenrateDirectory()
        { 
            var root = new Directory(@"\");
            var currentDirectory = root;
            foreach(var line in allLines)
            {
                var splitted = line.Split(" ");
                if(splitted[1] == "cd" && splitted[2] != "..")
                {
                    currentDirectory =  currentDirectory.AddDirectory(splitted[2]);
                }
                else if (int.TryParse(splitted[0], System.Globalization.NumberStyles.Integer, null, out int fileSize))
                {
                    currentDirectory.AddFile(fileSize);
                }
                else if (splitted[1] == "cd" && splitted[2] == "..")
                {
                    currentDirectory = currentDirectory.GetParent();
                }
            }
            return root;
        }
        private long calculateTotal(Directory directory, long result = 0)
        {
            if(directory.GetSize() < 100000)
            {
                result += directory.GetSize();
            }

            foreach(var dir in directory.GetSubDirectorys())
            {
                result = this.calculateTotal(dir, result);
            }

            return result;
        }
        public long SolveNext()
        {
            var root =  this.GenrateDirectory();
            var spaceLeft = 70000000 - root.GetSize();
            var spaceToClear = 30000000 - spaceLeft;

            //24933642 too high;
            //12785886
            return root
                .GetSubDirectorysRecursive(new List<Directory>())
                .OrderBy( x=> x.GetSize())
                .First(x => x.GetSize() > spaceToClear)
                .GetSize();
        }
       private class Directory
       {
           public Directory(string name):this(name, null)
           {
           }
           public Directory(string name, Directory parent)
           {
               this.directories = new List<Directory>();
               this.parent = parent;
               this.name = name;
           }
            List<Directory> directories;
            Directory parent;
            long fileSizes;
            public string name;
            public long GetSize()
            {
                return fileSizes + this.directories.Select(x => x.GetSize()).Sum();
            }
            public Directory AddDirectory(string name)
            {
                var dir = new Directory(name, this);
                this.directories.Add(dir);
                return dir;
            }
            public void AddFile(int size)
            {
                this.fileSizes += size;
            }
            public Directory GetParent()
            {
                return this.parent;
            }
            public List<Directory> GetSubDirectorysRecursive(List<Directory> result)
            {
                foreach(var dir in this.directories)
                {
                    result.Add(dir);
                    result = dir.GetSubDirectorysRecursive(result);
                }
                return result;
            }
            public List<Directory> GetSubDirectorys()
            {
                return this.directories;
            }
       }
    }
}