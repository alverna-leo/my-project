using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Data;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace Domain.Repository
{
    public class DressCategoryRepository : IDressCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public DressCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DressCategory>> GetAllAsync()
        {
            return await _context.DressCategories.ToListAsync();
        }
        public async Task<DressCategory?> GetByIdAsync(Guid id)
        {
            return await _context.DressCategories.FindAsync(id);
        }
        public async Task DeleteAsync(DressCategory category)
        {
            _context.DressCategories.Remove(category);
            await _context.SaveChangesAsync();
        }
        public async Task AddAsync(DressCategory category)
        {
            await _context.DressCategories.AddAsync(category);
            await _context.SaveChangesAsync();
        }
    }
}
