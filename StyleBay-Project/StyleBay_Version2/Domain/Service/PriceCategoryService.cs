using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

namespace Domain.Service
{
    public class PriceCategoryService : IPriceCategoryService
    {
        private readonly IPriceCategoryRepository _repository;

        public PriceCategoryService(IPriceCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(PriceCategory category)
        {
            await _repository.AddAsync(category);
        }
        public async Task<List<PriceCategory>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<PriceCategory?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(PriceCategory category)
        {
            await _repository.UpdateAsync(category);
        }
        public async Task DeleteAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);

            if (category == null)
                throw new Exception("Category not found");

            await _repository.DeleteAsync(category);
        }
    }
}
