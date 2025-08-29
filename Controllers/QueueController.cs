using ABCRetail.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetail.Controllers
{
    public class QueueController : Controller
    {
        private readonly QueueStorageService _queueService;

        public QueueController(QueueStorageService queueService)
        {
            _queueService = queueService;
        }

        // GET: /Queue
        public async Task<IActionResult> Index()
        {
            var messages = await _queueService.PeekMessagesAsync(10);
            return View(messages);
        }

        // GET: /Queue/Send
        public IActionResult Send()
        {
            return View();
        }

        // POST: /Queue/Send
        [HttpPost]
        public async Task<IActionResult> Send(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                await _queueService.SendMessageAsync(message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
