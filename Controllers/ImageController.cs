using ABCRetail.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetail.Controllers
{
    [Authorize] // Only logged-in admins can access
    public class ImageController : Controller
    {
        private readonly BlobStorageService _blobService;

        public ImageController(BlobStorageService blobService)
        {
            _blobService = blobService;
        }

        // GET: /Image/Gallery
        public async Task<IActionResult> Gallery()
        {
            // Retrieve all blob names
            var fileNames = await _blobService.GetAllFilesAsync();

            // Convert blob names to URLs
            var urls = fileNames.Select(name => _blobService.GetFileUrl(name)).ToList();

            return View(urls);
        }

        // GET: /Image/Upload
        public IActionResult Upload()
        {
            return View();
        }

        // POST: /Image/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // Upload file to Azure Blob Storage
                await _blobService.UploadFileAsync(file.FileName, file.OpenReadStream(), file.ContentType);

                // Optional: feedback message
                TempData["UploadMessage"] = $"Image '{file.FileName}' uploaded successfully!";
            }
            else
            {
                TempData["UploadMessage"] = "No file selected or file is empty.";
            }

            return RedirectToAction("Upload");
        }
    }
}

