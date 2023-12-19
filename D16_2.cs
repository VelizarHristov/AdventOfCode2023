namespace AdventOfCode2023
{
    internal class D16_2
    {
        public static void Run()
        {
            char[][] grid = File.ReadLines("inputs/16")
                .Select(line => line.ToCharArray()).ToArray();
            List<(int, int, int, int)> startPositions = 
                Enumerable.Range(0, grid.Length).Select(y => (y, 0, 0, 1))
                .Concat(Enumerable.Range(0, grid.Length).Select(y => (y, grid[0].Length - 1, 0, -1))
                ).Concat(Enumerable.Range(0, grid[0].Length).Select(x => (0, x, 1, 0))
                ).Concat(Enumerable.Range(0, grid[0].Length).Select(x => (grid.Length - 1, x, -1, 0))
                ).ToList();
            int res = startPositions.Select(state =>
            {
                var energized = new bool[grid.Length][];
                for (int i = 0; i < grid.Length; i++)
                    energized[i] = Enumerable.Repeat(false, grid[0].Length).ToArray();
                var visited = new HashSet<(int, int)>[grid.Length][];
                for (int i = 0; i < grid.Length; i++)
                {
                    visited[i] = new HashSet<(int, int)>[grid[0].Length];
                    for (int j = 0; j < grid.Length; j++)
                        visited[i][j] = [];
                }
                void runBeam(int y, int x, int dy, int dx)
                {
                    char tile = Helpers.SafeGet(grid, y, x);
                    if (tile == default || visited[y][x].Contains((dy, dx)))
                        return;
                    visited[y][x].Add((dy, dx));
                    energized[y][x] = true;
                    if (tile == '.' || (tile == '|' && dx == 0) || (tile == '-' && dy == 0))
                        runBeam(y + dy, x + dx, dy, dx);
                    else if (tile == '|')
                    {
                        runBeam(y + 1, x, 1, 0);
                        runBeam(y - 1, x, -1, 0);
                    }
                    else if (tile == '-')
                    {
                        runBeam(y, x + 1, 0, 1);
                        runBeam(y, x - 1, 0, -1);
                    }
                    else if (tile == '/')
                        runBeam(y - dx, x - dy, -dx, -dy);
                    else if (tile == '\\')
                        runBeam(y + dx, x + dy, dx, dy);
                }
                var (y, x, dy, dx) = state;
                runBeam(y, x, dy, dx);
                return energized.Sum(line => line.Count(b => b));
            }).Max();
            Console.WriteLine(res);
        }
    }
}
