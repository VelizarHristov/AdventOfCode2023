using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    internal class D6
    {
        public static void Run()
        {
            string[] input = File.ReadAllLines("inputs/6");
            var times = Regex.Matches(input[0], "\\d+").Select(x => int.Parse(x.Value)).ToArray();
            var distances = Regex.Matches(input[1], "\\d+").Select(x => int.Parse(x.Value)).ToArray();

            int product = Enumerable.Range(0, times.Length).Select(idx =>
            {
                return Enumerable.Range(1, times[idx]).Count(heldTime =>
                {
                    return heldTime * (times[idx] - heldTime) > distances[idx];
                });
            }).Aggregate((prod, next) => prod * next);
            Console.WriteLine(product);
        }
    }
}
