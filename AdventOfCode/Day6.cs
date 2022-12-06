namespace AdventOfCode;

public static class Day6
{
    public static bool CheckQueue(this Queue<char> qChar)
    {
        return qChar.Distinct().Count() == 4; 
    }
    public static bool CheckQueuePart2(this Queue<char> qChar)
    {
        return qChar.Distinct().Count() == 14;
    }
}