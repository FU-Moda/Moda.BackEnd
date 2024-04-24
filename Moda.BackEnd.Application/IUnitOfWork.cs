using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Application
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
