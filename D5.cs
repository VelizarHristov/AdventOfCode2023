namespace AdventOfCode2023
{
    internal class D5
    {
        public static void Run()
        {
            string[] lines = File.ReadAllLines("inputs/5");
            var seeds = lines[0][7..].Split(" ").Select(long.Parse).ToList();
            int curLine = 3;

            var nextItems = seeds;
            while (curLine < lines.Length)
            {
                var nextLines = lines[curLine..].TakeWhile(line => line.Length > 0);
                var map = nextLines.Select(line =>
                {
                    var nums = line.Split(" ").Select(long.Parse).ToList();
                    return (nums[0], nums[1], nums[2]);
                });

                nextItems = nextItems.Select(item =>
                {
                    var (yStart, xStart, _) = map.FirstOrDefault(mapItem =>
                    {
                        var (yStart, xStart, length) = mapItem;
                        return xStart <= item && xStart + length > item;
                    }, (item, item, 0));
                    return yStart + (item - xStart);
                }).ToList();
                curLine += nextLines.Count() + 2;
            }

            Console.WriteLine(nextItems.Min());
        }
    }
}
