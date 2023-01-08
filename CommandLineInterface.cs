using Proiect_IA_Program.GraphUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proiect_IA_Program
{
    class CommandLineInterface
    {
        private Graph graph;
        private bool isRunning;
        public CommandLineInterface()
        {
            this.IsRunning = true;
        }

        public bool IsRunning { get => isRunning; set => isRunning = value; }

        private void PrintDomainMenu(int nodeNumber)
        {
            Console.Clear();
            Console.WriteLine($"\t\t[Domain Menu] [{graph.Nodes[nodeNumber].Name}]");
            Console.WriteLine($"Pick a state:");

            Console.WriteLine($"\t{-1}) NONE");
            int cntDomain = 0;
            foreach (string obj in graph.Nodes[nodeNumber].Domain)
            {
                Console.WriteLine($"\t {cntDomain++}) {obj}");
            }

            Console.Write($"Your option: ");
            string valueString = Console.ReadLine();
            bool success = int.TryParse(valueString, out int value);

            if (success && (-1 <= value && value < graph.Nodes[nodeNumber].Domain.Count))
            {
                graph.Nodes[nodeNumber].Observation = value;
            }
            else
            {
                Console.WriteLine($"[ERROR] Invalid Domain Value!");
                Console.WriteLine($"Press ENTER to continue...");
                Console.ReadLine();
            }
        }
        private void PrintObservationMenu()
        {
            Console.Clear();
            Console.WriteLine($"\t\t[Observation Menu]");
            Console.WriteLine($"Pick a node:");

            int cnt = 0;
            foreach (Node node in graph.Nodes)
            {
                Console.WriteLine($"\t {cnt++}) {node.Name}");
            }

            Console.Write($"Your option: ");
            string nodeNumberString = Console.ReadLine();
            bool success = int.TryParse(nodeNumberString, out int nodeNumber);

            if (success && (0 <= nodeNumber && nodeNumber < graph.Nodes.Count))
            {
                PrintDomainMenu(nodeNumber);
            }
            else
            {
                Console.WriteLine($"[ERROR] Invalid Node!");
                Console.WriteLine($"Press ENTER to continue...");
                Console.ReadLine();
            }
        }
        
        private void PrintViewObservationsMenu()
        {
            Console.Clear();
            Console.WriteLine($"\t\t[View Observation Menu]");

            foreach (Node node in graph.Nodes)
            {
                if (node.Observation != Node.NO_OBSERVATION)
                {
                    Console.WriteLine($"\t{node.Name} -> {node.Domain[node.Observation]}");
                }
            }
            Console.WriteLine($"Press ENTER to continue...");
            Console.ReadLine();
        }

        private void PrintQueryMenu()
        {
            Console.Clear();
            Console.WriteLine($"\t\t[Query Menu]");
            Console.WriteLine($"Pick a node for interogation:");

            int cnt = 0;
            foreach (Node node in graph.Nodes)
            {
                Console.WriteLine($"\t{cnt++}) {node.Name}");
            }

            Console.Write($"Your option: ");
            string nodeNumberString = Console.ReadLine();

            bool success = int.TryParse(nodeNumberString, out int nodeNumber);

            if (success && (0 <= nodeNumber && nodeNumber < graph.Nodes.Count))
            {
                Inference inferenceInstance = new Inference(graph);
                List<double> output = inferenceInstance.EnumerationAsk(nodeNumber);

                Console.WriteLine($"RESULTS[{graph.Nodes[nodeNumber].Name}]");

                for (int i = 0; i < output.Count; i++)
                {
                    string obj = graph.Nodes[nodeNumber].Domain[i];
                    double val = Math.Round(output[i], 2);
                    Console.WriteLine($"\t{obj}: \t {val}");
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"[ERROR] Invalid Node!");
            }
            Console.WriteLine($"Press ENTER to continue...");
            Console.ReadLine();
        }

        private void PrintMenu()
        {
            Console.Clear();

            Console.WriteLine($"Optiuni:");
            Console.WriteLine($"\t1)Make Observation");
            Console.WriteLine($"\t2)Make Query");
            Console.WriteLine($"\t3)Observation Menu");
            Console.Write($"Your option: ");
            string option = Console.ReadLine();
            
            
            switch (option)
            {
                case "1":
                    PrintObservationMenu();
                    break;
                case "2":
                    PrintQueryMenu();
                    break;
                case "3":
                    PrintViewObservationsMenu();
                    break;
                default:
                    Console.WriteLine($"[ERROR] Invalid Option!");
                    break;
            }
        }

        public void Run()
        {
            try
            {
                graph = Reader.CreateGraph();
                Console.WriteLine("[ LOG ] Graph was susccesfully created!\n");
            }
            catch
            {
                Console.WriteLine("[ERROR] Graph creation failed!");
                this.isRunning = false;
                return;
            }

            while (this.isRunning)
            {
                PrintMenu();
            }
            
        }
    }
}
