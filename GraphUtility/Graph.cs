using System;
using System.Collections.Generic;
using System.Text;

namespace Proiect_IA_Program.GraphUtility
{
    class Graph
    {
        private List<Node> nodes;

        internal List<Node> Nodes { get => nodes; set => nodes = value; }


        private bool[] visited;
        private List<int> order;

        public Graph()
        {
            nodes = new List<Node>();
            order = new List<int>();
        }

        public Node GetNodeByName(string name)
        {
            foreach (Node node in this.Nodes)
                if (node.Name == name) return node;
            return null;
        }

        public int GetNodeIndexByName(string name)
        {
            for (int i = 0; i < this.Nodes.Count; i++)
                if (this.Nodes[i].Name == name) return i;
            return -1;
        }

        private void DFS(int k)
        {
            this.visited[k] = true;

            foreach (int parentIndex in this.Nodes[k].Parents)
            {
                if (!this.visited[parentIndex])
                    DFS(parentIndex);
            }
            this.order.Add(k);
        }
        public List<int> TopologicSort()
        {
            this.visited = new bool[this.Nodes.Count];
            Array.Clear(this.visited, 0, this.visited.Length);
            order.Clear();

            for (int i = 0; i < this.Nodes.Count; i++)
                if (!this.visited[i])
                    DFS(i);

            return order;
        }

        public List<double> GetProbabilityByParentsObservation(Node node, List<int> obsParents)
        {
            int index = 0;
            int scaleFactor = 1;
            for (int i = node.Parents.Count - 1; i >= 0; i--)
            {
                Node parent = this.Nodes[i];
                index += obsParents[i] * scaleFactor;
                scaleFactor *= parent.Domain.Count;
            }
            return node.Table[index];
        }
    }
}
