using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    internal class D3
    {
        class Num(int value, int line, int startIdx, int length)
        {
            public bool isValid = false;
            public int Value { get; } = value;
            public int Line { get; } = line;
            public int StartIdx { get; } = startIdx;
            public int Length { get; } = length;
            public void EncounterSymbol(int line, int idx)
            {
                bool onAdjacentLine = Enumerable.Range(Line - 1, 3).Contains(line);
                bool onAdjacentIndex = Enumerable.Range(StartIdx - 1, Length + 2).Contains(idx);
                if (onAdjacentLine && onAdjacentIndex)
                    isValid = true;
            }
        }
        public static void Run()
        {
            var input = File.ReadAllLines("inputs/3");
            var numbers = input.SelectMany((line, lineNum) =>
                Regex.Matches(line, "\\d+").Select(m => new Num(int.Parse(m.Value), lineNum, m.Index, m.Value.Length))).ToArray();
            for (int lineNum = 0; lineNum < input.Length; lineNum++)
                for (int idx = 0; idx < input[lineNum].Length; idx++)
                    if (!char.IsDigit(input[lineNum][idx]) && input[lineNum][idx] != '.')
                        foreach (var num in numbers)
                            num.EncounterSymbol(lineNum, idx);
            int sum = numbers.Where(num => num.isValid).Sum(num => num.Value);
            Console.WriteLine(sum);
        }
    }
}
