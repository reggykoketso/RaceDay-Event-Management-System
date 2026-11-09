
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RaceDayAPI.Services;

namespace RaceDayAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Organiser")]
    public class BlobStorageController : ControllerBase
    {
        private readonly BlobStorageService _blobStorageService;

        public BlobStorageController(
            BlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(
            IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please select an image.");
            }

            var allowedTypes = new[]
            {
                "image/jpeg",
                "image/png",
                "image/webp"
            };

            if (!allowedTypes.Contains(file.ContentType))
            {
                return BadRequest(
                    "Only JPG, PNG and WEBP images are allowed.");
            }

            if (file.Length > 5 * 1024 * 1024)
            {
                return BadRequest(
                    "The image must be smaller than 5 MB.");
            }

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";

            using var stream = file.OpenReadStream();

            var imageUrl =
                await _blobStorageService.UploadImageAsync(
                    stream,
                    fileName,
                    file.ContentType);

            return Ok(new
            {
                Message = "Image uploaded successfully.",
                ImageUrl = imageUrl
            });
        }
    }
}
