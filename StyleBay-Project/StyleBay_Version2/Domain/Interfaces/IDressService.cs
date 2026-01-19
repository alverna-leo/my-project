using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IDressService
    {
        Task<IEnumerable<Dress>> GetAllDressesAsync();
        Task<(bool Success, string? ErrorMessage)> CreateDressAsync(
        Dress dress,
        Stream? imageStream,
        string? fileName,
        string webRootPath);
        Task<(bool Success, string? ErrorMessage)> UpdateDressAsync(
        Dress dress,
        Stream? imageStream,
        string? fileName,
        string webRootPath);
        Task<Dress?> GetByIdAsync(Guid id);
        Task<(bool Success, string? ErrorMessage)> DeleteAsync(Guid id, string webRootPath);
    }
}

     
