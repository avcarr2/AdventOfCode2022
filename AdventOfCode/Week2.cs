using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Text.Encodings.Web;

namespace AdventOfCode
{
    public class Week2
    {
        public static void Day10(byte[] buffer, bool part2)
        {
            string[] input = Helper.BufferToStrings(buffer, '\n');
            //List<string> input = new();
            //var read = new FileStream(@"C:\Users\Alex\Desktop\Day10Example.txt", FileMode.Open, FileAccess.Read);
            //using (var streamreader = new StreamReader(read))
            //{
            //    while (!streamreader.EndOfStream)
            //    {
            //        input.Add(streamreader.ReadLine());
            //    }
            //}

            CommsDevice comm = new CommsDevice();
            foreach (string command in input)
            {
                comm.LoadInstruction(command);
            }

            if (!part2)
            {
                int signalSum = 0;
                int cycle = 20;
                signalSum += comm.RunCycles(20) * cycle;
                for (int i = 0; i < 5; i++)
                {
                    cycle += 40;
                    int signalCurrent = comm.RunCycles(40) * cycle;
                    signalSum += signalCurrent;
                }

                Console.WriteLine("The sum of the signal strengths across relevant cycles was {0}", signalSum);
            }

            List<char[]> screen = new List<char[]>();
            for (int i = 0; i < 6; i++)
            {
                screen.Add(new char[40]);
                for (int j = 0; j < 40; j++)
                {
                    screen[i][j] = comm.CrtCycle() ? '#' : '.';
                }
            }

            foreach (var line in screen)
            {
                Console.WriteLine(new String(line));
            }

        }

        #region oldProblems
        public static void Day9(byte[] buffer, bool part2)
        {
            string[] input = Helper.BufferToStrings(buffer, '\n');

            //input = new[]
            //{
            //    "R 5",
            //    "U 8",
            //    "L 8",
            //    "D 3",
            //    "R 17",
            //    "D 10",
            //    "L 25",
            //    "U 20"
            //};
            Rope bridgeRope = new Rope();
            if (part2)
            {
                bridgeRope = new Rope(9);
            }
            foreach (string line in input)
            {
                if (!String.IsNullOrWhiteSpace(line))
                {
                    bridgeRope.Update(line);
                }
            }

            Console.WriteLine("The tail visited {0} unique positions", bridgeRope.UniqueTailPositions);
        }


        public static int CountBackwards(int[] treeLine, int startIndex)
        {
            int visibleTrees = 0;
            for(int i = startIndex-1; i >=0; i--)
            {
                if(treeLine[i] < treeLine[startIndex])
                {
                    visibleTrees++;
                } else
                {
                    visibleTrees++;
                    break;
                }
            }
            return visibleTrees;
        }

        public static int CountForward(int[] treeLine, int startIndex)
        {
            int visibleTrees = 0;
            for (int i = startIndex + 1; i < treeLine.Length; i++)
            {
                if (treeLine[i] < treeLine[startIndex])
                {
                    visibleTrees++;
                }
                else
                {
                    visibleTrees++;
                    break;
                }
            }
            return visibleTrees;
        }

        public static void Day8(byte[] buffer, bool part2=false)
        {
            char[] input = Helper.BufferToChars(buffer);

            // Find length of each row (this is equivalent to the number of columns)
            int rowLength = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '\n')
                {
                    rowLength = i;
                    break;
                }
            }

            // Construct grid from input
            List< int[] > grid = new List<int[]>();
            grid.Add(new int[rowLength]);
            int colIndex = 0;
            int rowIndex = 0;
            foreach (char c in input)
            {
                if (char.IsNumber(c))
                {
                    grid[rowIndex][colIndex] = int.Parse(c.ToString());
                    colIndex++;
                }
                else if (c == '\n')
                {
                    rowIndex++;
                    grid.Add(new int[rowLength]);
                    colIndex = 0;
                }
            }
            grid.RemoveAt(rowIndex);
            rowIndex--;

            int numRows = grid.Count;
            bool[,] invisibleGrid = new bool[numRows, rowLength];
            int[,] scoreGrid = new int[numRows, rowLength]; 
            
            // ensure input was read in correctly
            //foreach(var row in grid)
            //{
            //    foreach(int number in row)
            //    {
            //        Console.Write(number.ToString());
            //    }
            //    Console.Write('\n');
            //}

