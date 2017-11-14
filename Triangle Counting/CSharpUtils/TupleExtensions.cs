using System;

namespace CSharpUtils
{
    public static class TupleExtensions
    {
        public static Tuple<A, B> Inverse<A, B>(this Tuple<B, A> @this) => Tuple.Create(@this.Item2, @this.Item1);
    }
}
