using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace user.sharedkernel.Domain
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }
    }
}