            // Check for horizontal invisibility
            int leftMax = 0;
            int rightMax = 0;
            int leftMaxIndex = 0;
            int rightMaxIndex = 0;
            for(int i = 0; i < numRows; i++)
            {
                leftMax = grid[i][0];
                
                for (int k = 0; k<rowLength; k++)
                {
                    if(rightMax < grid[i][k]) rightMax = grid[i][k];
                    rightMaxIndex = k;
                }

                for(int j = 0; j < rowLength; j++)
                {
                    scoreGrid[i,j] = CountForward(grid[i], j) * CountBackwards(grid[i], j); 
                    if (i == 0 | j == 0 | i == numRows - 1 | j == rowLength - 1)
                    {
                        invisibleGrid[i, j] = false;
                    } else
                    {
                        if (
                            (grid[i][j] < leftMax || (grid[i][j] == leftMax & grid[i][j] == grid[i][..j].Max())) &
                            (grid[i][j] < rightMax || (grid[i][j] == rightMax & grid[i][j] == grid[i][(j + 1)..].Max())))
                        {
                            invisibleGrid[i, j] = true;
                        }
                        else
                        {
                            if (grid[i][j] > leftMax)
                            {
                                leftMax = grid[i][j];
                            }
                            if (grid[i][j] >= rightMax)
                            {
                                rightMax = grid[i][(j + 1)..].Max();
                            }
                        }
                    }
                }
            }

            Console.WriteLine("The number of horizontally invisible trees = {0}", invisibleGrid.Cast<bool>().Count(t => t));

            int topMax = 0;
            int bottomMax = 0;
            int[] colVec = new int[numRows];
            for (int j = 0; j < rowLength; j++)
            {
                for (int k =0; k < numRows; k++)
                {
                    colVec[k] = grid[k][j];
                    if (colVec[k] > bottomMax) bottomMax = colVec[k];
                }
                topMax = colVec[0];

                for (int i = 0; i < numRows; i++)
                {
                    scoreGrid[i, j] *= CountForward(colVec, i) * CountBackwards(colVec, i);
                    if (i == 0 | j == 0 | i == numRows - 1 | j == rowLength - 1)
                    {
                        invisibleGrid[i, j] = false;
                    }
                    else
                    {
                        if (
                            (colVec[i] < topMax || (colVec[i] == topMax & colVec[i] == colVec[..i].Max())) &
                            (colVec[i] < bottomMax || (colVec[i] == bottomMax & colVec[i] == colVec[(i + 1)..].Max()))
                            )
                        {
                        int treeHeight = colVec[i];
                        int placeholder = 0;
                        }
                        else
                        {
                            invisibleGrid[i, j] = false;
                            if (colVec[i] > topMax)
                            {
                                topMax = colVec[i];
                            }
                            if (colVec[i] >= bottomMax)
                            {
                                bottomMax = colVec[(i + 1)..].Max();
                            }
                        }
                    }
                }
            }

