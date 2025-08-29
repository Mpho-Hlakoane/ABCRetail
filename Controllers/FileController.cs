using ABCRetail.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetail.Controllers
{
    [Authorize] // Only logged-in users/admins can access
    [Route("File")]
    public class FileController : Controller
    {
        private readonly FileStorageService _fileService;

        public FileController(FileStorageService fileService)
        {
            _fileService = fileService;
        }

        // GET: /File
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var files = await _fileService.GetAllFilesAsync();
            return View(files);
        }

        // GET: /File/Upload
        [HttpGet("Upload")]
        public IActionResult Upload()
        {
            return View();
        }

        // POST: /File/Upload
        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please select a file to upload.";
                return RedirectToAction(nameof(Index));
            }

            using (var stream = file.OpenReadStream())
            {
                var content = await new StreamReader(stream).ReadToEndAsync();
                await _fileService.WriteLogAsync(file.FileName, content);
            }

            TempData["Success"] = $"File '{file.FileName}' uploaded successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
