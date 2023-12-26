namespace AdventOfCode2023
{
    internal class D24_2
    {
        // Relies on using an equation solver (not included)
        public static void Run()
        {
            List<string> answer = [];
            int i = 1;
            // luckily, lines 1-3 are enough (that was not mathematically guaranteed)
            foreach (var line in File.ReadLines("inputs/24").Take(3))
            {
                var parts = line.Split(" @ ");
                var pos = parts[0].Split(", ").Select(double.Parse).ToList();
                var vel = parts[1].Split(", ").Select(int.Parse).ToList();
                // d_1, d_2, d_3 stand for dx, dy, dz relative to time
                answer.Add($"x + d_1 * t_{i} = {pos[0]} {(vel[0] < 0 ? "-" : "+")} {Math.Abs(vel[0])} * t_{i}");
                answer.Add($"y + d_2 * t_{i} = {pos[1]} {(vel[1] < 0 ? "-" : "+")} {Math.Abs(vel[1])} * t_{i}");
                answer.Add($"z + d_3 * t_{i} = {pos[2]} {(vel[2] < 0 ? "-" : "+")} {Math.Abs(vel[2])} * t_{i}");
                i++;
            }
            Console.WriteLine("Enter the printed equation in a solver. The problem's answer is x + y + z.");
            Console.WriteLine(string.Join("\n", answer));
        }
    }
}
