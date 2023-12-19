namespace AdventOfCode2023
{
    internal class D16
    {
        public static void Run()
        {
            char[][] grid = File.ReadLines("inputs/16")
                .Select(line => line.ToCharArray()).ToArray();
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
            runBeam(0, 0, 0, 1);
            var res = energized.Sum(line => line.Count(b => b));
            Console.WriteLine(res);
        }
    }
}
