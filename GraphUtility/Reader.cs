using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Proiect_IA_Program.GraphUtility
{
    public static class Reader
    {
        // CHANGE THIS
        private const string FILE_PATH = "..\\..\\..\\resources\\Proiect.xml";

        private static dynamic ReadFromFile(string filePath)
        {
            string jsonString;
            //using (StreamReader r = new StreamReader(filePath))
            //{
              //  jsonString = r.ReadToEnd();
            //}

            // Load the XML document
             System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
             doc.Load(FILE_PATH);

             // Convert the XML document to a JSON string
             jsonString= JsonConvert.SerializeXmlNode(doc);
            
            var obj = JObject.Parse(jsonString);
            return obj["BIF"]["NETWORK"];
        }
        
        public static Graph CreateGraph()
        {
            Graph graph = new Graph();

            var graphJSON = ReadFromFile(FILE_PATH);

            foreach (var node in graphJSON["VARIABLE"])
            {
                List<string> domain = new List<string>();
                foreach (var obj in node["OUTCOME"])
                    domain.Add(obj.ToString());

                string name = node["NAME"];

                Node newNode = new Node(name, domain);
                graph.Nodes.Add(newNode);
            }

            foreach (var nodeDetails in graphJSON["DEFINITION"])
            {
                string currentNodeName = nodeDetails["FOR"];
                int nodeIndex = graph.GetNodeIndexByName(currentNodeName);

                if (nodeDetails["GIVEN"] != null)
                {
                    try
                    {
                        foreach (string parent in nodeDetails["GIVEN"])
                        {
                            int parentIndex = graph.GetNodeIndexByName(parent);
                            graph.Nodes[nodeIndex].Parents.Add(parentIndex);
                        }
                    }
                    catch
                    {
                        string parentName = nodeDetails["GIVEN"];
                        int parentIndex = graph.GetNodeIndexByName(parentName);
                        graph.Nodes[nodeIndex].Parents.Add(parentIndex);
                    }                    
                }

                
                string tableString = nodeDetails["TABLE"];
                //Console.WriteLine("PArinti:" + graph.Nodes[nodeIndex].Parents.Count);
                //Console.Write("TABLE VALUEs " + currentNodeName + " :");
                string[] tableValuesString = tableString.Split(' ');
                //foreach(string data in tableValuesString)
                //Console.Write(data + " ");
                List<double> doubleValues = new List<double>();
                foreach (string valueString in tableValuesString)
                {
                    double value = double.Parse(valueString, CultureInfo.InvariantCulture);
                   // Console.Write(value + " ");
                    doubleValues.Add(value);
                }
                //Console.WriteLine();

                for (int i = 0; i < doubleValues.Count; i += graph.Nodes[nodeIndex].Domain.Count)
                {
                    List<double> toAddList = new List<double>();
                    for (int j = i; j < i + graph.Nodes[nodeIndex].Domain.Count; j++)
                    {
                        toAddList.Add(doubleValues[j]);
                        //Console.Write(doubleValues[j].ToString() + " ");
                    }
                    //Console.WriteLine();

                    graph.Nodes[nodeIndex].Table.Add(toAddList);
                }
            }

            return graph;
        }
    }
}
