using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Week1_Problems
    {
        public static void Day1Problem1(byte[] buffer)
        {
            string[] stringArray = Encoding.UTF8.GetString(buffer).Split( "\n\n" );
            List<List<int>> elfBindles = new();
            int[] topThreeCalories = new int[3];
            foreach (string str in stringArray)
            {
                if (String.IsNullOrEmpty(str)) continue;
                List<int> elfBindle = str.Split('\n').Where(s => !String.IsNullOrEmpty(s)).
                    Select( s => Int32.Parse(s)).ToList();
                int bindleSum = elfBindle.Sum();
                ThreeSort(topThreeCalories, bindleSum);
                if (!(elfBindle == null) || elfBindle.Count > 0) elfBindles.Add(elfBindle);
            }
            Console.WriteLine("Max Calories = " + topThreeCalories[0].ToString());
            Console.WriteLine("Top three calorie sum = " + topThreeCalories.Sum().ToString());
        }

        internal static Dictionary<string, int> RpsDictionary = new Dictionary<string, int>
        {
            { "A X", 3+1 },
            { "A Y", 6+2 },
            { "A Z", 0+3},
            { "B X", 0+1 },
            { "B Y", 3+2 },
            { "B Z", 6+3},
            { "C X", 6+1 },
            { "C Y", 0+2 },
            { "C Z", 3+3}
        };

        internal static Dictionary<string, int> RpsDictionaryPart2 = new Dictionary<string, int>
        {
            { "A X", 0+3 },
            { "A Y", 3+1 },
            { "A Z", 6+2},
            { "B X", 0+1 },
            { "B Y", 3+2 },
            { "B Z", 6+3},
            { "C X", 0+2 },
            { "C Y", 3+3 },
            { "C Z", 6+1}
        };

        private static void ThreeSort(int[] topThree, int current)
        {
            if (current > topThree[2])
            {
                topThree[2] = topThree[1];
                if (current > topThree[1])
                {
                    topThree[1] = topThree[0];
                    if (current > topThree[0])
                    {
                        topThree[0] = current;
                    }
                    else
                    {
                        topThree[1] = current;
                    }
                }
                else
                {
                    topThree[2] = current;
                }
            }
            else
            {
                // Top three stays the same
            }
        }

        public static void Day2(byte[] buffer)
        {
            //Console.WriteLine(Encoding.UTF8.GetString(buffer));

            string[] stringArray = Encoding.UTF8.GetString(buffer).Split('\n');
            int scoreTotal = 0;
            int part2ScoreTotal = 0;
            foreach (string str in stringArray)
            {
                if (RpsDictionary.TryGetValue(str, out int score))
                {
                    scoreTotal += score;

                }
                else
                {
                    Console.WriteLine("oops, something went wrong");
                }

                if (RpsDictionaryPart2.TryGetValue(str, out int score2))
                {
                    part2ScoreTotal += score2;

                }
                else
                {
                    Console.WriteLine("oops, something went wrong");
                }
            }

            Console.WriteLine("Total RPS score  = " + scoreTotal.ToString());
            Console.WriteLine("Total Corrected RPS score  = " + part2ScoreTotal.ToString());
        }

        public static void Day3(byte[] buffer)
        {
            string[] stringArray = Encoding.UTF8.GetString(buffer).Split('\n');

            Dictionary<char, int> firstCompartment = new();
            int prioritySum = 0;
            foreach(string sack in stringArray)
            {
                firstCompartment.Clear();
                char[] itemArray = sack.ToCharArray();
                // iterate through the first compartment, storing each item + the number of occurences
                for (int i = 0; i < itemArray.Length/2; i++)
                {
                    if(!firstCompartment.TryAdd(itemArray[i], 1))
                    {
                        firstCompartment[itemArray[i]]++;
                    }
                }
                for (int j = itemArray.Length/2; j < itemArray.Length; j++)
                {
                    if (firstCompartment.TryGetValue(itemArray[j], out int itemCount) &&
                        itemCount > 0)
                    {
                        prioritySum += ConvertCharToPriority(itemArray[j]);
                        break;
                    }
                }
            }
            Console.WriteLine("The sum of the priority values for the misassigned objects = " + prioritySum.ToString());
        }

        public static void Day3Part2(byte[] buffer)
        {
            string[] stringArray = Encoding.UTF8.GetString(buffer).Split('\n').Where(s => !String.IsNullOrEmpty(s)).ToArray();

            int badgeSum = 0;
            if (stringArray.Length % 3 > 0) throw new InvalidDataException("The input isn't grouped by threes");

            List<char> elfSack = new();
            Dictionary<char, int> groupSacks = new();
            for (int i = 0; i < stringArray.Length/3; i++)
            {
                int[] groupIndices = new int[]
                {
                    0 + i*3,
                    1 + i*3,
                    2 + i*3
                };
                groupSacks.Clear();

                // store contents of the first elf's sack in groupSacks dictionary
                elfSack.Clear();
                elfSack.AddRange(stringArray[groupIndices[0]]);
                foreach (char item in elfSack)
                {
                    groupSacks.TryAdd(item, 1);
                }

                // find overlap between first and second elf
                elfSack.Clear();
                elfSack.AddRange(stringArray[groupIndices[1]]);
                foreach (char item in elfSack)
                {
                    if (groupSacks.ContainsKey(item))
                    {
                        groupSacks[item]++;
                    }
                }

                // find overlap between all three
                elfSack.Clear();
                elfSack.AddRange(stringArray[groupIndices[2]]);
                foreach (char item in elfSack)
                {
                    if (groupSacks.ContainsKey(item) && groupSacks[item] >= 2)
                    {
                        badgeSum += ConvertCharToPriority(item);
                        break;
                    }
                }
            }

            Console.WriteLine("The sum of the badge numbers for each group is " + badgeSum.ToString());
        }

        private static int ConvertCharToPriority(char c)
        {
            int charASCII = (int)c;
            // convert all lowercase characters to their respective priority values
            if (charASCII >= 97 & charASCII <= 122)
            {
                return charASCII - 96;
            } 
            // convert upercase characters
            else if (charASCII >= 65 & charASCII <= 90)
            {
                return charASCII - 38;
            }
            return 0;
        }

    }
}
