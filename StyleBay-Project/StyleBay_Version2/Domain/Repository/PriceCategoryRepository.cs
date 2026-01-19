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
    public class PriceCategoryRepository : IPriceCategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public PriceCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<PriceCategory?> GetByIdAsync(Guid id)
        {
            return await _context.PriceCategories
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(PriceCategory category)
        {
            await _context.PriceCategories.AddAsync(category);
            await _context.SaveChangesAsync();
        }
        public async Task<List<PriceCategory>> GetAllAsync()
        {
            return await _context.PriceCategories.ToListAsync();
        }
        public async Task UpdateAsync(PriceCategory category)
        {
            _context.PriceCategories.Update(category);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(PriceCategory category)
        {
            _context.PriceCategories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
