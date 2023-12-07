namespace AdventOfCode2023
{
    internal class D7
    {
        readonly static Dictionary<char, string> STRENGTH_MAP = new()
        {
            { 'A', "14" },
            { 'K', "13" },
            { 'Q', "12" },
            { 'J', "11" },
            { 'T', "10" }
        };

        private static int Strengths(string cards)
        {
            return cards.Select(card => int.Parse(
                    STRENGTH_MAP.TryGetValue(card, out string? res) ? res : card + "")
            ).Aggregate((prev, cur) => prev * 20 + cur);
        }
        private static int Points(string cards)
        {
            var counts = cards.Distinct().Select(c1 => cards.Count(c2 => c1 == c2)).OrderDescending();
            int maxCount = counts.First();
            int points = maxCount;
            if (maxCount <= 3 && counts.Skip(1).First() == 2)
                points++;
            else if (maxCount >= 4)
                points += 2;
            if (maxCount == 3)
                points += 1;
            return points;
        }

        public static void Run()
        {
            var sum = File.ReadLines("inputs/7").Select(line =>
            {
                var split = line.Split(" ");
                var cards = split.First();
                var number = int.Parse(split.Last());
                return (cards, number);
            }).OrderBy(hand => Points(hand.cards))
            .ThenBy(hand => Strengths(hand.cards))
            .Select((hand, idx) => hand.number * (idx + 1)).Sum();
            Console.WriteLine(sum);
        }
    }
}
