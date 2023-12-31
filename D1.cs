﻿namespace AdventOfCode2023
{
    internal class D1
    {
        public static void Run()
        {
            int sum = File.ReadLines("inputs/1").Sum(line =>
            {
                int first = line.First(char.IsDigit) - '0';
                int last = line.Last(char.IsDigit) - '0';
                return first * 10 + last;
            });
            Console.WriteLine(sum);
        }
    }
}
