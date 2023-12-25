namespace AdventOfCode2023
{
    internal class D24
    {
        // Taken from StackOverflow
        static bool CCW(double x1, double y1, double x2, double y2, double x3, double y3) =>
            (x3 - x1) * (y2 - y1) > (x2 - x1) * (y3 - y1);
        static bool Insersect(((double, double), (double, double)) line1, ((double, double), (double, double)) line2)
        {
            var ((x1, y1), (x2, y2)) = line1;
            var ((x3, y3), (x4, y4)) = line2;
            return CCW(x1, y1, x3, y3, x4, y4) != CCW(x2, y2, x3, y3, x4, y4) &&
                CCW(x1, y1, x2, y2, x3, y3) != CCW(x1, y1, x2, y2, x4, y4);
        }

        readonly static string input_filename = "inputs/24";
        readonly static double BOUNDARY_START = input_filename == "inputs/24" ? 200000000000000.0 : 7.0;
        readonly static double BOUNDARY_END = input_filename == "inputs/24" ? 400000000000000.0 : 27.0;

        static (double, double)? NextPoint(double x, double y, int dx, int dy)
        {
            var a = (BOUNDARY_END - x) / dx;
            var b = (BOUNDARY_START - x) / dx;
            var c = (BOUNDARY_END - y) / dy;
            var d = (BOUNDARY_START - y) / dy;
            double candidate = double.MaxValue;
            if (a > 0) candidate = Math.Min(candidate, a);
            if (b > 0) candidate = Math.Min(candidate, b);
            if (c > 0) candidate = Math.Min(candidate, c);
            if (d > 0) candidate = Math.Min(candidate, d);
            if (candidate == double.MaxValue)
                return null;
            else
                return (x + dx * candidate, y + dy * candidate);
        }

        static (double, double)? StartPoint(double x, double y, int dx, int dy)
        {
            while (Math.Min(x, y) < BOUNDARY_START || Math.Max(x, y) > BOUNDARY_END)
            {
                if (NextPoint(x, y, dx, dy) is (double, double) xy)
                    (x, y) = xy;
                else
                    return null;
            }
            return (x, y);
        }

        public static void Run()
        {
            List<((double, double), (double, double))> lines = [];
            foreach (var line in File.ReadLines(input_filename))
            {
                var parts = line.Split(" @ ");
                var pos = parts[0].Split(", ").Select(double.Parse).ToList();
                var vel = parts[1].Split(", ").Select(int.Parse).ToList();

                if (StartPoint(pos[0], pos[1], vel[0], vel[1]) is (double, double) point1)
                    if (NextPoint(point1.Item1, point1.Item2, vel[0], vel[1]) is (double, double) point2)
                        lines.Add((point1, point2));
            }

            int intersections = Enumerable.Range(0, lines.Count).Sum(i =>
                lines.Skip(i + 1).Count(p2 =>
                    Insersect(lines[i], p2)));
            Console.WriteLine(intersections);
        }
    }
}
