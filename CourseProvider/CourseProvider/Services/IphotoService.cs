
using CloudinaryDotNet.Actions;

namespace CourseProvider.Services {
    public interface IphotoService 
    {
        Task<ImageUploadResult> AddPhotsAsync(IFormFile file);
        Task<DeletionResult> DeletePhotAsync(string publicId);
    }
}
