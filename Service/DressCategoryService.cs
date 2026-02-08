using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

namespace Domain.Service
{
    public class DressCategoryService : IDressCategoryService
    {
        private readonly IDressCategoryRepository _repository;

        public DressCategoryService(IDressCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DressCategory>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<DressCategory?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);

            if (category == null)
                throw new Exception("Dress category not found.");

            await _repository.DeleteAsync(category);
        }
        public async Task AddAsync(DressCategory category)
        {
            await _repository.AddAsync(category);
        }
    }
}
