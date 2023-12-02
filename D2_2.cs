namespace AdventOfCode2023
{
    internal class D2_2
    {
        public static void Run()
        {
            int sum = File.ReadLines("inputs/2").Select(line =>
            {
                var maxes = new Dictionary<string, int>()
                {
                    { "red", 0 },
                    { "green", 0 },
                    { "blue", 0 }
                };
                var remaining = line.SkipWhile(c => c != ':').Skip(2);
                while (remaining.Count() > 0)
                {
                    string countStr = string.Concat(remaining.TakeWhile(char.IsDigit));
                    int count = int.Parse(countStr);
                    string color = string.Concat(remaining.Skip(countStr.Length + 1).TakeWhile(char.IsLetter));
                    maxes[color] = Math.Max(maxes[color], count);
                    remaining = remaining.SkipWhile(c => c != ',' && c != ';').Skip(2);
                }

                return maxes.Values.Aggregate((prod, next) => prod * next);
            }).Sum();
            Console.WriteLine(sum);
        }
    }
}
