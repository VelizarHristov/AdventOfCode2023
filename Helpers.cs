namespace AdventOfCode2023
{
    internal class Helpers
    {
        public static A? SafeGet<A>(A[][] ls, int y, int x)
        {
            try
            {
                return ls[y][x];
            }
            catch (IndexOutOfRangeException)
            {
                return default;
            }
        }
    }
}
