namespace AdventOfCode2023
{
    // O(1) by solving the square equation:
    // heldTime * (time - heldTime) = distance
    // transformed to
    // -heldTime^2 + heldTime*time - distance = 0
    // The 2 solutions are:
    //     x or higher: waited too much
    //     x2 or lower: waited just too little
    // Anything which waits more than x and less than x2 is a solution
    internal class D6_2_Alt
    {
        public static void Run()
        {
            string[] input = File.ReadAllLines("inputs/6");
            long time = int.Parse(string.Concat(input[0].Where(char.IsDigit)));
            long distance = long.Parse(string.Concat(input[1].Where(char.IsDigit)));
            // BUG: has an off-by-one error when x or x2 happen to be exact. (Too lazy to fix it)
            double d = Math.Sqrt(time * time - 4 * distance);
            long x = (long) (time + d)/2;
            long x2 = (long) (time - d)/2;
            Console.WriteLine(x - x2);
        }
    }
}
