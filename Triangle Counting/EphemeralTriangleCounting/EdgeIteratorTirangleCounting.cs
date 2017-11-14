using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpUtils;
using TriangleCountingProblem;

namespace EphemeralTriangleCounting
{
    public sealed class EdgeIteratorTriangleCounting : TriangleCountingSolver
    {
        public override IEnumerable<TimeSpan> NumberOfTriangles(IEnumerable<Tuple<int, int>> edges, Box<int> numOfTriangles)
        {
            var starting = DateTime.Now;
            var graph = EphemeralGraph.Empty;
            foreach (var edge in edges)
            {
                graph.AddEdge(edge);
                graph.AddEdge(edge.Inverse());
            }
            numOfTriangles.Value = graph.NumOfTrianglesEdgeIterator();
            var ending = DateTime.Now;
            yield return ending - starting;
        }

        public override string SolverName => "Edge Iterator";
    }
}
