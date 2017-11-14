using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpUtils;

namespace EphemeralTriangleCounting
{
    public sealed class EphemeralGraph
    {
        public Dictionary<int, HashSet<int>> Nodes { get; }
        public HashSet<Tuple<int, int>> Edges { get; }

        public EphemeralGraph(Dictionary<int, HashSet<int>> nodes, HashSet<Tuple<int, int>> edges)
        {
            Nodes = nodes;
            Edges = edges;
        }

        public static EphemeralGraph Empty => new EphemeralGraph(new Dictionary<int, HashSet<int>>(), new HashSet<Tuple<int, int>>());



        public void AddEdge(Tuple<int, int> edge)
        {
            var node1 = edge.Item1;
            var node2 = edge.Item2;
            if(!Nodes.ContainsKey(node1))
                Nodes.Add(node1, new HashSet<int>());
            Nodes[node1].Add(node2);
            if (!Edges.Contains(edge.Inverse()))
                Edges.Add(edge);
        }


        public int NumOfTrianglesEdgeIterator()
        {
            int numOfTriangles = 0;
            foreach (var edge in Edges)
            {
                var node1 = edge.Item1;
                var node2 = edge.Item2;
                if (Nodes.ContainsKey(node1) && Nodes.ContainsKey(node2))
                {
                    var minNode = Nodes[node1].Count < Nodes[node2].Count ? node1 : node2;
                    var maxNode = node1 == minNode ? node2 : node1;
                    foreach (var neighbor in Nodes[minNode])
                        if (Nodes[neighbor].Contains(maxNode))
                            numOfTriangles++;
                }
            }
            return numOfTriangles / 3;
        }

        public int NumOfTrianglesNodeIterator()
        {
            int numOfTriangles = 0;
            foreach (var node in Nodes.Keys)
                foreach (var neighbor1 in Nodes[node])
                    foreach (var neighbor2 in Nodes[neighbor1])
                        if (Nodes[neighbor2].Contains(node))
                            numOfTriangles++;
            return numOfTriangles / 6;
        }
    }
}
