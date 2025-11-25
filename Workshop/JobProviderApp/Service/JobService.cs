using AutoMapper;
using JobProviderApp.Dto;
using JobProviderApp.Interface;
using JobProviderApp.Model;

namespace JobProviderApp.Service
{
    public class JobService : IJobservice
    {
        private readonly IJobRepository _jobRepository;
        private readonly IMapper _mapper;

        public JobService(IJobRepository jobRepository, IMapper mapper)
        {
            _jobRepository = jobRepository;
            _mapper = mapper;
        }

        public async Task<List<JobDto>> GetJobsByProviderIdAsync(int providerId)
        {
            var jobs = await _jobRepository.GetJobsByProviderIdAsync(providerId);
            return _mapper.Map<List<JobDto>>(jobs);
        }

        public async Task<bool> AddJobAsync(JobDto jobDto, int providerId)
        {
            var job = _mapper.Map<Job>(jobDto);
            job.JobProviderId = providerId;
            await _jobRepository.AddAsync(job);
            return true;
        }

        public async Task<bool> UpdateJobAsync(JobDto jobDto)
        {
            var job = await _jobRepository.GetByIdAsync(jobDto.Id);
            if (job == null) return false;

            _mapper.Map(jobDto, job);
            await _jobRepository.UpdateAsync(job);
            return true;
        }

        public async Task<bool> DeleteJobAsync(int jobId)
        {
            await _jobRepository.DeleteAsync(jobId);
            return true;
        }
    }

}
