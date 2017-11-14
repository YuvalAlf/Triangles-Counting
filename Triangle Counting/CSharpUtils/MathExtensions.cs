using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpUtils
{
    public static class MathExtensions
    {
        public static int Pow(this int @this, int exp) => (int) Math.Pow(@this, exp);
    }
}
