using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace user.sharedkernel
{
    public class UserName
    {
        public string First { get; private set; }
        public string Last { get; private set; }

        public UserName(string first, string last)
        {
            if (string.IsNullOrWhiteSpace(first) || string.IsNullOrWhiteSpace(last))
                throw new ArgumentException("Name fields cannot be empty.");

            First = first;
            Last = last;
        }

        public override string ToString() => $"{First} {Last}";
    }
}
