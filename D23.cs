using System.Collections.Immutable;

namespace AdventOfCode2023
{
    internal class D23
    {
        static readonly Dictionary<char, (int, int)> DIRS = new()
        {
            { '^', (-1, 0) },
            { '>', (0, 1) },
            { 'v', (1, 0) },
            { '<', (0, -1) }
        };
        record State(int Y, int X, ImmutableHashSet<(int, int)> Visited);

        public static void Run()
        {
            char[][] grid = File.ReadAllLines("inputs/23").Select(line => line.ToCharArray()).ToArray();
            List<State> states = [new(0, 1, [])];
            int bestSolutionLen = 0;
            while (states.Count != 0)
            {
                State next = states[0];
                states.RemoveAt(0);
                if (next.Y == grid.Length - 1 && next.X == grid[0].Length - 2)
                {
                    bestSolutionLen = Math.Max(bestSolutionLen, next.Visited.Count);
                    continue;
                }
                var nextDirs = DIRS.Values.ToList();
                if (DIRS.TryGetValue(grid[next.Y][next.X], out var dydx))
                    nextDirs = [dydx];
                foreach (var (dy, dx) in nextDirs)
                {
                    var nextTile = Helpers.SafeGet(grid, next.Y + dy, next.X + dx);
                    if (!next.Visited.Contains((next.Y + dy, next.X + dx)) &&
                        nextTile != '#' && nextTile != default)
                    {
                        states.Add(new(next.Y + dy, next.X + dx, next.Visited.Add((next.Y, next.X))));
                    }
                }
            }
            Console.WriteLine(bestSolutionLen);
        }
    }
}
