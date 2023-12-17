using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace AdventOfCode2023
{
    internal class D12_2
    {
        private static long HashCode(string chars, ImmutableList<int> arrangement)
        {
            long res = 0;
            foreach (var c in arrangement)
                res = unchecked(res * 31 + c);
            return (res << 20) + res ^ chars.GetHashCode();
        }
        public static void Run()
        {
            ConcurrentDictionary<long, long> memo = [];
            long ReturnWithMemo(long res, string chars, ImmutableList<int> arrangement)
            {
                memo[HashCode(chars, arrangement)] = res;
                return res;
            }

            long Calc(string chars, ImmutableList<int> arrangement)
            {
                chars = string.Concat(chars.SkipWhile(c => c == '.'));
                if (arrangement.Count == 0)
                    if (chars.Contains('#'))
                        return 0;
                    else
                        return 1;
                if (memo.TryGetValue(HashCode(chars, arrangement), out long value))
                    return value;
                if (arrangement.Sum() > chars.Count(c => c != '.'))
                    return 0;

                if (chars.StartsWith('#'))
                {
                    int count = chars.TakeWhile(c => c != '.').Count();
                    long res;
                    if (count >= arrangement[0] && (chars.Length <= arrangement[0] || chars[arrangement[0]] != '#'))
                        res = Calc(string.Concat(chars.Skip(arrangement[0] + 1)), arrangement.RemoveAt(0));
                    else
                        res = 0;
                    return ReturnWithMemo(res, chars, arrangement);
                }
                else
                {
                    long res = Calc(chars[1..], arrangement);
                    res += Calc(string.Concat("#", chars.AsSpan(1)), arrangement);
                    return ReturnWithMemo(res, chars, arrangement);
                }
            }
            long sum = File.ReadLines("inputs/12").AsParallel().Sum(line =>
            {
                var parts = line.Split(" ");
                string initRecord = parts[0];
                var initArrangement = parts[1].Split(",").Select(int.Parse).ToImmutableList();
                string record = initRecord;
                var arrangement = initArrangement;
                for (int i = 0; i < 4; i++)
                {
                    record += "?" + initRecord;
                    arrangement = [.. arrangement, .. initArrangement];
                }
                return Calc(record, arrangement);
            });
            Console.WriteLine(sum);
        }
    }
}
