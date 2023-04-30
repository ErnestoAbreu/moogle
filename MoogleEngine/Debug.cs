namespace MoogleEngine;
using System.Diagnostics;

public class Debug
{
    public static Stopwatch watch = Stopwatch.StartNew();

    public static void Write()
    {
        Console.WriteLine();
    }

    public static void Write(string s)
    {
        Console.WriteLine(s);
    }

    public static void Write(string s, float num)
    {
        Console.WriteLine(s, num);
    }

    public static void StartTime()
    {
        watch = Stopwatch.StartNew();
    }

    public static float GetTime()
    {
        return watch.ElapsedMilliseconds / 1000;
    }
}
