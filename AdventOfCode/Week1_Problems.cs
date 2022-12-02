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



    }
}
