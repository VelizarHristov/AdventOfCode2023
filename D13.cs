namespace AdventOfCode2023
{
    internal class D13
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
                    grid.All(row =>
                        Enumerable.Range(0, Math.Min(lineOffset + 1, grid[0].Length - lineOffset - 1)).All(i =>
                            row[lineOffset - i] == row[lineOffset + i + 1])));
                if (colIdxCheck.Any())
                {
                    return colIdxCheck.First() + 1;
                }
                else
                {
                    int rowIdx = Enumerable.Range(0, grid.Length - 1).First(lineOffset =>
                        Enumerable.Range(0, Math.Min(lineOffset + 1, grid.Length - lineOffset - 1)).All(i =>
                            grid[lineOffset - i].SequenceEqual(grid[lineOffset + i + 1])));
                    return 100 * (1 + rowIdx);
                }
            });
            Console.WriteLine(sum);
        }
    }
}
