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
    public class DressRepository : IDressRepository
    {
        private readonly ApplicationDbContext _context;
        public DressRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Dress>> GetAllDressesAsync()
        {
            return await _context.Dresses
                .Include(d => d.DressCategory)
                .Include(d => d.PriceCategory)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }
        public async Task AddAsync(Dress dress)
        {
            await _context.Dresses.AddAsync(dress);
            await _context.SaveChangesAsync();
        }
        public async Task<Dress?> GetByIdAsync(Guid id)
        {
            return await _context.Dresses.FindAsync(id);
        }
        public async Task UpdateAsync(Dress dress)
        {
            _context.Dresses.Update(dress);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Dress dress)
        {
            _context.Dresses.Remove(dress);
            await _context.SaveChangesAsync();
        }
    }
}
