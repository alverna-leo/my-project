using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

namespace Domain.Service
{
    public class DressService : IDressService
    {
        private readonly IDressRepository _dressRepository;
        private readonly IPriceCategoryRepository _priceCategoryRepository;
        public DressService(IDressRepository dressRepository, IPriceCategoryRepository priceCategoryRepository)
        {
            _dressRepository = dressRepository;
            _priceCategoryRepository = priceCategoryRepository;
        }
        public async Task<IEnumerable<Dress>> GetAllDressesAsync()
        {
            return await _dressRepository.GetAllDressesAsync();
        }
        public async Task<(bool Success, string? ErrorMessage)> CreateDressAsync(
       Dress dress,
       Stream? imageStream,
       string? fileName,
       string webRootPath)
        {
            // Price validation
            var priceCategory = await _priceCategoryRepository
                .GetByIdAsync(dress.PriceCategoryId);

            if (priceCategory == null)
                return (false, "Invalid price category selected.");

            if (dress.Price < priceCategory.MinPrice || dress.Price > priceCategory.MaxPrice)
                return (false, $"Price must be between {priceCategory.MinPrice} and {priceCategory.MaxPrice} for {priceCategory.Name} category.");

            // Image upload
            if (imageStream != null && fileName != null)
            {
                string folder = Path.Combine(webRootPath, "images");
                Directory.CreateDirectory(folder);

                string newFileName = Guid.NewGuid() + Path.GetExtension(fileName);
                string filePath = Path.Combine(folder, newFileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await imageStream.CopyToAsync(stream);

                dress.ImageName = "/images/" + newFileName;
            }

            await _dressRepository.AddAsync(dress);
            return (true, null);
        }
        public async Task<(bool Success, string? ErrorMessage)> UpdateDressAsync(
        Dress dress,
        Stream? imageStream,
        string? fileName,
        string webRootPath)
        {
            var existing = await _dressRepository.GetByIdAsync(dress.Id);
            if (existing == null)
                return (false, "Dress not found.");

            // Price validation
            var priceCategory = await _priceCategoryRepository
                .GetByIdAsync(dress.PriceCategoryId);

            if (priceCategory == null)
                return (false, "Invalid price category selected.");

            if (dress.Price < priceCategory.MinPrice || dress.Price > priceCategory.MaxPrice)
                return (false,
                    $"Price must be between {priceCategory.MinPrice} and {priceCategory.MaxPrice} for {priceCategory.Name} category.");

            // Update fields
            existing.Name = dress.Name;
            existing.Price = dress.Price;
            existing.Description = dress.Description;
            existing.DressCategoryId = dress.DressCategoryId;
            existing.PriceCategoryId = dress.PriceCategoryId;

            // Image update only if new image uploaded
            if (imageStream != null && fileName != null)
            {
                string folder = Path.Combine(webRootPath, "images");
                Directory.CreateDirectory(folder);

                string newFile = Guid.NewGuid() + Path.GetExtension(fileName);
                string path = Path.Combine(folder, newFile);

                using var stream = new FileStream(path, FileMode.Create);
                await imageStream.CopyToAsync(stream);

                existing.ImageName = "/images/" + newFile;
            }

            await _dressRepository.UpdateAsync(existing);
            return (true, null);
        }
        public async Task<Dress?> GetByIdAsync(Guid id)
        {
            return await _dressRepository.GetByIdAsync(id);
        }
        public async Task<(bool Success, string? ErrorMessage)> DeleteAsync(Guid id, string webRootPath)
        {
            var dress = await _dressRepository.GetByIdAsync(id);

            if (dress == null)
                return (false, "Dress not found.");

            // Delete image
            if (!string.IsNullOrEmpty(dress.ImageName))
            {
                var imagePath = Path.Combine(
                    webRootPath,
                    dress.ImageName.TrimStart('/')
                );

                if (File.Exists(imagePath))
                    File.Delete(imagePath);
            }

            await _dressRepository.DeleteAsync(dress);

            return (true, null);
        }

    }
}
