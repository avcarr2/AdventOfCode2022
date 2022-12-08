using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;

namespace AdventOfCode;

public class Node
{
    public List<Node> Children { get; set; }
    public Node Parent { get; set; }
    public List<Day7File> FileList { get; set; }
    public string Name { get; set; }
    private int _cachedSize = 0;
    private int _cachedItemsCount = 0; 
    public int Size
    {
        get
        {
            if (_cachedItemsCount == Children.Count + FileList.Count)
                return _cachedSize;
            _cachedSize = GetSize();
            _cachedItemsCount = FileList.Count + Children.Count;
            return _cachedSize; 
        }
    } 
    public bool IsTerminal => Children.Count == 0;

    public int GetSize()
    {
        return FileList.Sum(x => x.Size) + Children.Sum(x => x.Size); 
    }
    public Node(string name, Node parentNode)
    {
        Name = name;
        Children = new List<Node>();
        FileList = new List<Day7File>();
        Parent = parentNode;
    }

    public Node(string name)
    {
        Name = name;
        Children = new List<Node>();
        FileList = new List<Day7File>();
    }

    public void AddDay7File(string name, int size)
    {
        FileList.Add(new Day7File(name, size, Parent));
    }

    public void AddChildNode(Node childNode)
    {
        Children.Add(childNode);
    }

    public Node GetChildNode(string name)
    {
        return Children.Find(i => i.Name == name)!; 
    }

    public IEnumerable<Node> Traverse(Node node, Func<Node, IEnumerable<Node>> children)
    {
        var seen = new HashSet<Node>();
        var stack = new Stack<Node>(); 
        seen.Add(node);
        stack.Push(node);
        // perform function on original item
        yield return node; 
        while (stack.Count > 0)
        {
            Node current = stack.Pop();
            foreach (Node newItem in children(current))
            {
                if (!seen.Contains(newItem))
                {
                    seen.Add(newItem);
                    stack.Push(newItem);
                    // perform function on new item 
                    yield return newItem; 
                }
            }
        }
    }
    
}

public class Day7File
{
    public string Name { get; set; }
    public int Size { get; set; }
    public Node ParentNode { get; set; }
    public Day7File(string name, int size, Node parentNode)
    {
        Name = name;
        Size = size;
        ParentNode = parentNode;
    }
}

public class Day7Parsers
{
    public Node ParseLine(string[] line, Node currentNode, int lineNumber)
    {
        if (line[lineNumber] == " " && lineNumber < line.Length - 1) return currentNode; 

        if (line[lineNumber] == "$ cd ..")
        {
            lineNumber++; 
            return ParseLine(line, currentNode.Parent, lineNumber); 
        }

        if (line[lineNumber].Contains("$ ls"))
        {
            lineNumber++;
            while (!line[lineNumber].Contains("$") && lineNumber < line.Length - 1)
            {

                if (line[lineNumber].Contains("dir"))
                {
                    string[] tempData = line[lineNumber].Split(" "); 
                    currentNode.AddChildNode(new Node(tempData[1], currentNode));
                    lineNumber++; 
                }
                else
                {
                    string[] tempData = line[lineNumber].Split(" ");
                    currentNode.AddDay7File(tempData[1], int.Parse(tempData[0]));
                    lineNumber++; 
                }
            }
            return ParseLine(line, currentNode, lineNumber); 
        }

        var tempString = line[lineNumber].Split(" ");
        lineNumber++;
        if (lineNumber > line.Length - 1) return currentNode; 
        return ParseLine(line, currentNode.GetChildNode(tempString[2]), lineNumber);
    }
}

public static class NodeExtensions
{
}

