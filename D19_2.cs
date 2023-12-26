namespace AdventOfCode2023
{
    internal class D19_2
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
            long calcCombinations(Workflow curWorkflow, Dictionary<char, Range> ratings)
            {
                long sum = 0;
                foreach (var (key, moreThan, compareWith, dest) in curWorkflow.Rules)
                {
                    Range currentRating = ratings[key];
                    Range passingRating;
                    if (moreThan)
                    {
                        passingRating = (compareWith + 1)..currentRating.End;
                        currentRating = currentRating.Start..(compareWith + 1);
                    }
                    else
                    {
                        passingRating = currentRating.Start..compareWith;
                        currentRating = compareWith..currentRating.End;
                    }
                    var passingRatings = ratings.ToDictionary();
                    passingRatings[key] = passingRating;
                    if (dest is Workflow branchWorkflow)
                        sum += calcCombinations(branchWorkflow, passingRatings);
                    else if (dest is Accept)
                        sum += passingRatings.Values.Select(x => (long)x.End.Value - x.Start.Value).Aggregate((x, y) => x * y);
                    ratings[key] = currentRating;
                }
                if (curWorkflow.FinalCase is Workflow workflow)
                    sum += calcCombinations(workflow, ratings);
                else if (curWorkflow.FinalCase is Accept)
                    sum += ratings.Values.Select(x => (long)x.End.Value - x.Start.Value).Aggregate((x, y) => x * y);
                return sum;
            }
            long res = calcCombinations(workflows["in"], new()
            {
                { 'x', 1..4001 },
                { 'm', 1..4001 },
                { 'a', 1..4001 },
                { 's', 1..4001 }
            });
            Console.WriteLine(res);
        }
    }
}
