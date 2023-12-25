namespace AdventOfCode2023
{
    internal class D22_2
    {
        static bool Insersect(((int, int), (int, int)) line1, ((int, int), (int, int)) line2)
        {
            var ((x1, y1), (x2, y2)) = line1;
            var ((x3, y3), (x4, y4)) = line2;
            HashSet<(int, int)> set1 = [];
            for (int x = x1; x <= x2; x++)
                for (int y = y1; y <= y2; y++)
                    set1.Add((x, y));
            HashSet<(int, int)> set2 = [];
            for (int x = x3; x <= x4; x++)
                for (int y = y3; y <= y4; y++)
                    set2.Add((x, y));
            return set1.Intersect(set2).Count() > 0;
        }

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
            for (int i = 0; i < bricks.Count; i++)
            {
                var (x1, y1, z1, x2, y2, z2) = bricks[i];
                int minZ = 1;
                for (int j = 0; j < i; j++)
                {
                    var (x3, y3, z3, x4, y4, z4) = bricks[j];
                    if (Insersect(((x1, y1), (x2, y2)), ((x3, y3), (x4, y4))))
                    {
                        minZ = Math.Max(minZ, z4 + 1);
                    }
                }
                bricks[i] = new(x1, y1, minZ, x2, y2, minZ - z1 + z2);
            }
            bricks = bricks.OrderBy(b => b.Z1).ToList();
            int count = 0;
            for (int toRemove = 0; toRemove < bricks.Count; toRemove++)
            {
                HashSet<int> removed = [toRemove];
                HashSet<int> zs = [bricks[toRemove].Z2];
                for (int i = toRemove; i < bricks.Count; i++)
                {
                    if (removed.Contains(i))
                        continue;
                    var (x1, y1, z1, x2, y2, z2) = bricks[i];
                    if (!zs.Contains(z1 - 1))
                        continue;
                    int minZ = 1;
                    for (int j = 0; j < i; j++)
                    {
                        if (removed.Contains(j))
                            continue;
                        var (x3, y3, z3, x4, y4, z4) = bricks[j];
                        if (Insersect(((x1, y1), (x2, y2)), ((x3, y3), (x4, y4))))
                        {
                            minZ = Math.Max(minZ, z4 + 1);
                        }
                    }
                    if (minZ != z1)
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
