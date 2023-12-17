namespace AdventOfCode2023
{
    internal class D17_2
    {
        static readonly (int, int)[] allDirs = [(1, 0), (-1, 0), (0, 1), (0, -1)];
        readonly struct State(int y, int x, int cost, int dy, int dx, int movesSoFar)
        {
            public readonly int y = y;
            public readonly int x = x;
            public readonly int cost = cost;
            public readonly int dx = dx;
            public readonly int dy = dy;
            public readonly int movesSoFar = movesSoFar;
            public List<(int, int)> NextDirs()
            {
                if (movesSoFar < 4)
                    return [(dy, dx)];
                List<(int, int)> res = [];
                foreach (var dir in allDirs)
                    if (dir != (-dy, -dx) && (movesSoFar < 10 || dir != (dy, dx)))
                        res.Add(dir);
                return res;
            }
        }

        public static void Run()
        {
            int[][] grid = File.ReadAllLines("inputs/17").Select(line => line.Select(x => int.Parse(x.ToString())).ToArray()).ToArray();
            State start1 = new(0, 0, 0, 1, 0, 0);
            State start2 = new(0, 0, 0, 0, 1, 0);
            int Heuristic(State state) => grid.Length + grid[0].Length - 2 - state.x - state.y + state.cost;
            PriorityQueue<State, int> fringe = new();
            fringe.Enqueue(start1, Heuristic(start1));
            fringe.Enqueue(start2, Heuristic(start2));
            Dictionary<(int, int, int, int, int), int> visited = new() { { (0, 0, 1, 0, 2), 0 } };
            int bestSolutionLen = int.MaxValue;
            while (Heuristic(fringe.Peek()) < bestSolutionLen)
            {
                var next = fringe.Dequeue();
                foreach (var (dy, dx) in next.NextDirs())
                {
                    int addedCost = Helpers.SafeGet(grid, next.y + dy, next.x + dx);
                    if (addedCost != default)
                    {
                        int movesSoFar = (dy == next.dy && dx == next.dx) ? (next.movesSoFar + 1) : 1;
                        State state = new(next.y + dy, next.x + dx, next.cost + addedCost, dy, dx, movesSoFar);
                        if (visited.TryGetValue((state.y, state.x, state.dy, state.dx, state.movesSoFar),
                            out int value) && value <= state.cost)
                        {
                            continue;
                        }
                        visited[(state.y, state.x, state.dy, state.dx, state.movesSoFar)] = state.cost;
                        if (state.y == grid.Length - 1 && state.x == grid[0].Length - 1)
                            bestSolutionLen = Math.Min(bestSolutionLen, state.cost);
                        else
                            fringe.Enqueue(state, Heuristic(state));
                    }
                }
            }
            Console.WriteLine(bestSolutionLen);
        }
    }
}
