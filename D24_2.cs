using Z3.Linq;

namespace AdventOfCode2023
{
    internal class D24_2
    {
        public static void Run()
        {
            var items = File.ReadLines("inputs/24").Take(3).Select(line =>
            {
                var parts = line.Split(" @ ");
                var pos = parts[0].Split(", ").Select(long.Parse).ToList();
                var vel = parts[1].Split(", ").Select(int.Parse).ToList();
                return (pos, vel);
            }).ToList();
            // If I inline any of these, the code crashes.
            long x1 = items[0].pos[0];
            long x2 = items[1].pos[0];
            long x3 = items[2].pos[0];
            long y1 = items[0].pos[1];
            long y2 = items[1].pos[1];
            long y3 = items[2].pos[1];
            long z1 = items[0].pos[2];
            long z2 = items[1].pos[2];
            long z3 = items[2].pos[2];
            long dx1 = items[0].vel[0];
            long dx2 = items[1].vel[0];
            long dx3 = items[2].vel[0];
            long dy1 = items[0].vel[1];
            long dy2 = items[1].vel[1];
            long dy3 = items[2].vel[1];
            long dz1 = items[0].vel[2];
            long dz2 = items[1].vel[2];
            long dz3 = items[2].vel[2];
            var theorem = from t in new Z3Context().NewTheorem<(long x, long y, long z,
                                                                long dx, long dy, long dz,
                                                                long t1, long t2, long t3)>()
                          where t.x + t.dx * t.t1 == x1 + dx1 * t.t1
                          where t.y + t.dy * t.t1 == y1 + dy1 * t.t1
                          where t.z + t.dz * t.t1 == z1 + dz1 * t.t1
                          where t.x + t.dx * t.t2 == x2 + dx2 * t.t2
                          where t.y + t.dy * t.t2 == y2 + dy2 * t.t2
                          where t.z + t.dz * t.t2 == z2 + dz2 * t.t2
                          where t.x + t.dx * t.t3 == x3 + dx3 * t.t3
                          where t.y + t.dy * t.t3 == y3 + dy3 * t.t3
                          where t.z + t.dz * t.t3 == z3 + dz3 * t.t3
                          select t;
            var (x, y, z, dx, dy, dz, t1, t2, t3) = theorem.Solve();
            long res = x + y + z;
            Console.WriteLine(res);
        }
    }
}
