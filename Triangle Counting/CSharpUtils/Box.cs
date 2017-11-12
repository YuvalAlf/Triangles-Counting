using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpUtils
{
    public sealed class Box<T>
    {
        public T Value { get; set; }

        public Box(T value)
        {
            Value = value;
        }
    }
}
