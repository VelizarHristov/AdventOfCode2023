using System.Text.RegularExpressions;

// Alternative solution for D4 part 2 - arguably more elegant
// Does less work, but harder to read
namespace AdventOfCode2023
{
    internal class D4_2_Alt
    {
        public static void Run()
        {
            string[] lines = File.ReadAllLines("inputs/4");
            int cardsSum = 0;
            int extraCards = 0;
            int[] extraCardsExpiry = new int[lines.Length + 10];
            for (int curNum = 0; curNum < lines.Length; curNum++)
            {
                var res = string.Concat(lines[curNum].Skip(9)).Split("|");
                var winningNums = Regex.Matches(res[0], "\\d+").Select(x => x.Value).ToHashSet();
                int count = Regex.Matches(res[1], "\\d+").Count(x => winningNums.Contains(x.Value));

                int curCards = 1 + extraCards;
                cardsSum += curCards;
                if (count != 0)
                {
                    extraCards += curCards;
                    extraCardsExpiry[curNum + count] += curCards;
                }
                extraCards -= extraCardsExpiry[curNum];
            }
            Console.WriteLine(cardsSum);
        }
    }
}
