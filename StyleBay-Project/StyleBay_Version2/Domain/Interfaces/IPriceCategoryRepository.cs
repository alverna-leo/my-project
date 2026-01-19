using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IPriceCategoryRepository
    {
        Task<PriceCategory?> GetByIdAsync(Guid id);
        Task AddAsync(PriceCategory category);
        Task<List<PriceCategory>> GetAllAsync();
        Task UpdateAsync(PriceCategory category);
        Task DeleteAsync(PriceCategory category);
        
    }
}
