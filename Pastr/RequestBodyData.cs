using System;
using System.Linq.Expressions;

namespace Pastr
{
    public class RequestBodyData
    {
        public RequestBodyData(string name, bool required = true, int min = -1, int max = -1)
        {
            Name = name;
            Required = required;
            Min = min;
            Max = max;
        }

        public string Name { get; set; }

        public bool Required { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }
    }
}

