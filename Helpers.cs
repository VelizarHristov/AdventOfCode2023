namespace AdventOfCode2023
{
    internal class Helpers
    {
        public static A? SafeGet<A>(A[][] ls, int x, int y)
        {
            try
            {
                return ls[x][y];
            }
            catch (IndexOutOfRangeException)
            {
                return default;
            }
        }
    }
}
