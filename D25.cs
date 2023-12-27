namespace AdventOfCode2023
{
    internal class D25
    {
        public static void Run()
        {
            var input = File.ReadAllLines("inputs/25");
            var symbols = input.SelectMany(line =>
                line.Replace(":", "").Split(" ")).Distinct().Order().ToArray();
            Dictionary<string, int> symToInt = [];
            for (int i = 0; i < symbols.Length; i++)
                symToInt[symbols[i]] = i;

            Dictionary<int, HashSet<int>> edges = [];
            List<(int, int)> allEdges = [];

            foreach (var line in input)
            {
                var parts = line.Split(": ");
                int origin = symToInt[parts[0]];
                foreach (var item in parts[1].Split(' '))
                {
                    int itemInt = symToInt[item];
                    if (!edges.ContainsKey(origin))
                        edges[origin] = [];
                    if (!edges.ContainsKey(itemInt))
                        edges[itemInt] = [];
                    edges[origin].Add(itemInt);
                    edges[itemInt].Add(origin);
                    allEdges.Add((origin, itemInt));
                }
            }

            int n = symToInt.Values.Max() + 1;
            int[] parent = new int[n * 2];
            int getParent(int i)
            {
                if (parent[i] == i)
                    return i;
                else
                {
                    int topAncestor = getParent(parent[i]);
                    parent[i] = topAncestor;
                    return topAncestor;
                }
            }
            Random r = new();

            // Karger's algorithm
            var edgesInOrder = new List<(int, int)>(allEdges).ToArray();
            while (true)
            {
                for (int i = 0; i < n * 2; i++)
                    parent[i] = i;
                int lastParent = n;
                r.Shuffle(edgesInOrder);

                int j = 0;
                while (lastParent < 2 * n - 2)
                {
                    var (from, to) = edgesInOrder[j];
                    j++;
                    if (getParent(from) == getParent(to))
                        continue;
                    parent[getParent(from)] = lastParent;
                    parent[getParent(to)] = lastParent;
                    lastParent++;
                }
                int edgesCount = 0;
                foreach (var (from, to) in edgesInOrder)
                    if (parent[getParent(from)] != parent[getParent(to)])
                        edgesCount++;
                if (edgesCount == 3)
                {
                    int n1 = Enumerable.Range(0, n).Count(x => getParent(x) == getParent(0));
                    int n2 = n - n1;
                    int res = n1 * n2;
                    Console.WriteLine(res);
                    return;
                }
            }
        }
    }
}
