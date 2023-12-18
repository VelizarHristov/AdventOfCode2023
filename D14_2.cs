namespace AdventOfCode2023
{
    internal class D14_2
    {
        public static void Run()
        {
            char[][] grid = File.ReadLines("inputs/14").Select(line => line.ToCharArray()).ToArray();
            char[][] prevGrid = new char[grid.Length][];
            foreach (int i in Enumerable.Range(0, grid.Length))
                prevGrid[i] = [];
            List<char[][]> prevStates = [];
            int firstCycleIdx = -1;
            while (firstCycleIdx == -1)
            {
                prevGrid = grid.Select(line => (char[])line.Clone()).ToArray();
                prevStates.Add(prevGrid);
                List<(int, int)> directions = [(-1, 0), (0, -1), (1, 0), (0, 1)];
                foreach (var (dy, dx) in directions)
                {
                    var yRange = Enumerable.Range(0, grid.Length);
                    var xRange = Enumerable.Range(0, grid[0].Length);
                    if (dy == 1)
                        yRange = yRange.Reverse();
                    if (dx == 1)
                        xRange = xRange.Reverse();
                    foreach (int i in yRange)
                        foreach (int j in xRange)
                            if (grid[i][j] == 'O')
                            {
                                int tilesToMove = 0;
                                while (Helpers.SafeGet(grid,
                                    i + dy * (tilesToMove + 1),
                                    j + dx * (tilesToMove + 1)) == '.')
                                {
                                    tilesToMove++;
                                }
                                grid[i][j] = '.';
                                grid[i + dy * tilesToMove][j + dx * tilesToMove] = 'O';
                            }
                }
                firstCycleIdx = prevStates.FindIndex(prevState =>
                    Enumerable.Range(0, grid.Length).All(i => grid[i].SequenceEqual(prevState[i])));
            }
            var statesInCycle = prevStates[firstCycleIdx..];
            int stateIdx = (1000000000 - firstCycleIdx) % statesInCycle.Count;
            char[][] lastState = statesInCycle[stateIdx];
            int sum = Enumerable.Range(0, lastState.Length).Sum(i =>
                (lastState.Length - i) * lastState[i].Count(c => c == 'O'));
            Console.WriteLine(sum);
        }
    }
}
