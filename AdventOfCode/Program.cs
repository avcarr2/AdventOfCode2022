using System.Net;
using System.Text;
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
            // Main_Day1(args);
        }


        #region Previous Days
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