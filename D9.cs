namespace AdventOfCode2023
{
    internal class D9
    {
        public static void Run()
        {
            int sum = File.ReadLines("inputs/9").Sum(line =>
            {
                var nums = line.Split(" ").Select(int.Parse).ToList();
                List<List<int>> allNums = [nums];
                while (!nums.All(n => n == 0))
                {
                    nums = nums.Zip(nums.Skip(1)).Select(ab => ab.Second - ab.First).ToList();
                    allNums = allNums.Prepend(nums).ToList();
                }
                return allNums.Sum(ls => ls.Last());
            });
            Console.WriteLine(sum);
        }
    }
}
