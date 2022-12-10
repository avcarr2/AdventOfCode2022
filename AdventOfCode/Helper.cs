using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Helper
    {
        public static char[] BufferToChars(byte[] buffer)
        {
            char[] chars = Encoding.UTF8.GetChars(buffer);
            return chars;
        }

        public static string[] BufferToStrings(byte[] buffer, char split)
        {
            string[] stringArray = Encoding.UTF8.GetString(buffer).Split(split);
            return stringArray;
        }

        private static bool CheckDuplicate<T>(Queue<T> queue) where T : struct
        {
            Queue<T> queueCopy = new Queue<T>(queue);
            Dictionary<T, int> queueDict = new();
            while (queueCopy.TryDequeue(out var nextItem))
            {
                if (!queueDict.TryAdd(nextItem, 1)) return true;

            }
            return false;
        }

        public static Stack<T> ReverseStack<T>(Stack<T> forwardStack)
        {
            Stack<T> reverseStack = new();
            while (forwardStack.TryPop(out var box))
            {
                reverseStack.Push(box);
            }
            return reverseStack;
        }
    }
}
