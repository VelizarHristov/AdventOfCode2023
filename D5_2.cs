namespace AdventOfCode2023
{
    internal class D5_2
    {
        private static List<(long, long)> ParseSeeds(string[] lines)
        {
            var line1 = string.Concat(lines[0].Skip(7)).Split(" ").Select(long.Parse).ToArray();
            List<(long, long)> seeds = [];
            int curToken = 0;
            while (curToken < line1.Length)
            {
                seeds.Add((line1[curToken], line1[curToken + 1]));
                curToken += 2;
            }
            return seeds;
        }

        public static void Run()
        {
            string[] lines = File.ReadAllLines("inputs/5");
            int curLine = 3;

            var nextItems = ParseSeeds(lines);
            while (curLine < lines.Length)
            {
                var nextLines = lines.Skip(curLine).TakeWhile(line => line.Length > 0);
                var map = nextLines.Select(line =>
                {
                    var nums = line.Split(" ").Select(long.Parse).ToList();
                    return (nums[0], nums[1], nums[2]);
                }).OrderBy(y_x_l => y_x_l.Item2);

                nextItems = nextItems.SelectMany(item =>
                {
                    List<(long, long)> next = [];
                    var (itemStart, itemLength) = item;
                    long itemEnd = itemStart + itemLength - 1;
                    foreach (var (yStart, xStart, length) in map)
                    {
                        if (itemEnd < itemStart)
                            break;
                        long xToY(long x) => x - xStart + yStart;
                        long xEnd = xStart + length - 1;
                        if (xEnd < itemStart) { }
                        else if (xStart <= itemStart)
                        {
                            long len = Math.Min(itemEnd, xEnd) - itemStart + 1;
                            next.Add((xToY(itemStart), len));
                            itemStart += len;
                        }
                        else if (xStart <= itemEnd)
                        {
                            next.Add((itemStart, xStart - itemStart));
                            next.Add((xToY(xStart), itemEnd - xStart + 1));
                            itemStart = xEnd + 1;
                            itemEnd = xEnd;
                        }
                        else
                        {
                            next.Add((itemStart, itemEnd - itemStart));
                            itemStart = itemEnd + 1;
                        }
                    }
                    if (itemEnd >= itemStart)
                        next.Add((itemStart, itemEnd - itemStart + 1));
                    return next;
                }).Where(x => x.Item2 != 0).ToList();
                curLine += nextLines.Count() + 2;
            }

            Console.WriteLine(nextItems.MinBy(x => x.Item1).Item1);
        }
    }
}
