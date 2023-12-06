namespace AdventOfCode2023
{
    internal class D6_2
    {
        public static void Run()
        {
            string[] input = File.ReadAllLines("inputs/6");
            long time = int.Parse(string.Concat(input[0].Where(char.IsDigit)));
            long distance = long.Parse(string.Concat(input[1].Where(char.IsDigit)));

            int count = 0;
            for (long heldTime = 1; heldTime <= time; heldTime++)
                if (heldTime * (time - heldTime) > distance)
                    count++;
                else if (count >= 1)
                    break; // for performance
            Console.WriteLine(count);
        }
    }
}
