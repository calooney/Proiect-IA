using Proiect_IA_Program.GraphUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proiect_IA_Program
{
    public class Inference
    {
        private Graph graph;

        internal Graph Graph { get => graph; set => graph = value; }

        public Inference(Graph _graph)
        {
            this.Graph = _graph;
        }

        public List<double> Normalization(List<double> Q)
        {
            return Q.Select(d => d / Q.Sum()).ToList();
        }

        public List<double> EnumerationAsk(int NodeId)
        {
            Node node = graph.Nodes[NodeId];
            List<double> Q = new List<double>();
            if (node.Observation != Node.NO_OBSERVATION)
            {
                for (int domainIndex = 0; domainIndex < node.Domain.Count; domainIndex++) { 
                    if (domainIndex != node.Observation)
                        Q.Add(0);
                    else
                        Q.Add(1);
                }
            }   
            else 
            { 
                for (int domainIndex = 0; domainIndex < node.Domain.Count; domainIndex++)
                {
                    Queue<int> vars = new Queue<int>();
                    foreach (int index in graph.TopologicSort())
                        vars.Enqueue(index);
                    Queue<int> varsCopy = new Queue<int>(vars.ToArray());

                    node.Observation = domainIndex;
                
                    double result = EnumerateAll(varsCopy);
                    Q.Add(result);

                    node.Observation = Node.NO_OBSERVATION;
                }
            }
            return Normalization(Q);
        }

        private double EnumerateAll(Queue<int> vars)
        {
            if (vars.Count == 0)
                return 1.0;

            int Yindex = vars.Dequeue();

            if (graph.Nodes[Yindex].Observation != Node.NO_OBSERVATION)
            {
                List<double> table = new List<double>();
                List<int> obsParent = new List<int>();
                
                if (graph.Nodes[Yindex].Parents.Count == 0)
                {
                    table = graph.Nodes[Yindex].Table[0];
                }
                else
                {
                    foreach (int parentIndex in graph.Nodes[Yindex].Parents)
                    {
                        Node parent = graph.Nodes[parentIndex];
                        obsParent.Add(parent.Observation);
                    }
                    table = graph.GetProbabilityByParentsObservation(graph.Nodes[Yindex], obsParent);
                }

                double Pyi = table[graph.Nodes[Yindex].Observation];

                Queue<int> varsCopy = new Queue<int>(vars.ToArray());

                return (Pyi * EnumerateAll(varsCopy));
            }
            else
            {
                List<double> table = new List<double>();
                List<int> obsParent = new List<int>();
                
                if (graph.Nodes[Yindex].Parents.Count == 0)
                {
                    table = graph.Nodes[Yindex].Table[0];
                }
                else
                {
                    foreach (int parentIndex in graph.Nodes[Yindex].Parents)
                    {
                        Node parent = graph.Nodes[parentIndex];
                        obsParent.Add(parent.Observation);
                    }
                    table = graph.GetProbabilityByParentsObservation(graph.Nodes[Yindex], obsParent);
                }

                double sum = 0;
                for (int i = 0; i < table.Count; i++)
                {
                    double Pyi = table[i];
                    Queue<int> varsCopy = new Queue<int>(vars.ToArray());

                    graph.Nodes[Yindex].Observation = i;
                    sum += Pyi * EnumerateAll(varsCopy);
                    graph.Nodes[Yindex].Observation = Node.NO_OBSERVATION;
                }
                return sum;
            }
        }
    }
}
