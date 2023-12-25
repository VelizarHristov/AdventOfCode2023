using System.Collections.Immutable;

namespace AdventOfCode2023
{
    internal class D23_2
    {
        static readonly List<(int, int)> DIRS = [(-1, 0), (0, 1), (1, 0), (0, -1)];
        static readonly Dictionary<(int, int), HashSet<(int, int)>> edges = [];
        static readonly Dictionary<((int, int), (int, int)), int> lengths = [];
        static (int, int) finalPos = (0, 0);
        static void AddEdge(int y1, int x1, int y2, int x2, int len)
        {
            if (edges.TryGetValue((y1, x1), out var set))
                set.Add((y2, x2));
            else
                edges[(y1, x1)] = [(y2, x2)];
            lengths[((y1, x1), (y2, x2))] = len;
        }
        // assigns edges, lengths, finalPos
        static void calcGraph()
        {
            char[][] grid = File.ReadAllLines("inputs/23").Select(line => line.ToCharArray()).ToArray();
            finalPos = (grid.Length - 1, grid[0].Length - 2);
            List<(int, int, int, int)> nextEdges = [(0, 1, 1, 1)];
            HashSet<(int, int)> visitedEdges = [];
            while (nextEdges.Count != 0)
            {
                var (startY, startX, y, x) = nextEdges[0];
                nextEdges.RemoveAt(0);
                if (visitedEdges.Contains((y, x)))
                    continue;
                visitedEdges.Add((y, x));
                int len = 1;
                HashSet<(int, int)> visitedTiles = [(startY, startX)];
                while (true)
                {
                    if (y == grid.Length - 1 && x == grid[0].Length - 2)
                    {
                        AddEdge(startY, startX, grid.Length - 1, grid[0].Length - 2, len);
                        break;
                    }
                    var next = DIRS.Select(dydx => (y + dydx.Item1, x + dydx.Item2))
                        .Where(yx =>
                        {
                            var (nextY, nextX) = yx;
                            var nextTile = Helpers.SafeGet(grid, nextY, nextX);
                            return !visitedTiles.Contains(yx) &&
                                nextTile != '#' && nextTile != default;
                        });
                    if (next.Count() == 0)
                        break;
                    else if (next.Count() > 1)
                    {
                        AddEdge(startY, startX, y, x, len);
                        AddEdge(y, x, startY, startX, len);
                        foreach (var (branchY, branchX) in next)
                            nextEdges.Add((y, x, branchY, branchX));
                        break;
                    }
                    else
                    {
                        len++;
                        visitedTiles.Add((y, x));
                        (y, x) = next.First();
                    }
                }
            }
        }

        record State((int, int) Pos, int Len, ImmutableHashSet<(int, int)> Visited);
        public static void Run()
        {
            calcGraph();
            Stack<State> states = [];
            states.Push(new((0, 1), 0, [(0, 1)]));
            int bestSolutionLen = 0;
            while (states.Count != 0)
            {
                var (node, len, visited) = states.Pop();
                if (node == finalPos)
                    bestSolutionLen = Math.Max(bestSolutionLen, len);
                else
                    foreach (var next in edges[node])
                        if (!visited.Contains(next))
                            states.Push(new(next, len + lengths[(node, next)], visited.Add(next)));
            }
            Console.WriteLine(bestSolutionLen);
        }
    }
}
