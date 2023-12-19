namespace AdventOfCode2023
{
    internal class D18_2
    {
        static readonly Dictionary<char, (int, int)> dirMap = new()
        {
            { '0', (0, 1) },
            { '1', (1, 0) },
            { '2', (0, -1) },
            { '3', (-1, 0) }
        };
        public static void Run()
        {
            List<(long, long)> tiles = [(0, 0)];
            long currentY = 0;
            long currentX = 0;
            long length = 0;
            foreach (var line in File.ReadLines("inputs/18"))
            {
                var parts = line.Split(' ')[2];
                var (dy, dx) = dirMap[parts[7]];
                var len = long.Parse(parts[2..7], System.Globalization.NumberStyles.HexNumber);
                currentY += dy * len;
                currentX += dx * len;
                tiles.Add((currentY, currentX));
                length += len;
            }
            var area = Math.Abs(tiles.Zip(tiles.Skip(1)).Sum(pair =>
            {
                var ((y1, x1), (y2, x2)) = pair;
                return y1 * x2 - x1 * y2; // shoelace formula
            })) / 2;
            long innerPointCount = area - length / 2 + 1; // pick's theorem
            long res = innerPointCount + length;
            Console.WriteLine(res);
        }
    }
}
