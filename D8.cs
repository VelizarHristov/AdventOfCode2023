namespace AdventOfCode2023
{
    internal class D8
    {
        public static void Run()
        {
            var input = File.ReadAllLines("inputs/8");
            var instructions = input[0];
            var graph = input.Skip(2).Select(line =>
            {
                var start = string.Concat(line.Take(3));
                var left = string.Concat(line.Skip(7).Take(3));
                var right = string.Concat(line.Skip(12).Take(3));
                return (start, (left, right));
            }).ToDictionary(x => x.start, x => x.Item2);
            var currentNode = "AAA";
            var numSteps = 0;
            while (currentNode != "ZZZ")
            {
                foreach (var step in instructions)
                {
                    if (step == 'L')
                        currentNode = graph[currentNode].left;
                    else
                        currentNode = graph[currentNode].right;
                }
                numSteps += instructions.Length;
            }
            Console.WriteLine(numSteps);
        }
    }
}
