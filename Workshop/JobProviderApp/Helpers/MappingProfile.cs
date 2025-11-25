using AutoMapper;
using JobProviderApp.Dto;
using JobProviderApp.Model;

namespace JobProviderApp.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<JobProvider, JobProviderDto>().ReverseMap();
            CreateMap<Job, JobDto>().ReverseMap();  
        }
    }
}
