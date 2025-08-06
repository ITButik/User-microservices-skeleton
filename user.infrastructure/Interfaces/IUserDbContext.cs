using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using user.domain.Entities;

namespace user.application.Interfaces;

public interface IUserDbContext
{
    DbSet<User> Users { get; }



    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
