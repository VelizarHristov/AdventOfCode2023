using System.Collections.Immutable;

namespace AdventOfCode2023
{
    internal class D12
    {
        public static void Run()
        {
            Dictionary<(string, ImmutableList<int>), int> memo = [];
            int ReturnWithMemo(int res, string chars, ImmutableList<int> arrangement)
            {
                memo[(chars, arrangement)] = res;
                return res;
            }

            int Calc(string chars, ImmutableList<int> arrangement)
            {
                chars = string.Concat(chars.SkipWhile(c => c == '.'));
                if (arrangement.Count == 0)
                    if (chars.Contains('#'))
                        return 0;
                    else
                        return 1;
                if (memo.TryGetValue((chars, arrangement), out int value))
                    return value;
                if (arrangement.Sum() > chars.Count(c => c != '.'))
                    return ReturnWithMemo(0, chars, arrangement);

                if (chars.StartsWith('#'))
                {
                    int count = chars.TakeWhile(c => c != '.').Count();
                    int res;
                    if (count >= arrangement[0] && (chars.Length <= arrangement[0] || chars[arrangement[0]] != '#'))
                        res = Calc(string.Concat(chars.Skip(arrangement[0] + 1)), arrangement.RemoveAt(0));
                    else
                        res = 0;
                    return ReturnWithMemo(res, chars, arrangement);
                }
                else
                {
                    int res = Calc(string.Concat(".", chars.AsSpan(1)), arrangement);
                    res += Calc(string.Concat("#", chars.AsSpan(1)), arrangement);
                    return ReturnWithMemo(res, chars, arrangement);
                }
            }
            int sum = File.ReadLines("inputs/12").Sum(line =>
            {
                var parts = line.Split(" ");
                string record = parts[0];
                var arrangement = parts[1].Split(",").Select(int.Parse).ToImmutableList();
                return Calc(record, arrangement);
            });
            Console.WriteLine(sum);
        }
    }
}
