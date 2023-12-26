namespace AdventOfCode2023
{
    // Solution relies on how in the data, going in one direction leads to no obstacles
    //   and also that thare are no obstacles at the outermost points
    // Approach: divide the grid in the following parts:
    // In n-1 steps, we can reach the corner of all four diagonally adjacent grids
    // From there, each one independently forms a triangle of grids, whose count can be calculated with math
    // The triangles ends up unfinished so we need to also count those partial grids
    // Simultaneously, we go straight up, down, left and right to form four lines
    // They similarly have up to two not fully visited grids
    // Finally we add the centre square
    // We count the tiles in partially accessible grids with breadth-first search in `numReachable`
    // We also note that within one grid, tiles can be painted like a chessboard, depending on the number
    //   of moves we can either only access "white" tiles, or "black" ones.
    // Moving to a neighboring grid flips the white and black tiles, so we also need to keep track of
    //   which grid has the same tile arrangement as in the middle one (`tilesInBase`) vs not (`tilesInAdjacent`)
    internal class D21_2
    {
        public static void Run()
        {
            List<(int, int)> directions = [(-1, 0), (1, 0), (0, -1), (0, 1)];
            var grid = File.ReadAllLines("inputs/21")
                .Select(x => x.Replace('S', '.').ToCharArray()).ToArray();
            int steps = 26501365;

            int numReachable((int, int) start, int moves, bool isEven)
            {
                HashSet<(int, int)> visited = [start];
                List<(int, int)> last = [start];
                for (int i = 0; i < moves; i++)
                {
                    List<(int, int)> next = [];
                    foreach (var (y, x) in last)
                        foreach (var (dy, dx) in directions)
                            if (!visited.Contains((y + dy, x + dx)) &&
                                Helpers.SafeGet(grid, y + dy, x + dx) == '.')
                            {
                                next.Add((y + dy, x + dx));
                            }
                    visited = visited.Concat(next).ToHashSet();
                    last = next.Distinct().ToList();
                }
                return visited.Count(xy => (xy.Item1 + xy.Item2) % 2 == 0 == isEven);
            }

            int n = grid.Length; // n = m in our dataset
            int stepsFromCorners = steps - n - 1;
            int lengthsFromCorners = stepsFromCorners / n - 1;

            long triangles(long n) => (n + 1) * n / 2;
            long evenSquaresPerCorner = triangles(lengthsFromCorners / 2) * 2;
            long oddSquaresPerCorner = triangles(lengthsFromCorners) - evenSquaresPerCorner;

            int tilesInBase = numReachable((0, 0), n * 3, steps % 2 == 0);
            int tilesInAdjacent = numReachable((0, 0), n * 3, steps % 2 != 0);
            long res = 4 * (evenSquaresPerCorner * tilesInAdjacent + oddSquaresPerCorner * tilesInBase);

            bool lastFullTriangleIsEven = lengthsFromCorners % 2 == 0;
            int remainingMoves = stepsFromCorners % n + n;
            List<(int, int)> corners = [(0, 0), (0, n - 1), (n - 1, 0), (n - 1, n - 1)];
            foreach (var point in corners)
            {
                res += (lengthsFromCorners + 1) * numReachable(point, remainingMoves, !lastFullTriangleIsEven);
                res += (lengthsFromCorners + 2) * numReachable(point, remainingMoves - n, lastFullTriangleIsEven);
            }

            int stepsFromSides = steps - (n / 2) - 1;
            long numSidesPerDirection = stepsFromSides / n - 1;
            res += 4 * (long)(Math.Ceiling(numSidesPerDirection / 2.0) * tilesInAdjacent);
            res += 4 * (long)(Math.Floor(numSidesPerDirection / 2.0) * tilesInBase);

            int remainingSideMoves = stepsFromSides % n + n;
            bool lastSideIsEven = !(numSidesPerDirection % 2 == 0);
            List<(int, int)> sides = [(0, n / 2), (n - 1, n / 2), (n / 2, 0), (n / 2, n - 1)];
            foreach (var point in sides)
            {
                res += numReachable(point, remainingSideMoves, !lastSideIsEven);
                res += numReachable(point, remainingSideMoves - n, lastSideIsEven);
            }
            res += tilesInBase;
            Console.WriteLine(res);
        }
    }
}
