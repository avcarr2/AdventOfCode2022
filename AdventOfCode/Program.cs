using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace AdventOfCode
{
    internal class Program
    {
        // cookie value comes from the advent of code website when you are logged in. 
        private const string _cookieFile = ".cookie"; 
        private static Uri _uri = new Uri("https://adventofcode.com");

        static void Main(string[] args)
        {
            string cookie = ReadCookie();
            byte[] buffer = GetInput(4, 2022, cookie).Result;
            string data = Encoding.UTF8.GetString(buffer);
            Console.WriteLine(Main_Day4Part2(data));
        }

        static int Main_Day4Part2(string data)
        {
            int sum = 0;
            string[] dataSplit = data.Split("\n");
            // find any and all overlapping pairs. 
            for (int i = 0; i < dataSplit.Length; i++)
            {
                if (dataSplit[i] == "") continue;
                var tempData = dataSplit[i].Split(new char[] { '-', ',' });
                int[] tempDataInt = tempData.Select(i => int.Parse(i)).ToArray();

                // take last number and subtract second number in first pair
                // if the number is negative, it overlaps, 
                // if the number is positive, then subtract the first number 
                // in the second pair from the second number in the first pair
                // If that's still positive, they don't overlap 
                int temp = tempDataInt[3] - tempDataInt[1];
                if (temp <= 0)
                {
                    if (tempDataInt[3] - tempDataInt[0] < 0)
                    {
                        continue;
                    }
                    else
                    {
                        sum++;
                    }
                }
                else
                {
                    temp = tempDataInt[2] - tempDataInt[1];
                    if (temp <= 0)
                    {
                        sum++;
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            return sum;
        }


        static int Main_Day4Part1(string data)
        {
            int sum = 0;

            string[] dataSplit = data.Split("\n");

            for (int i = 0; i < dataSplit.Length; i++)
            {
                if (dataSplit[i] == "") continue; 
                var tempData = dataSplit[i].Split(new char[] {'-', ','});
                // temp data should be length four, so iterate over that. 
                // ? Get the max and the min vals, then compare? 
                int[] tempDataInt = tempData.Select(i => int.Parse(i)).ToArray();
                // if 0 is greater than 2 and 4 is greater than 3, it's fully contained. 
                
                if ((tempDataInt[2] <= tempDataInt[0] && tempDataInt[3] >= tempDataInt[1]) ||
                    (tempDataInt[0] <= tempDataInt[2] && tempDataInt[1] >= tempDataInt[3]))
                {
                    sum++; 
                }
            }

            return sum; 
        }

        #region Previous Days
        static int Main_Day3Part2(string data)
        {
            var splitData = data.Split("\n");
            char[] lower = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            char[] upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            int totalScore = 0; 
            // find intersection.intersection.intersection of every three 
            for (int i = 0; i < splitData.Length - 3; i += 3)
            {
                var temp = splitData[i..(i + 3)];
                var intersectChar = temp[0].Intersect(temp[1]).Intersect(temp[2]);

                if (!intersectChar.Any()) continue;

                if (lower.Contains(intersectChar.First()))
                {
                    // lower case priority is 1 - 26.
                    // convert to ascii because C# cast to byte defaults to utf, 
                    // and I didn't want to learn how to do math in hexadecimal today, thanks. 
                    totalScore += Encoding.ASCII.GetBytes(intersectChar.First().ToString())[0] - 96;
                    continue; 
                }

                if (upper.Contains(intersectChar.First()))
                {
                    // upper case priority is 27-52. 
                    // so subtract 38 = x - 96 + 26. 
                    totalScore += Encoding.ASCII.GetBytes(intersectChar.First().ToString())[0] - 38; 
                }
            }
            return totalScore; 
        }


        static void Main_Day3Part1(string data)
        {
            var splitData = data.Split("\n");
            char[] lower = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            char[] upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            int sum = 0; 
            for (int i = 0; i < splitData.Length; i++)
            {
                if (splitData[i] == "") continue; 
                // split each string into 2 based on length. 
                int stringLength = splitData[i].Length;
                int splitIndex = stringLength / 2; 
                string compartment1 = splitData[i].Substring(0, splitIndex);
                string compartment2 = splitData[i].Substring(splitIndex);
                
                // find common letters between the two compartments 
                var commonList = compartment1.Intersect(compartment2);
                var commonList2 = compartment2.Intersect(compartment1);
                
                var commonChars = commonList2.Concat(commonList).Distinct(); 

                int temp = 0;
                foreach (char c in commonChars)
                {
                    if (lower.Contains(c))
                    {
                        // lower case priority is 1 - 26.
                        temp += (Encoding.ASCII.GetBytes(c.ToString())[0] - 96); 
                    }

                    if (upper.Contains(c))
                    {
                        // upper case priority is 27-52. 
                        temp += (Encoding.ASCII.GetBytes(c.ToString())[0] - 38); 
                    }
                }
                sum += temp;
            }

            Console.WriteLine(sum);

        }



        static void Main_Day2Part1(string data)
        {
            var splitData = data.Split("\n");
            int cumScore = 0;
            for (int i = 0; i < splitData.Length; i++)
            {
                if (splitData[i] == "") continue;
                var bytes = Encoding.ASCII.GetBytes(splitData[i]);
                int tempSum = bytes[2] - bytes[0];

                if (tempSum % 3 == 0)
                {
                    cumScore += 6 + bytes[2] - 87;
                }
                if (tempSum % 3 == 1)
                {
                    cumScore += 0 + bytes[2] - 87;
                }
                if (tempSum % 3 == 2)
                {
                    cumScore += 3 + bytes[2] - 87;
                }
            }

            Console.WriteLine(cumScore);
        }
        static void Main_Day2Part2(string data)
        {
            var splitData = data.Split("\n");
            int cumScore = 0;
            for (int i = 0; i < splitData.Length; i++)
            {
                if (splitData[i] == "") continue;
                var bytes = Encoding.ASCII.GetBytes(splitData[i]);
                // Lose = X (88); Win = Z(90); Draw = Y(89); 
                //int tempSum = bytes[2] - bytes[0];
                // calculate round score

                // tempSum tells you what you're playing. 
                int playScore = ((bytes[2] + bytes[0] - 64) % 3) + 1;
                // winVal gives the score of the outcome. 
                int winVal = 6 - 3 * (90 - bytes[2]);

                cumScore += (playScore + winVal);
            }

            Console.WriteLine(cumScore);
        }
        static void Main_Day1(string[] args)
        {

            string cookie = ReadCookie();
            byte[] buffer = GetInput(1, 2022, cookie).Result;
            string data = Encoding.UTF8.GetString(buffer);
            int maxCal = Day1Part1(data);
            // successful day 1! 
            Console.WriteLine(maxCal);
            Console.WriteLine(Day1Part2(data));
            // successful day 2! 
        }
        public static int Day1Part1(string data)
        {
            string[] elfs = data.Split('\n');
            List<int> elfCalList = new();
            int temp = 0; 
            for(int i = 0; i < elfs.Length; i++)
            {
                if (elfs[i] != "")
                {
                    temp += Convert.ToInt32(elfs[i]);
                }

                if (elfs[i] == "")
                {
                    elfCalList.Add(temp);
                    temp = 0; 
                }
            }

            return elfCalList.Max(); 
        }
        public static int Day1Part2(string data)
        {
            string[] elfs = data.Split('\n');
            List<int> elfCalList = new();
            int temp = 0;
            for (int i = 0; i < elfs.Length; i++)
            {
                if (elfs[i] != "")
                {
                    temp += Convert.ToInt32(elfs[i]);
                }

                if (elfs[i] == "")
                {
                    elfCalList.Add(temp);
                    temp = 0;
                }
            }

            var orderedElfCalList = elfCalList.OrderByDescending(i => i);
            return orderedElfCalList.Take(3).Sum();
        }
#endregion
        #region Utilities
        /// <summary>
        /// Get the input from the input url depending on day, year and the cookie. Writes to a file in bin. 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="year"></param>
        /// <param name="cookie"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        static async Task GetInput(int day, int year, string cookie, string filename)
        {
            var uri = new Uri("https://adventofcode.com");
                var cookies = new CookieContainer();
                cookies.Add(uri, new System.Net.Cookie("session", cookie));
                using var file = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
                using var handler = new HttpClientHandler() { CookieContainer = cookies };
                using var client = new HttpClient(handler) { BaseAddress = uri };
                using var response = await client.GetAsync($"/{year}/day/{day}/input");
                using var stream = await response.Content.ReadAsStreamAsync();
                await stream.CopyToAsync(file);
        }

        static async Task<byte[]> GetInput(int day, int year, string cookie)
        {

            using MemoryStream ms = new(); 
            var cookies = new CookieContainer();
            cookies.Add(_uri, new System.Net.Cookie("session", cookie));
            using var handler = new HttpClientHandler() { CookieContainer = cookies };
            using var client = new HttpClient(handler) { BaseAddress = _uri };
            using var response = await client.GetAsync($"/{year}/day/{day}/input");
            using var stream = await response.Content.ReadAsStreamAsync();

            await stream.CopyToAsync(ms);

            return ms.ToArray();
        }
        /// <summary>
        /// Loads a file called .cookie that has a unique key that allows you to get your own
        /// dataset from the advent of code website. 
        /// </summary>
        /// <returns></returns>
        private static string ReadCookie()
        {
            using StreamReader reader= new StreamReader(_cookieFile);
            return reader.ReadToEnd(); 
        }
#endregion
    }
}