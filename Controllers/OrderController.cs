using ABCRetail.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetail.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly QueueStorageService _queueService;

        public OrdersController(QueueStorageService queueService)
        {
            _queueService = queueService;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await _queueService.PeekMessagesAsync();
            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(string orderDetails)
        {
            if (!string.IsNullOrEmpty(orderDetails))
            {
                await _queueService.SendMessageAsync(orderDetails);
            }
            return RedirectToAction("Index");
        }
    }
}
