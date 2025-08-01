using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using user.sharedkernel;

namespace user.domain.Entities
{
    public class User : BaseEntity
    {
        public UserName Name { get; private set; }

        public User(UserName name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public void UpdateName(UserName name)
        {
            Name = name;
        }
    }
}
