using System.Numerics;

namespace AdventOfCode2023
{
    internal class D11_2
    {
        public static void Run()
        {
            bool[][] grid = File.ReadLines("inputs/11")
                .Select(line => line.Select(c => c == '#').ToArray()).ToArray();
            HashSet<int> bigRows = [];
            HashSet<int> bigCols = [];
            for (int i = 0; i < grid.Length; i++)
            {
                if (grid[i].All(c => !c))
                    bigRows.Add(i);
                if (grid.All(line => !line[i]))
                    bigCols.Add(i);
            }
            List<(int, int)> galaxies = [];
            for (int i = 0; i < grid.Length; i++)
                for (int j = 0; j < grid[i].Length; j++)
                    if (grid[i][j])
                        galaxies.Add((i, j));
            BigInteger totalLength = 0;
            BigInteger bigCount = 0;
            foreach (var (y1, x1) in galaxies)
                foreach (var (y2, x2) in galaxies)
                    if (y1 < y2 || (y1 == y2 && x1 < x2))
                    {
                        for (int y = y1; y < y2; y++)
                            if (bigRows.Contains(y))
                                bigCount++;
                            else
                                totalLength++;
                        for (int x = Math.Min(x1, x2); x < Math.Max(x1, x2); x++)
                            if (bigCols.Contains(x))
                                bigCount++;
                            else
                                totalLength++;
                    }
            totalLength += bigCount * 1000000;
            Console.WriteLine(totalLength);
        }
    }
}
