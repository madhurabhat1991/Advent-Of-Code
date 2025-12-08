using System;
using System.Collections.Generic;
using System.Text;
using Template;

namespace _2025.Day08
{
    public class Day08 : Day<List<(int, int, int)>, long, long>
    {
        public override string DayNumber { get { return "08"; } }

        public override long PartOne(List<(int, int, int)> input)
        {
            // unique pairs and sorted distances between them
            PriorityQueue<((int, int, int), (int, int, int)), double> distances = new PriorityQueue<((int, int, int), (int, int, int)), double>();
            for (int i = 0; i < input.Count - 1; i++)
            {
                for (int j = i + 1; j < input.Count; j++)
                {
                    var p = (input[i].Item1, input[i].Item2, input[i].Item3);
                    var q = (input[j].Item1, input[j].Item2, input[j].Item3);
                    if (p != q)
                    {
                        var pair = ((input[i].Item1, input[i].Item2, input[i].Item3), (input[j].Item1, input[j].Item2, input[j].Item3));
                        var distance = Math.Sqrt((long)Math.Pow((p.Item1 - q.Item1), 2) + (long)Math.Pow((p.Item2 - q.Item2), 2) + (long)Math.Pow((p.Item3 - q.Item3), 2));
                        distances.Enqueue(pair, distance);
                    }
                }
            }

            //int connections = 10;       // example
            int connections = 1000;     // input, skip example
            List<Circuit> circuits = new List<Circuit>();
            while (connections > 0)
            {
                var pair = distances.Dequeue();
                var circuit = new Circuit();
                if (circuits.Count > 0 && circuits.Any(c => c.IsConnection(pair.Item1) || c.IsConnection(pair.Item2)))
                {
                    var existingList = circuits.Where(c => c.IsConnection(pair.Item1) || c.IsConnection(pair.Item2)).ToList();
                    circuits.RemoveAll(c => c.IsConnection(pair.Item1) || c.IsConnection(pair.Item2));
                    // merge circuits
                    foreach (var existing in existingList)
                    {
                        circuit.AddConnections(existing.Connections);
                    }
                }
                // add new connections
                circuit.AddConnection(pair.Item1);
                circuit.AddConnection(pair.Item2);
                circuits.Add(circuit);
                connections--;
            }
            return circuits.OrderByDescending(c => c.Connections.Count).ToList().Take(3).Select(c => c.Connections.Count).Aggregate((a, x) => a * x);
        }

        public override long PartTwo(List<(int, int, int)> input)
        {
            // unique pairs and sorted distances between them
            PriorityQueue<((int, int, int), (int, int, int)), double> distances = new PriorityQueue<((int, int, int), (int, int, int)), double>();
            for (int i = 0; i < input.Count - 1; i++)
            {
                for (int j = i + 1; j < input.Count; j++)
                {
                    var p = (input[i].Item1, input[i].Item2, input[i].Item3);
                    var q = (input[j].Item1, input[j].Item2, input[j].Item3);
                    if (p != q)
                    {
                        var pair = ((input[i].Item1, input[i].Item2, input[i].Item3), (input[j].Item1, input[j].Item2, input[j].Item3));
                        var distance = Math.Sqrt((long)Math.Pow((p.Item1 - q.Item1), 2) + (long)Math.Pow((p.Item2 - q.Item2), 2) + (long)Math.Pow((p.Item3 - q.Item3), 2));
                        distances.Enqueue(pair, distance);
                    }
                }
            }

            long finalDistance = 0;
            List<Circuit> circuits = new List<Circuit>();
            HashSet<(int, int, int)> remaining = new HashSet<(int, int, int)>();
            remaining.UnionWith(input);
            while (true)
            {
                var pair = distances.Dequeue();
                var circuit = new Circuit();
                if (circuits.Count > 0 && circuits.Any(c => c.IsConnection(pair.Item1) || c.IsConnection(pair.Item2)))
                {
                    var existingList = circuits.Where(c => c.IsConnection(pair.Item1) || c.IsConnection(pair.Item2)).ToList();
                    circuits.RemoveAll(c => c.IsConnection(pair.Item1) || c.IsConnection(pair.Item2));
                    // merge circuits
                    foreach (var existing in existingList)
                    {
                        circuit.AddConnections(existing.Connections);
                    }
                }
                // add new connections
                circuit.AddConnection(pair.Item1);
                circuit.AddConnection(pair.Item2);
                circuits.Add(circuit);
                // remove newly added from remaining
                remaining.Remove(pair.Item1);
                remaining.Remove(pair.Item2);
                // calculate final distance from wall if nothing is remaining and one big circuit has formed
                if (circuits.Count == 1 && remaining.Count == 0)
                {
                    finalDistance = (long)pair.Item1.Item1 * pair.Item2.Item1;
                    break;
                }
            }
            return finalDistance;
        }

        public override List<(int, int, int)> ProcessInput(string[] input)
        {
            List<(int, int, int)> positions = new List<(int, int, int)>();
            foreach (var line in input)
            {
                var splits = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
                positions.Add((Int32.Parse(splits[0]), Int32.Parse(splits[1]), Int32.Parse(splits[2])));
            }
            return positions;
        }

        public class Circuit
        {
            public HashSet<(int, int, int)> Connections;

            public Circuit()
            {
                Connections = new HashSet<(int, int, int)>();
            }

            public bool IsConnection((int, int, int) connection)
            {
                return Connections != null && Connections.Count > 0 && Connections.Contains(connection);
            }

            public void AddConnection((int, int, int) connection)
            {
                Connections.Add(connection);
            }

            public void AddConnections(HashSet<(int, int, int)> connections)
            {
                Connections.UnionWith(connections);
            }
        }
    }
}
