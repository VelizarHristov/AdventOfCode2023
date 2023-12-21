namespace AdventOfCode2023
{
    internal class D20
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
            long lowPulsesSent = 0;
            long highPulsesSent = 0;
            void pressButton()
            {
                lowPulsesSent++; // button -> broadcaster
                List<(string, string, bool)> nextSignals = broadcastOutputs.Select(
                    recv => (recv, "broadcaster", false)).ToList();
                while (nextSignals.Count != 0)
                {
                    var (nextSignal, sender, isHigh) = nextSignals[0];
                    if (isHigh)
                        highPulsesSent++;
                    else
                        lowPulsesSent++;
                    nextSignals.RemoveAt(0);
                    if (modules.TryGetValue(nextSignal, out var module))
                        if (module is FlipFlop flipFlop && !isHigh)
                        {
                            flipFlop.IsOn = !flipFlop.IsOn;
                            modules[nextSignal] = flipFlop;
                            foreach (string output in flipFlop.Outputs)
                                nextSignals.Add((output, nextSignal, flipFlop.IsOn));
                        }
                        else if (module is Conjunction conj)
                        {
                            conj.ReceivedHighFrom[sender] = isHigh;
                            bool sendHigh = conj.ReceivedHighFrom.Values.Any(b => !b);
                            foreach (string output in conj.Outputs)
                                nextSignals.Add((output, nextSignal, sendHigh));
                        }
                }
            }
            for (int i = 0; i < 1000; i++)
                pressButton();
            Console.WriteLine(lowPulsesSent * highPulsesSent);
        }
    }
}
