using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpUtils;
using TriangleCountingProblem;

namespace EphemeralTriangleCounting
{
    public sealed class IncrementalTriangleCounting : TriangleCountingSolver
    {
        public override IEnumerable<TimeSpan> NumberOfTriangles(IEnumerable<Tuple<int, int>> edges, Box<int> numOfTriangles)
        {
            int triangles = 0;
            var graph = EphemeralGraph.Empty;
            foreach (var edge in edges)
            {
                var starting = DateTime.Now;
                var node1 = edge.Item1;
                var node2 = edge.Item2;
                if (graph.Nodes.ContainsKey(node1) && graph.Nodes.ContainsKey(node2))
                {
                    var minNode = graph.Nodes[node1].Count < graph.Nodes[node2].Count ? node1 : node2;
                    var maxNode = node1 == minNode ? node2 : node1;
                    foreach (var neighbor in graph.Nodes[minNode])
                        if (graph.Nodes[neighbor].Contains(maxNode))
                            triangles++;
                }
                graph.AddEdge(edge);
                graph.AddEdge(edge.Inverse());
                var ending = DateTime.Now;
                yield return ending - starting;
            }
            numOfTriangles.Value = triangles;
        }

        public override string SolverName => "Incremental";
    }
}
