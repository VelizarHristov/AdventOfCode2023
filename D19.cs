namespace AdventOfCode2023
{
    internal class D19
    {
        interface IDestination;
        class Accept : IDestination;
        class Reject : IDestination;
        class Workflow(List<Rule> rules, IDestination finalCase) : IDestination
        {
            public List<Rule> Rules { get; set; } = rules;
            public IDestination FinalCase { get; set; } = finalCase;
        }
        record Rule(char Key, bool MoreThan, int CompareWith, IDestination Dest);
        public static void Run()
        {
            Dictionary<string, Workflow> workflows = [];
            var input = File.ReadAllLines("inputs/19");
            foreach (var line in input.TakeWhile(x => x.Length > 0))
            {
                string name = string.Concat(line.TakeWhile(c => c != '{'));
                workflows[name] = new([], new Accept());
            }
            IDestination GetDest(string str) => str switch
            {
                "A" => new Accept(),
                "R" => new Reject(),
                string name => workflows[name],
            };
            foreach (var line in input.Take(workflows.Count))
            {
                string name = string.Concat(line.TakeWhile(c => c != '{'));
                var parts = line[(name.Length + 1)..].Split(',');
                Workflow cur = workflows[name];
                cur.Rules = parts.SkipLast(1).Select(part => new Rule(
                    part[0],
                    part[1] == '>',
                    int.Parse(string.Concat(part[2..].TakeWhile(char.IsDigit))),
                    GetDest(part.Split(':')[1]))
                ).ToList();
                cur.FinalCase = GetDest(parts.Last()[..^1]);
            }
            int res = input.Skip(workflows.Count + 1).Sum(line =>
            {
                Dictionary<char, int> part = [];
                foreach (string assignment in line[1..^1].Split(','))
                    part[assignment[0]] = int.Parse(assignment[2..]);
                Workflow next = workflows["in"];
                while (true)
                {
                    var res = next.Rules.FirstOrDefault(rule =>
                    {
                        var (key, moreThan, compareWith, _) = rule;
                        return moreThan ? part[key] > compareWith : part[key] < compareWith;
                    });
                    var dest = res == null ? next.FinalCase : res.Dest;
                    if (dest is Workflow workflow)
                        next = workflow;
                    else if (dest is Accept)
                        return part.Values.Sum();
                    else
                        return 0;
                }
            });
            Console.WriteLine(res);
        }
    }
}
