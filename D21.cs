namespace AdventOfCode2023
{
    internal class D21
    {
        public static void Run()
        {
            var grid = File.ReadAllLines("inputs/21").Select(x => x.ToCharArray()).ToArray();
            HashSet<(int, int)> reachable = [];
            for (int i = 0; i < grid.Length; i++)
                for (int j = 0; j < grid[i].Length; j++)
                    if (grid[i][j] == 'S')
                        reachable.Add((i, j));
            grid[reachable.First().Item1][reachable.First().Item2] = '.';
            List<(int, int)> directions = [(-1, 0), (1, 0), (0, -1), (0, 1)];

            HashSet<(int, int)> nextReachable = [];
            for (int step = 0; step < 64; step++)
            {
                foreach (var (y, x) in reachable)
                    foreach (var (dy, dx) in directions)
                        if (Helpers.SafeGet(grid, y + dy, x + dx) == '.')
                            nextReachable.Add((y + dy, x + dx));
                reachable = nextReachable;
                nextReachable = [];
            }
            Console.WriteLine(reachable.Count);
        }
    }
}
