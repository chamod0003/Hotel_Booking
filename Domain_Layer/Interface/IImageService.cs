using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Interface
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(byte[] fileData, string fileName, string contentType, string folder = "hotels");
        Task<bool> DeleteImageAsync(string imagePath);
        Task<List<string>> SaveMultipleImagesAsync(IEnumerable<(byte[] fileData, string fileName, string contentType)> imageFiles, string folder = "hotels");
        bool IsValidImageFile(byte[] fileData, string fileName, string contentType, long fileSize);
        string GetImageUrl(string imagePath);
    }
}