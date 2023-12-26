namespace AdventOfCode2023
{
    internal class D22_2
    {
        record Brick(int X1, int Y1, int Z1, int X2, int Y2, int Z2);
        public static void Run()
        {
            var bricks = File.ReadLines("inputs/22").Select(line =>
            {
                var parts = line.Split('~');
                var p1 = parts[0].Split(',').Select(int.Parse).ToList();
                var p2 = parts[1].Split(',').Select(int.Parse).ToList();
                return new Brick(p1[0], p1[1], p1[2], p2[0], p2[1], p2[2]);
            }).OrderBy(b => b.Z1).ToList();
            List<HashSet<(int, int)>> brickPoints = bricks.Select(brick =>
            {
                HashSet<(int, int)> set = [];
                for (int x = brick.X1; x <= brick.X2; x++)
                    for (int y = brick.Y1; y <= brick.Y2; y++)
                        set.Add((x, y));
                return set;
            }).ToList();
            bool[][] intersect = new bool[bricks.Count][];
            for (int i = 0; i < bricks.Count; i++)
            {
                intersect[i] = new bool[bricks.Count];
                for (int j = 0; j < bricks.Count; j++)
                    intersect[i][j] = brickPoints[i].Intersect(brickPoints[j]).Any();
            }

            for (int i = 0; i < bricks.Count; i++)
            {
                int minZ = 1;
                for (int j = 0; j < i; j++)
                    if (intersect[i][j])
                        minZ = Math.Max(minZ, bricks[j].Z2 + 1);
                var (x1, y1, z1, x2, y2, z2) = bricks[i];
                bricks[i] = new(x1, y1, minZ, x2, y2, minZ - z1 + z2);
            }
            int count = 0;
            for (int toRemove = 0; toRemove < bricks.Count; toRemove++)
            {
                HashSet<int> removed = [toRemove];
                HashSet<int> zs = [bricks[toRemove].Z2];
                for (int i = toRemove; i < bricks.Count; i++)
                {
                    if (removed.Contains(i) || !zs.Contains(bricks[i].Z1 - 1))
                        continue;
                    int minZ = 1;
                    for (int j = 0; j < i; j++)
                    {
                        if (removed.Contains(j))
                            continue;
                        if (intersect[i][j])
                            minZ = Math.Max(minZ, bricks[j].Z2 + 1);
                    }
                    if (minZ != bricks[i].Z1)
                    {
                        removed.Add(i);
                        zs.Add(bricks[i].Z2);
                    }
                }
                count += removed.Count - 1;
            }
            Console.WriteLine(count);
        }
    }
}
