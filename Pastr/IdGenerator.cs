using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pastr
{
    public class IdGenerator
    {
        private static IdGenerator _instance;

        public static IdGenerator Instance
        {
            get
            {
                return _instance ?? (_instance = new IdGenerator());
            }
        }

        public string CreateNew()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public Guid ToGuidUnsafe(string input)
        {
            return Guid.Parse(input);
        }

        public bool ToGuid(string input, out Guid guid)
        {
            return Guid.TryParse(input, out guid);
        }
    }
}
