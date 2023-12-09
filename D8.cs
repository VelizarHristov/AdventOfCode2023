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
                var start = line[..3];
                var left = line[7..10];
                var right = line[12..15];
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
