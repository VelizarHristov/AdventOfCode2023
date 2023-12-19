namespace AdventOfCode2023
{
    internal class D18
    {
        static readonly Dictionary<char, (int, int)> dirMap = new()
        {
            { 'U', (-1, 0) },
            { 'D', (1, 0) },
            { 'L', (0, -1) },
            { 'R', (0, 1) }
        };
        private static HashSet<(int, int)> GetTiles()
        {
            HashSet<(int, int)> tiles = [];
            int currentY = 0;
            int currentX = 0;
            foreach (var line in File.ReadLines("inputs/18"))
            {
                var parts = line.Split(' ');
                var (dy, dx) = dirMap[parts[0][0]];
                var len = int.Parse(parts[1]);
                foreach (var _ in Enumerable.Range(0, len))
                {
                    currentY += dy;
                    currentX += dx;
                    tiles.Add((currentY, currentX));
                }
            }
            var minY = tiles.MinBy(t => t.Item1).Item1;
            var minX = tiles.MinBy(t => t.Item2).Item2;
            return tiles.Select(yx =>
            {
                var (y, x) = yx;
                return (y - minY, x - minX);
            }).ToHashSet();
        }
        static readonly int OUTSIDE = 0;
        static readonly int INSIDE = 1;
        static readonly int UNKNOWN = 2;
        public static void Run()
        {
            var tiles = GetTiles();
            int maxY = tiles.MaxBy(t => t.Item1).Item1;
            int maxX = tiles.MaxBy(t => t.Item2).Item2;

            var gridStates = new int[maxY + 1][];
            for (int i = 0; i <= maxY; i++)
            {
                gridStates[i] = new int[maxX + 1];
                for (int j = 0; j <= maxX; j++)
                    gridStates[i][j] = tiles.Contains((i, j)) ? INSIDE : UNKNOWN;
            }
            (HashSet<(int, int)>, bool) bfs(int startY, int startX)
            {
                HashSet<(int, int)> visited = [];
                List<(int, int)> next = [(startY, startX)];
                bool reachesOutside = false;
                while (next.Count != 0)
                {
                    var (y, x) = next[0];
                    next.RemoveAt(0);
                    if (visited.Contains((y, x)))
                        continue;
                    foreach (var (dy, dx) in dirMap.Values.ToList())
                    {
                        int state = Helpers.SafeGet(gridStates, y + dy, x + dx);
                        if (state == OUTSIDE)
                            reachesOutside = true;
                        else if (state == UNKNOWN && !visited.Contains((y + dy, x + dx)))
                            next.Add((y + dy, x + dx));
                    }
                    visited.Add((y, x));
                }
                return (visited, reachesOutside);
            }
            for (int i = 0; i <= maxY; i++)
                for (int j = 0; j <= maxX; j++)
                    if (gridStates[i][j] == UNKNOWN)
                    {
                        var (visited, reachesOutside) = bfs(i, j);
                        foreach (var (y, x) in visited)
                            gridStates[y][x] = reachesOutside ? OUTSIDE : INSIDE;
                    }

            int res = gridStates.Sum(line => line.Count(i => i == INSIDE));
            Console.WriteLine(res);
        }
    }
}
