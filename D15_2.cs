namespace AdventOfCode2023
{
    internal class D15_2
    {
        public static void Run()
        {
            var boxes = new List<(string, int)>[256];
            for (int i = 0; i < 256; i++)
                boxes[i] = [];
            foreach (var segment in File.ReadLines("inputs/15").First().Split(","))
            {
                string key = string.Concat(segment.TakeWhile(c => c != '=' && c != '-'));
                int box = 0;
                foreach (char c in key)
                    box = (box + c) * 17 % 256;
                if (segment.Contains('='))
                {
                    int value = int.Parse(segment[(key.Length + 1)..]);
                    if (boxes[box].Any(el => el.Item1 == key))
                        boxes[box] = boxes[box].Select(el => el.Item1 == key ? (key, value) : el).ToList();
                    else
                        boxes[box].Add((key, value));
                }
                else
                    boxes[box].RemoveAll(el => el.Item1 == key);
            };
            int sum = boxes.Select((box, boxIdx) =>
                box.Select((item, itemIdx) =>
                    item.Item2 * (itemIdx + 1)
                ).Sum() * (boxIdx + 1)
            ).Sum();
            Console.WriteLine(sum);
        }
    }
}
