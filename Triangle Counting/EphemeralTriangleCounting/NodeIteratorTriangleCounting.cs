using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpUtils;
using TriangleCountingProblem;

namespace EphemeralTriangleCounting
{
    public sealed class NodeIteratorTriangleCounting : TriangleCountingSolver
    {
        public override IEnumerable<TimeSpan> NumberOfTriangles(IEnumerable<Tuple<int, int>> nodes, Box<int> numOfTriangles)
        {
            var starting = DateTime.Now;
            var graph = EphemeralGraph.Empty;
            foreach (var edge in nodes)
            {
                graph.AddEdge(edge);
                graph.AddEdge(edge.Inverse());
            }
            numOfTriangles.Value = graph.NumOfTrianglesNodeIterator();
            var ending = DateTime.Now;
            yield return ending - starting;
        }

        public override string SolverName => "Node Iterator";
    }
}
