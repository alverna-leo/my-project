using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IDressCategoryService
    {
        Task<List<DressCategory>> GetAllAsync();
        Task<DressCategory?> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task AddAsync(DressCategory category);
    }
}