            Console.WriteLine("The number of visible trees = {0}", invisibleGrid.Cast<bool>().Count(t => !t));
            Console.WriteLine("The most scenic view has a tree score of {0}", scoreGrid.Cast<int>().Max());
        }

        #endregion

    }

    public class CommsDevice
    {
        public static Regex whiteSpaceSplit = new Regex(@"\s+");
        public int Cycle;
        private int _register;
        public Queue<int> Todo;

        public CommsDevice()
        {
            Cycle = 0;
            _register = 1;
            Todo = new();
        }

        public void LoadInstruction(string command)
        {
            if (command.Contains("noop"))
            {
                Todo.Enqueue(0);
            } else if (command.Contains("addx"))
            {
                Todo.Enqueue(0);
                Todo.Enqueue(Int32.Parse(whiteSpaceSplit.Split(command)[1]));
            }
        }

        public int RunCycles(int numberOfCycles)
        {
            int penultimateRegVal = 0;
            for (int i = 0; i < numberOfCycles; i++)
            {
                Cycle++;
                if (i == numberOfCycles - 1)
                {
                    penultimateRegVal = _register;
                }
                _register += Todo.Dequeue();

            }
            return penultimateRegVal;
        }

        public bool CrtCycle()
        {
            
            int xPosition = Cycle % 40;
            int currentRegister = _register;
            _register += Todo.Dequeue();
            Cycle++;

            if (Math.Abs(xPosition - currentRegister) < 2)
            {
                return true;
            }
            return false;
        }

    }

    public class Rope
    {

        public static Regex digits = new Regex(@"\D+");
        public static Regex directions = new Regex(@"\w{1}");
        public static Regex whiteSpaceSplit = new Regex(@"\s+");
        public (int x, int y) HeadPosition;
        public (int x, int y) TailPosition;
        public List<(int x, int y)> TailPositions;
        public int NumberOfKnots;
        public Dictionary<(int x, int y), int> TailDict;
        public int UniqueTailPositions => TailDict.Count;

        public Rope()
        {
            HeadPosition = (100, 100);
            TailPosition = (100, 100);
            NumberOfKnots = 1;
            TailDict = new Dictionary<(int x, int y), int>();
        }

        public Rope(int knotLength)
        {
            HeadPosition = (100, 100);
            TailPositions = new();
            for (int i = 0; i < knotLength; i++)
            {
                TailPositions.Add((100, 100));
                if (i == knotLength - 1)
                {
                    TailPosition = TailPositions[i];
                }
            }

            NumberOfKnots = knotLength;
            TailDict = new Dictionary<(int x, int y), int>();
        }

        public void Update(string line)
        {
            string[] instruction = whiteSpaceSplit.Split(line);
            if (instruction[0].Equals("R"))
            {
                for (int i = 0; i < Int32.Parse(instruction[1]); i++)
                {
                    HeadPosition.x++;
                    UpdateTail();
                }
                
            }
            else if (instruction[0].Equals("L"))
            {
                for (int i = 0; i < Int32.Parse(instruction[1]); i++)
                {
                    HeadPosition.x--;
                    UpdateTail();
                }
            }
            else if (instruction[0].Equals("U"))
            {
                for (int i = 0; i < Int32.Parse(instruction[1]); i++)
                {
                    HeadPosition.y++;
                    UpdateTail();
                }
            }
            else if (instruction[0].Equals("D"))
            {
                for (int i = 0; i < Int32.Parse(instruction[1]); i++)
                {
                    HeadPosition.y--;
                    UpdateTail();
                }
            }
        }

        private void UpdateTail()
        {
            if (NumberOfKnots > 1)
            {
                UpdateAllTails();
                return;
            }
            int xDelta = HeadPosition.x - TailPosition.x;
            int yDelta = HeadPosition.y - TailPosition.y;
            switch((xDelta, yDelta))
            {
                // Linear movement
                case (2, 0):
                    TailPosition.x++;
                    break;
                case (-2, 0):
                    TailPosition.x--;
                    break;
                case (0, 2):
                    TailPosition.y++;
                    break;
                case (0, -2):
                    TailPosition.y--;
                    break;
                // Diagonal, horizontal lead
                case (-2, 1):
                    TailPosition.y++;
                    TailPosition.x--;
                    break;
                case (2, 1):
                    TailPosition.y++;
                    TailPosition.x++;
                    break;
                case (2, -1):
                    TailPosition.y--;
                    TailPosition.x++;
                    break;
                case (-2, -1):
                    TailPosition.y--;
                    TailPosition.x--;
                    break;
                // Diagonal, vertical lead
                case (1, -2):
                    TailPosition.y--;
                    TailPosition.x++;
                    break;
                case  (1, 2):
                    TailPosition.y++;
                    TailPosition.x++;
                    break;
                case (-1, 2):
                    TailPosition.y++;
                    TailPosition.x--;
                    break;
                case (-1, -2):
                    TailPosition.y--;
                    TailPosition.x--;
                    break;

                default:
                    break;

            }

            TailDict.TryAdd(TailPosition, 1);
        }

        private (int x, int y) UpdateTailSpecific((int x, int y) head, (int x, int y) tail)
        {
            int xDelta = head.x - tail.x;
            int yDelta = head.y - tail.y;
            switch ((xDelta, yDelta))
            {
                // Linear movement
                case (2, 0):
                    tail.x++;
                    break;
                case (-2, 0):
                    tail.x--;
                    break;
                case (0, 2):
                    tail.y++;
                    break;
                case (0, -2):
                    tail.y--;
                    break;
                // Diagonal, horizontal lead
                case (-2, 1):
                    tail.y++;
                    tail.x--;
                    break;
                case (2, 1):
                    tail.y++;
                    tail.x++;
                    break;
                case (2, -1):
                    tail.y--;
                    tail.x++;
                    break;
                case (-2, -1):
                    tail.y--;
                    tail.x--;
                    break;
                // Diagonal, vertical lead
                case (1, -2):
                    tail.y--;
                    tail.x++;
                    break;
                case (1, 2):
                    tail.y++;
                    tail.x++;
                    break;
                case (-1, 2):
                    tail.y++;
                    tail.x--;
                    break;
                case (-1, -2):
                    tail.y--;
                    tail.x--;
                    break;
                case (2, 2):
                    tail.y++;
                    tail.x++;
                    break;
                case (2, -2):
                    tail.y--;
                    tail.x++;
                    break;
                case (-2, 2):
                    tail.y++;
                    tail.x--;
                    break;
                case (-2, -2):
                    tail.y--;
                    tail.x--;
                    break;
                default:
                    break;
                // Linear double

            }

            return tail;

        }



        private void UpdateAllTails()
        {
            TailPositions[0] = UpdateTailSpecific(HeadPosition, TailPositions[0]);
            for (int i = 1; i < NumberOfKnots; i++)
            {
                TailPositions[i] = UpdateTailSpecific(TailPositions[i-1], TailPositions[i]);
            }
            TailDict.TryAdd(TailPositions[NumberOfKnots-1], 1);
        }
    }
}
