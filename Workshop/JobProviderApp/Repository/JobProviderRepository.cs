using JobProviderApp.Interface;
using JobProviderApp.Model;
using Microsoft.EntityFrameworkCore;

namespace JobProviderApp.Repository
{
    public class JobProviderRepository : IJobProviderRepository
    {
        private readonly JobProviderAppDbContext _context;

        public JobProviderRepository(JobProviderAppDbContext context)
        {
            _context = context;
        }

        public async Task<JobProvider> GetByEmailAsync(string email)
        {
            return await _context.JobProviders.FirstOrDefaultAsync(jp => jp.Email == email);
        }

        public async Task AddAsync(JobProvider jobProvider)
        {
            _context.JobProviders.Add(jobProvider);
            await _context.SaveChangesAsync();
        }
    }

}
