using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IDressRepository
    {
        Task<IEnumerable<Dress>> GetAllDressesAsync();
        Task AddAsync(Dress dress);
        Task<Dress?> GetByIdAsync(Guid id);
        Task UpdateAsync(Dress dress);
        Task DeleteAsync(Dress dress);
    }
}
