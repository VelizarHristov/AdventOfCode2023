using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    internal class D3_2
    {
        class Num(int value, int line, int startIdx, int length)
        {
            public int Value { get; } = value;
            public int Line { get; } = line;
            public int StartIdx { get; } = startIdx;
            public int Length { get; } = length;
            public bool IsAdjacent(int line, int idx)
            {
                bool onAdjacentLine = Enumerable.Range(Line - 1, 3).Contains(line);
                bool onAdjacentIndex = Enumerable.Range(StartIdx - 1, Length + 2).Contains(idx);
                return onAdjacentLine && onAdjacentIndex;
            }
        }
        public static void Run()
        {
            var input = File.ReadAllLines("inputs/3");
            var numbers = input.SelectMany((line, lineNum) =>
                Regex.Matches(line, "\\d+").Select(m => new Num(int.Parse(m.Value), lineNum, m.Index, m.Value.Length))).ToArray();
            int sum = 0;
            for (int lineNum = 0; lineNum < input.Length; lineNum++)
                for (int idx = 0; idx < input[lineNum].Length; idx++)
                    if (input[lineNum][idx] == '*')
                    {
                        var matches = numbers.Where(n => n.IsAdjacent(lineNum, idx)).ToList();
                        if (matches.Count == 2)
                            sum += matches[0].Value * matches[1].Value;
                    }
            Console.WriteLine(sum);
        }
    }
}
