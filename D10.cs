namespace AdventOfCode2023
{
    internal class D10
    {
        static (int, int) up = (-1, 0);
        static (int, int) down = (1, 0);
        static (int, int) left = (0, -1);
        static (int, int) right = (0, 1);
        static readonly Dictionary<char, List<(int, int)>> canGo = new()
        {
            { '|', [up, down] },
            { '-', [left, right] },
            { 'L', [up, right] },
            { 'J', [up, left] },
            { '7', [down, left] },
            { 'F', [down, right] },
            { '.', [] },
            { 'S', [up, down, left, right] },
        };
        static bool CanReach(char pipe, int fromY, int fromX, int toY, int toX)
        {
            return canGo[pipe].Exists(dydx =>
                (fromY + dydx.Item1, fromX + dydx.Item2) == (toY, toX));
        }

        public static void Run()
        {
            var grid = File.ReadAllLines("inputs/10").Select(x => x.ToCharArray()).ToArray();
            (int, int) start = (-1, -1);
            for (int i = 0; i < grid.Length; i++)
                for (int j = 0; j < grid[i].Length; j++)
                    if (grid[i][j] == 'S')
                        start = (i, j);
            var curCoords = start;
            foreach (var (dy, dx) in canGo['S'])
            {
                int newY = start.Item1 + dy;
                int newX = start.Item2 + dx;
                if (Helpers.SafeGet(grid, newY, newX) is char next && next != default)
                    if (CanReach(next, newY, newX, start.Item1, start.Item2))
                        curCoords = (newY, newX);
            }
            int loopLength = 2;
            var prevLocation = start;
            while (curCoords != start)
            {
                foreach (var (dy, dx) in canGo[grid[curCoords.Item1][curCoords.Item2]])
                {
                    var newLocation = (curCoords.Item1 + dy, curCoords.Item2 + dx);
                    if ((curCoords.Item1 + dy, curCoords.Item2 + dx) != prevLocation)
                    {
                        prevLocation = curCoords;
                        curCoords = newLocation;
                        break;
                    }
                }
                loopLength++;
            }
            Console.WriteLine(loopLength / 2);
        }
    }
}
