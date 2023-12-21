namespace AdventOfCode2023
{
    // This solution heavily relies on the shape of the input
    // Does not solve the fully general problem
    internal class D20_2
    {
        interface IModule
        {
            string[] GetOutputs();
        };
        record struct FlipFlop(bool IsOn, string[] Outputs) : IModule
        {
            public readonly string[] GetOutputs() => Outputs;
        };
        record struct Conjunction(Dictionary<string, bool> ReceivedHighFrom, string[] Outputs) : IModule
        {
            public readonly string[] GetOutputs() => Outputs;
        };

        public static void Run()
        {
            string[] broadcastOutputs = [];
            Dictionary<string, IModule> modules = [];
            foreach (var line in File.ReadLines("inputs/20"))
            {
                var parts = line.Split(" -> ");
                var descriptor = parts[0];
                var outputs = parts[1].Split(", ");
                if (descriptor == "broadcaster")
                    broadcastOutputs = outputs;
                else if (descriptor[0] == '%')
                    modules[descriptor[1..]] = new FlipFlop(false, outputs);
                else
                    modules[descriptor[1..]] = new Conjunction([], outputs);
            }
            foreach (var (senderName, sender) in modules)
                foreach (var signal in sender.GetOutputs())
                    if (modules.TryGetValue(signal, out IModule? recv))
                        if (recv is Conjunction conj)
                            conj.ReceivedHighFrom[senderName] = false;

            long pressesPreActivation = 1;
            List<(string, string)> btnOutputs = broadcastOutputs.Select(x => ("broadcaster", x)).ToList();
            Dictionary<string, Dictionary<string, long?>> conjActivations = [];
            foreach (var (moduleName, module) in modules)
                if (module is Conjunction conj)
                {
                    conjActivations[moduleName] = [];
                    foreach (string k in conj.ReceivedHighFrom.Keys)
                        conjActivations[moduleName][k] = null;
                }

            long? res = null;
            while (res == null)
            {
                List<(string, string)> newOutputs = [];
                foreach (var (sender, output) in btnOutputs)
                {
                    if (modules[output] is FlipFlop flipFlop)
                        newOutputs = newOutputs.Concat(flipFlop.Outputs.Select(x => (output, x))).ToList();
                    else if (modules[output] is Conjunction conj)
                    {
                        if (conjActivations[output][sender] != null)
                            throw new Exception("conjActivations values need to be a list");
                        conjActivations[output][sender] = pressesPreActivation / 2;
                    }
                }
                btnOutputs = newOutputs;
                pressesPreActivation *= 2;
                foreach (var (key, signals) in conjActivations)
                    if (signals.All(x => x.Value.HasValue))
                    {
                        // This only works because of the specific data
                        res ??= 1;
                        res *= signals.Values.Sum();
                    }
            }
            Console.WriteLine(res);
        }
    }
}
