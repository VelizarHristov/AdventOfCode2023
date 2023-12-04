using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    internal class D4_2
    {
        public static void Run()
        {
            string[] lines = File.ReadAllLines("inputs/4");
            int[] cardCounts = Enumerable.Repeat(1, lines.Count()).ToArray();
            for (int curNum = 0; curNum < lines.Count(); curNum++)
            {
                var res = string.Concat(lines[curNum].Skip(9)).Split("|");
                var winningNums = Regex.Matches(res[0], "\\d+").Select(x => x.Value).ToHashSet();
                var count = Regex.Matches(res[1], "\\d+").Count(x => winningNums.Contains(x.Value));
                for (int i = 0; i < count; i++)
                    cardCounts[curNum + 1 + i] += cardCounts[curNum];
            }
            Console.WriteLine(cardCounts.Sum());
        }
    }
}
