using JobProviderApp.Model;

namespace JobProviderApp.Interface
{
    public interface IJobRepository
    {
        Task<List<Job>> GetJobsByProviderIdAsync(int providerId);
        Task<Job> GetByIdAsync(int jobId);
        Task AddAsync(Job job);
        Task UpdateAsync(Job job);
        Task DeleteAsync(int jobId);
    }
}
