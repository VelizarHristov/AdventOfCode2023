namespace AdventOfCode2023
{
    internal class D13_2
    {
        public static List<int> RangeToList(int from, int to)
        {
            List<int> ls = [];
            for (int i = from; i <= to; i++)
                ls.Add(i);
            return ls;
        }
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
                var colIdxCheck = RangeToList(0, grid[0].Length - 2).Where(lineOffset =>
                    grid.Sum(row =>
                        RangeToList(0, Math.Min(lineOffset, grid[0].Length - lineOffset - 2)).Count(i =>
                            row[lineOffset - i] != row[lineOffset + i + 1])) == 1);
                if (colIdxCheck.Any())
                {
                    return colIdxCheck.First() + 1;
                }
                else
                {
                    int rowIdx = RangeToList(0, grid.Length - 2).First(lineOffset =>
                        RangeToList(0, Math.Min(lineOffset, grid.Length - lineOffset - 2)).Sum(i =>
                            RangeToList(0, grid[0].Length - 1).Count(j =>
                                grid[lineOffset - i][j] != grid[lineOffset + i + 1][j])) == 1);
                    return 100 * (1 + rowIdx);
                }
            });
            Console.WriteLine(sum);
        }
    }
}
