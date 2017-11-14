using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpUtils;

namespace Benchmark
{
    public sealed class GraphProperties
    {
        public int N { get; }
        public int M { get; }
        public int D_Max { get; }

        public GraphProperties(int n, int m, int dMax)
        {
            N = n;
            M = m;
            D_Max = dMax;
        }

        public override string ToString()
        {
            return $"{nameof(N)}: {N}, {nameof(M)}: {M}, {nameof(D_Max)}: {D_Max}";
        }

        private IEnumerable<Tuple<int,int>> GenerateGraph(Random rnd)
        {
            for (int i = 0; i < D_Max; i++)
                yield return Tuple.Create(0, rnd.Next(1, N));
            for (int i = 0; i < M; i++)
            {
                var node1 = rnd.Next(1, N);
                var node2 = rnd.Next(1, N);
                while (node1 == node2)
                    node2 = rnd.Next(1, N);
                yield return Tuple.Create(node1, node2);
            }
        }

        public Tuple<int, int>[] Generate(Random rnd)
        {
            HashSet<Tuple<int, int>> edges = new HashSet<Tuple<int, int>>();
            foreach (var edge in GenerateGraph(rnd))
                if (!(edges.Contains(edge) || edges.Contains(edge.Inverse())))
                    edges.Add(edge);
            return edges.OrderBy(_ => rnd.Next()).ToArray();
        }
    }
}
