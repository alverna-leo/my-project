using JobProviderApp.Model;

namespace JobProviderApp.Interface
{
    public interface IJobProviderRepository
    {
        Task<JobProvider> GetByEmailAsync(string email);
        Task AddAsync(JobProvider jobProvider);
    }
}
