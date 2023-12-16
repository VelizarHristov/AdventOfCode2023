namespace AdventOfCode2023
{
    internal class D10_2
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
            HashSet<(int, int)> startDirections = [];
            foreach (var (dy, dx) in canGo['S'])
            {
                int newY = start.Item1 + dy;
                int newX = start.Item2 + dx;
                if (Helpers.SafeGet(grid, newY, newX) is char next && next != default)
                    if (CanReach(next, newY, newX, start.Item1, start.Item2))
                    {
                        curCoords = (newY, newX);
                        startDirections.Add((dy, dx));
                    }
            }
            foreach (var kv in canGo)
                if (kv.Value.ToHashSet().SetEquals(startDirections))
                    grid[start.Item1][start.Item2] = kv.Key;
            HashSet<(int, int)> prevLocations = [start, curCoords];
            while (curCoords != start)
            {
                var (y, x) = curCoords;
                var next = canGo[grid[y][x]].FirstOrDefault(d =>
                {
                    var (dy, dx) = d;
                    return !prevLocations.Contains((y + dy, x + dx));
                });
                var (dy, dx) = next;
                var newLocation = (y + dy, x + dx);
                if (next == default)
                    newLocation = start;
                prevLocations.Add(newLocation);
                curCoords = newLocation;
            }

            int total = 0;
            for (int i = 0; i < grid.Length; i++)
            {
                bool isInside = false;
                int ABOVE = 1;
                int BELOW = 2;
                int cameFrom = ABOVE; // initial value is unused
                for (int j = 0; j < grid[i].Length; j++)
                {
                    char tile = grid[i][j];
                    if (!prevLocations.Contains((i, j)))
                        tile = '.';
                    if (tile == '|')
                        isInside = !isInside;
                    if (tile == 'L')
                        cameFrom = ABOVE;
                    if (tile == 'F')
                        cameFrom = BELOW;
                    if (tile == '7' && cameFrom == ABOVE ||
                        tile == 'J' && cameFrom == BELOW)
                        isInside = !isInside;
                    if (tile == '.' && isInside)
                        total++;
                }
            }
            Console.WriteLine("Total: " + total);
        }
    }
}
