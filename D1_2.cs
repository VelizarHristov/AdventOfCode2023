namespace AdventOfCode2023
{
    internal class D1_2
    {
        public static void Run()
        {
            var DIGITS = new Dictionary<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 },
                { "six", 6 },
                { "seven", 7 },
                { "eight", 8 },
                { "nine", 9 }
            };

            int sum = File.ReadLines("inputs/1").Sum(line =>
            {
                int? first = null;
                int? last = null;
                string acc = "";
                foreach (var c in line)
                {
                    int? found = null;
                    acc += c;
                    if (char.IsDigit(c))
                    {
                        found = c - '0';
                    }
                    else
                    {
                        for (int len = 3; len <= Math.Min(5, acc.Length); len++)
                        {
                            var digit = acc.Substring(acc.Length - len, len);
                            if (DIGITS.ContainsKey(digit))
                                found = DIGITS[digit];
                        }
                    }

                    if (found != null)
                    {
                        first ??= found;
                        last = found;
                    }
                }
                return first.Value * 10 + last.Value;
            });
            Console.WriteLine(sum);
        }
    }
}
