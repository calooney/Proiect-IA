using System;
using System.Collections.Generic;
using System.Text;

namespace Proiect_IA_Program.GraphUtility
{
    public class Node
    {
        public const int NO_OBSERVATION = -1;

        private string name;
        private List<String> domain;
        private List<int> parents; // index parinti in Graph
        private List<List<double>> table;
        private int observation; // index din domain

        public string Name { get => name; set => name = value; }
        public List<string> Domain { get => domain; set => domain = value; }
        internal List<int> Parents { get => parents; set => parents = value; }
        public List<List<double>> Table { get => table; set => table = value; }
        public int Observation { get => observation; set => observation = value; }

        public Node(string _Name, List<string> _Domain)
        {
            this.Parents     = new List<int>();
            this.Name        = _Name;
            this.Domain      = _Domain;
            this.Table       = new List<List<double>>();
            this.Observation = Node.NO_OBSERVATION;
        }
    }
}
