using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IDressCategoryRepository
    {
        Task<List<DressCategory>> GetAllAsync();
        Task<DressCategory?> GetByIdAsync(Guid id);
        Task DeleteAsync(DressCategory category);
        Task AddAsync(DressCategory category);
    }
}
