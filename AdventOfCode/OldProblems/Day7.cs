using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Day7
    {
        public static Regex folderName = new Regex(@"\s");
        public static Regex digits = new Regex(@"\D+");
        public static Folder currentFolder { get; set; }

        public static void RunDay7(byte[] buffer, bool part2 = false)
        {
            Folder root = new Folder("root", null);
            Queue<string> input = new Queue<string>(Week1_Problems.BufferToStrings(buffer, '\n'));

            // Build Folder System
            currentFolder = root;

            while(input.Count > 0)
            {
                string line = input.Dequeue();
                if (line.Contains("$"))
                {
                    if (line.Contains("cd"))
                    {
                        if (line.Contains(".."))
                        {
                            currentFolder = currentFolder.Parent;
                        } 
                        else if (line.Contains("/"))
                        {
                            currentFolder = root;
                        } 
                        else
                        {
                            if(currentFolder.TryGetFolder(folderName.Split(line).Last(), out var childFolder))
                            {
                                currentFolder = childFolder;
                            }
                            else
                            {
                                childFolder = CreateFolder(line);
                                currentFolder.Folders.Add(childFolder);
                                currentFolder = childFolder;
                            }
                        }
                    }

                    if (line.Contains("ls"))
                    {
                        while(input.Count > 0) 
                        {
                            if (input.Peek().Contains("$")) break;

                            line = input.Dequeue();
                            if (String.IsNullOrEmpty(line)) break;

                            if (line.Contains("dir"))
                            {
                                if (!currentFolder.TryGetFolder(folderName.Split(line).Last(), out var childFolder))
                                {
                                    currentFolder.Folders.Add(CreateFolder(line));
                                }
                            }
                            else
                            {
                                File newFile = CreateFile(line);
                                if (!currentFolder.ContainsFile(newFile.Name))
                                {
                                    currentFolder.Files.Add(newFile);
                                }
                            }
                        }

                    }
                }
            }


            Console.WriteLine("The total size of directories smaller than 100k is {0}", root.SumLessThan100k());

            if (part2)
            {
                int spaceNeeded = 30000000 - (70000000 - root.FolderSize);

                Console.WriteLine("The available space on the filesystem is 70,000,000, but currently {0} is in use. " +
                    "\n{1} space needs to be freed for the system update", root.FolderSize, spaceNeeded );

                var allFolders = root.GetAllFoldersInTree();
                var suitableFolders = allFolders.Where(f => f.FolderSize >= spaceNeeded).OrderBy(f => f.FolderSize);

                Console.WriteLine("The smallest folder that can be deleted is folder {0} with a size of {1}",
                    suitableFolders.Select(f => f.Name).First(), suitableFolders.Select(f => f.FolderSize).First());
            }
        }

        public static Folder CreateFolder(string line)
        {
            var newFolderName = folderName.Split(line).Where(s => !string.IsNullOrWhiteSpace(s));
            Folder newFolder = new Folder(newFolderName.Last(), currentFolder);
            
            return newFolder;
        }


        public static File CreateFile(string line)
        {
            string sizeString = digits.Split(line).Where(s => !string.IsNullOrWhiteSpace(s)).First();
            string name = folderName.Split(line).Where(s => !string.IsNullOrWhiteSpace(s)).Last();
            File newFile = new File(name, Int32.Parse(sizeString));
            return newFile;
        }
    }

    public class Folder
    {
        public string Name { get; }
        public List<File> Files { get; }
        public List<Folder> Folders { get; }
        public Folder Parent { get; }
        public int FileSize => Files.Select(f => f.Size).Sum();
        public int FolderSize => FileSize + Folders.Select(f => f.FolderSize).Sum();

        public Folder(string name, Folder parent)
        {
            Name = name;
            Files = new();
            Folders = new();
            Parent = parent;
        }

        public bool TryGetFolder(string name, out Folder childFolder)
        {
            foreach(Folder folderInList in Folders)
            {
                childFolder = folderInList;
                if(folderInList.Name.Equals(name)) return true;
            }
            childFolder = null;
            return false;
        }

        public bool ContainsFile(string name)
        {
            if (Files.Where(f => f.Name.Equals(name)).Any())
            {
                return true;
            }
            return false;
        }

        // Recursively Traverses Folder Tree to find the sum of all folders with sizes < 100k
        public int SumLessThan100k()
        {
            int treeSum = 0;
            if (FolderSize < 100000)
            {
                treeSum = FolderSize;
            }
            
            if (Folders.Any())
            {
                foreach(var folder in Folders)
                {
                    treeSum += folder.SumLessThan100k();
                }
            }
            return treeSum;
        }

        public List<Folder> GetChildFolders()
        {
            List<Folder> childFolders = new();
            foreach (var folder in Folders)
            {
                childFolders.Add(folder);
                childFolders.AddRange(folder.GetChildFolders());
            }
            return childFolders;
        }

        public List<Folder> GetAllFoldersInTree()
        {
            List<Folder> allFolders = GetChildFolders();
            allFolders.Add(this);
            return allFolders;
        }
    }

    public class File
    {
        public string Name { get; set; }
        public int Size { get; set; }

        public File(string name, int size)
        {
            Name = name;
            Size = size;
        }
    }
}
