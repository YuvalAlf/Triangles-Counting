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

        public override string ToString()
        {
            return $"Result: {Result}, Overall Time: {OverallTime.TotalSeconds}sec, Average time per operation: {AverageMsTimePerOperation.TotalMilliseconds}ms, Standard deviation: {TimeStdDev.TotalMilliseconds}ms";
        }
    }
}
