namespace AdventOfCode2023
{
    internal class D2
    {
        public static void Run()
        {
            var LIMITS = new Dictionary<string, int>()
            {
                { "red", 12 },
                { "green", 13 },
                { "blue", 14 }
            };

            int sum = File.ReadLines("inputs/2").Select(line =>
            {
                var remaining = line.SkipWhile(c => c != ':').Skip(2);
                while (remaining.Count() > 0)
                {
                    string countStr = string.Concat(remaining.TakeWhile(char.IsDigit));
                    int count = int.Parse(countStr);
                    string color = string.Concat(remaining.Skip(countStr.Length + 1).TakeWhile(char.IsLetter));
                    if (LIMITS[color] < count)
                        return 0;
                    remaining = remaining.SkipWhile(c => c != ',' && c != ';').Skip(2);
                }

                return int.Parse(string.Concat(line.Skip(5).TakeWhile(char.IsDigit)));
            }).Sum();
            Console.WriteLine(sum);
        }
    }
}
