using AutoMapper;
using JobProviderApp.Dto;
using JobProviderApp.Interface;
using JobProviderApp.Model;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace JobProviderApp.Service
{
    public class AuthService : IAuthService
    {
        private readonly IJobProviderRepository _jobProviderRepo;
        // private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ProtectedSessionStorage _sessionStorage;
        private readonly IMapper _mapper;

        public AuthService(IJobProviderRepository jobProviderRepo, ProtectedSessionStorage sessionStorage,IMapper mapper)
        {
            _jobProviderRepo = jobProviderRepo;
            // _httpContextAccessor = httpContextAccessor;
            _sessionStorage = sessionStorage;
            _mapper = mapper;
        }

        public async Task<bool> Register(JobProviderDto jobProviderDto, string password)
        {
            var existingProvider = await _jobProviderRepo.GetByEmailAsync(jobProviderDto.Email);
            if (existingProvider != null)
                return false; // Email already exists

            var jobProvider = _mapper.Map<JobProvider>(jobProviderDto);
            jobProvider.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password); // Hash password

            await _jobProviderRepo.AddAsync(jobProvider);
            return true;
        }

        //public async Task<bool> Login(string email, string password)
        //{
        //    var jobProvider = await _jobProviderRepo.GetByEmailAsync(email);
        //    if (jobProvider == null || !BCrypt.Net.BCrypt.Verify(password, jobProvider.PasswordHash))
        //        return false;

        //    var session = _httpContextAccessor.HttpContext.Session;
        //    session.SetInt32("JobProviderId", jobProvider.Id);
        //    session.SetString("JobProviderEmail", jobProvider.Email);

        //    return true;
        //}
        //public async Task<bool> Login(string email, string password)
        //{
        //    var jobProvider = await _jobProviderRepo.GetByEmailAsync(email);
        //    if (jobProvider == null || !BCrypt.Net.BCrypt.Verify(password, jobProvider.PasswordHash))
        //        return false;

        //    try
        //    {
        //        var session = _httpContextAccessor.HttpContext.Session;
        //        Console.WriteLine("Session available: " + (session != null)); // Debug
        //        session.SetInt32("JobProviderId", jobProvider.Id);
        //        session.SetString("JobProviderEmail", jobProvider.Email);
        //        Console.WriteLine("Session set successfully"); // Debug
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Session error: " + ex.Message);
        //    }

        //    return true;
        //}

        //public void Logout()
        //{
        //    _httpContextAccessor.HttpContext.Session.Clear();
        //}
        public async Task<bool> Login(string email, string password)
        {
            var jobProvider = await _jobProviderRepo.GetByEmailAsync(email);
            if (jobProvider == null || !BCrypt.Net.BCrypt.Verify(password, jobProvider.PasswordHash))
                return false;

            try
            {
                await _sessionStorage.SetAsync("jobProviderid",jobProvider.Id);
                await _sessionStorage.SetAsync("jobProvideremail", jobProvider.Email);
                Console.WriteLine("Session stored successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Session error: " + ex.ToString());
            }


            return true;
        }

        public async Task Logout()
        {
            await _sessionStorage.DeleteAsync("jobProviderid");
            await _sessionStorage.DeleteAsync("jobProvideremail");
        }
    }

}
