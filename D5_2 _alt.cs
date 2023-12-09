namespace AdventOfCode2023
{
    internal class D5_2_Alt
    {
        // Range of longs
        readonly struct LRange(long start, long end)
        {
            public static LRange FromLength(long start, long length)
            {
                return new LRange(start, start + length - 1);
            }

            public long Start { get; } = start;
            public long End { get; } = end;
            public long Length() { return End - Start + 1; }

            public LRange? Intersection(LRange that)
            {
                var (r1, r2) = (this, that);
                if (Start > that.Start)
                    (r2, r1) = (r1, r2);
                if (r1.End < r2.Start)
                    return null;
                return new(r2.Start, Math.Min(r1.End, r2.End));
            }

            public LRange? DiffSmaller(LRange that)
            {
                if (Start >= that.Start)
                    return null;
                return new(Start, Math.Min(End, that.Start));
            }

            public LRange? DiffGreater(LRange that)
            {
                if (End <= that.End)
                    return null;
                return new(Math.Max(Start, that.End), End);
            }
        }
        private static List<LRange> ParseSeeds(string[] lines)
        {
            var line1 = lines[0][7..].Split(" ").Select(long.Parse).ToArray();
            List<LRange> seeds = [];
            int curToken = 0;
            while (curToken < line1.Length)
            {
                seeds.Add(LRange.FromLength(line1[curToken], line1[curToken + 1]));
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

                nextItems = nextItems.SelectMany(item_ =>
                {
                    List<LRange> next = [];
                    LRange? item = item_;
                    foreach (var (yStart, xStart, length) in map)
                    {
                        if (item == null)
                            break;
                        long xToY(long x) => x - xStart + yStart;
                        var mapRange = LRange.FromLength(xStart, length);
                        var unmapped = item.Value.DiffSmaller(mapRange);
                        var mapped = item.Value.Intersection(mapRange);
                        item = item.Value.DiffGreater(mapRange);

                        if (unmapped != null)
                            next.Add(unmapped.Value);
                        if (mapped != null)
                            next.Add(new LRange(xToY(mapped.Value.Start), xToY(mapped.Value.End)));
                    }
                    if (item != null)
                        next.Add(item.Value);
                    return next;
                }).Where(x => x.Length() != 0).ToList();
                curLine += nextLines.Count() + 2;
            }

            Console.WriteLine(nextItems.MinBy(x => x.Start).Start);
        }
    }
}
