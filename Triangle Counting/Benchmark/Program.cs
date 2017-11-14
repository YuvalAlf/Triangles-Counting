using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpUtils;
using EphemeralTriangleCounting;
using PersistentTriangleCounting;
using TriangleCountingProblem;

namespace Benchmark
{
    class Program
    {

        static void Main(string[] args)
        {
            var rnd = new Random();
            var graph1 = new GraphProperties(10.Pow(4), 10.Pow(5), 10.Pow(3));
            var graph2 = new GraphProperties(10.Pow(4), 10.Pow(6), 10.Pow(4));
            var graph3 = new GraphProperties(10.Pow(5), 10.Pow(6), 10.Pow(4));
            var graph4 = new GraphProperties(10.Pow(5), 10.Pow(7), 10.Pow(5));
            var graph5 = new GraphProperties(10.Pow(5), 10.Pow(7), 10.Pow(5));
            var graphs = new[] {graph4};

            var incrementalSolver = new IncrementalTriangleCounting();
            var edgeIteratorSolver = new EdgeIteratorTriangleCounting();
            var nodeIteratorSolver = new NodeIteratorTriangleCounting();
            var persistentSolver = PersistentCounter.Create();
            var solvers = new TriangleCountingSolver[] {incrementalSolver, edgeIteratorSolver, nodeIteratorSolver, persistentSolver};

            foreach (var graphProperties in graphs)
            {
                var graph = graphProperties.Generate(rnd);
                Console.WriteLine(graphProperties + ":");
                foreach (var solver in solvers)
                {
                    var benchmarkResults = solver.Benchmark(graph);
                    Console.WriteLine("\t" + solver.SolverName + ": " + benchmarkResults);
                }
            }

            var n = 10.Pow(6);

            /*for (int m = 1000; m <= 10000000; m += m)
            {
                var p = new GraphProperties(n, m, 0);
                var graph = p.Generate(rnd);
                Console.Write("m = " + m + ": \t");
                Console.Write(new IncrementalTriangleCounting().Benchmark(graph).AverageMsTimePerOperation.TotalMilliseconds + "ms \t");
                Console.Write(PersistentCounter.Create().Benchmark(graph).AverageMsTimePerOperation.TotalMilliseconds + "ms");

                Console.WriteLine();
            }*/
        }
    }
}
