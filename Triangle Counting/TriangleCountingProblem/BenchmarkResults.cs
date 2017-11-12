using System;

namespace TriangleCountingProblem
{
    public sealed class BenchmarkResults<T>
    {
        public T Result { get; }
        public TimeSpan OverallTime { get; }
        public TimeSpan AverageMsTimePerOperation { get; }
        public TimeSpan TimeStdDev { get; }

        public BenchmarkResults(T result, TimeSpan overallTime, TimeSpan averageMsTimePerOperation, TimeSpan timeStdDev)
        {
            Result = result;
            OverallTime = overallTime;
            AverageMsTimePerOperation = averageMsTimePerOperation;
            TimeStdDev = timeStdDev;
        }
    }
}
