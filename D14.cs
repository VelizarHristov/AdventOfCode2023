namespace AdventOfCode2023
{
    internal class D14
    {
        public static void Run()
        {
            char[][] grid = File.ReadLines("inputs/14").Select(line => line.ToCharArray()).ToArray();
            for (int i = 0; i < grid.Length; i++)
                for (int j = 0; j < grid.Length; j++)
                    if (grid[i][j] == 'O')
                    {
                        int tilesToMove = 0;
                        while (Helpers.SafeGet(grid, i - tilesToMove - 1, j) == '.')
                            tilesToMove++;
                        grid[i][j] = '.';
                        grid[i - tilesToMove][j] = 'O';
                    }
            int sum = Enumerable.Range(0, grid.Length).Sum(i =>
                (grid.Length - i) * grid[i].Count(c => c == 'O'));
            Console.WriteLine(sum);
        }
    }
}
