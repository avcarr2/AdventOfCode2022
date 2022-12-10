using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Week2
    {
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

    }
}
