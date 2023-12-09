namespace AdventOfCode2023
{
    internal class D9_2
    {
        public static void Run()
        {
            int sum = File.ReadLines("inputs/9").Select(line =>
            {
                var nums = line.Split(" ").Select(int.Parse).ToList();
                List<List<int>> allNums = [nums];
                while (!nums.All(n => n == 0))
                {
                    nums = nums.Zip(nums.Skip(1)).Select(ab => ab.Second - ab.First).ToList();
                    allNums = allNums.Prepend(nums).ToList();
                }
                var diff = 0;
                foreach (var num in allNums.Select(ls => ls.First()))
                    diff = num - diff;
                return diff;
            }).Sum();
            Console.WriteLine(sum);
        }
    }
}
