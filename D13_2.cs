namespace AdventOfCode2023
{
    internal class D13_2
    {
        public static void Run()
        {
            string[] lines = File.ReadAllLines("inputs/13");
            int linesPassed = 0;
            List<bool[][]> grids = [];
            while (linesPassed < lines.Length) {
                var next = lines.Skip(linesPassed).TakeWhile(line => line.Length > 0);
                grids.Add(next.Select(line => line.Select(x => x == '#').ToArray()).ToArray());
                linesPassed += next.Count() + 1;
            }
            int sum = grids.Sum(grid =>
            {
                var colIdxCheck = Enumerable.Range(0, grid[0].Length - 1).Where(lineOffset =>
                    grid.Sum(row =>
                        Enumerable.Range(0, Math.Min(lineOffset + 1, grid[0].Length - lineOffset - 1)).Count(i =>
                            row[lineOffset - i] != row[lineOffset + i + 1])) == 1);
                if (colIdxCheck.Any())
                {
                    return colIdxCheck.First() + 1;
                }
                else
                {
                    int rowIdx = Enumerable.Range(0, grid.Length - 1).First(lineOffset =>
                        Enumerable.Range(0, Math.Min(lineOffset + 1, grid.Length - lineOffset - 1)).Sum(i =>
                            Enumerable.Range(0, grid[0].Length).Count(j =>
                                grid[lineOffset - i][j] != grid[lineOffset + i + 1][j])) == 1);
                    return 100 * (1 + rowIdx);
                }
            });
            Console.WriteLine(sum);
        }
    }
}
