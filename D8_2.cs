namespace AdventOfCode2023
{
    internal class D8_2
    {
        // Gcd and Lcm copied from StackOverflow
        static long Gcf(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
        static long Lcm(long a, long b)
        {
            return a / Gcf(a, b) * b;
        }

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
            var currentNodes = graph.Keys.Where(k => k.EndsWith('A')).ToArray();
            var numSteps = new long[currentNodes.Length];
            for (int i = 0; i < currentNodes.Length; i++)
            {
                while (!currentNodes[i].EndsWith('Z')) 
                {
                    foreach (var step in instructions)
                    {
                        if (step == 'L')
                            currentNodes[i] = graph[currentNodes[i]].left;
                        else
                            currentNodes[i] = graph[currentNodes[i]].right;
                    }
                    numSteps[i] += instructions.Length;
                }
            }
            Console.WriteLine(numSteps.Aggregate(Lcm));
        }
    }
}
