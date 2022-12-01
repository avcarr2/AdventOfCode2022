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
    }
}
