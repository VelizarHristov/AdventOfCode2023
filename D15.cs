namespace AdventOfCode2023
{
    internal class D15
    {
        public static void Run()
        {
            int sum = File.ReadLines("inputs/15").First().Split(",").Sum(segment =>
            {
                int hash = 0;
                foreach (char c in segment)
                    hash = (hash + c) * 17 % 256;
                return hash;
            });
            Console.WriteLine(sum);
        }
    }
}
