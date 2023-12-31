﻿using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    internal class D4
    {
        public static void Run()
        {
            int sum = File.ReadLines("inputs/4").Sum(line =>
            {
                var res = line[9..].Split("|");
                var winningNums = Regex.Matches(res[0], "\\d+").Select(x => x.Value).ToHashSet();
                var count = Regex.Matches(res[1], "\\d+").Count(x => winningNums.Contains(x.Value));
                return Convert.ToInt32(Math.Pow(2, count - 1));
            });
            Console.WriteLine(sum);
        }
    }
}
