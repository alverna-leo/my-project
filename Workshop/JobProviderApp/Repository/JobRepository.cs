using JobProviderApp.Interface;
using JobProviderApp.Model;
using Microsoft.EntityFrameworkCore;

namespace JobProviderApp.Repository
{
    public class JobRepository : IJobRepository
    {
        private readonly JobProviderAppDbContext _context;

        public JobRepository(JobProviderAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Job>> GetJobsByProviderIdAsync(int providerId)
        {
            return await _context.Jobs.Where(j => j.JobProviderId == providerId).ToListAsync();
        }

        public async Task<Job> GetByIdAsync(int jobId)
        {
            return await _context.Jobs.FindAsync(jobId);
        }

        public async Task AddAsync(Job job)
        {
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Job job)
        {
            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int jobId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job != null)
            {
                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();
            }
        }
    }

}
