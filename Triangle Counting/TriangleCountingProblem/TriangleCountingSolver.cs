using System;
using System.Collections.Generic;
using CSharpUtils;

namespace TriangleCountingProblem
{
    public abstract class TriangleCountingSolver
    {
        public abstract IEnumerable<TimeSpan> NumberOfTriangles(IEnumerable<Tuple<int, int>> edges, Box<int> numOfTriangles);

        public abstract string SolverName { get; }

        public BenchmarkResults<int> Benchmark(IEnumerable<Tuple<int, int>> edges)
        {
            TimeSpan overallTime = TimeSpan.Zero;
            TimeSpan overallSquaredTime = TimeSpan.Zero;
            int numOfOperations = 0;
            Box<int> numOfTriangles = new Box<int>(0);
            foreach (var opTime in NumberOfTriangles(edges, numOfTriangles))
            {
                numOfOperations += 1;
                overallTime += opTime;
                overallSquaredTime += opTime.Square();
            }
            TimeSpan averageTime = overallTime.Divide(numOfOperations);
            TimeSpan varianceTime = overallSquaredTime.Divide(numOfOperations) - averageTime;
            TimeSpan stdDev = varianceTime.Sqrt();

            return new BenchmarkResults<int>(numOfTriangles.Value, overallTime, averageTime, stdDev);
        }
    }
}
