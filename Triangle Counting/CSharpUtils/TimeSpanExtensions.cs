using System;

namespace CSharpUtils
{
    public static class TimeSpanExtensions
    {
        public static TimeSpan Square(this TimeSpan @this)
        {
            return TimeSpan.FromTicks(@this.Ticks * @this.Ticks);
        }

        public static TimeSpan Divide(this TimeSpan @this, int amount)
        {
            return TimeSpan.FromTicks(@this.Ticks / amount);
        }

        public static TimeSpan Sqrt(this TimeSpan @this)
        {
            return TimeSpan.FromTicks((long)Math.Sqrt(@this.Ticks));
        }
    }
}
