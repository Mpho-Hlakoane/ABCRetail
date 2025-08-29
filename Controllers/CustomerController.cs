using ABCRetail.Models;
using ABCRetail.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetail.Controllers
{
    [Authorize] // Only logged-in admin can access
    public class CustomerController : Controller
    {
        private readonly TableStorageService _tableService;

        public CustomerController(TableStorageService tableService)
        {
            _tableService = tableService;
        }

        // GET: /Customer
        public async Task<IActionResult> Index()
        {
            var customers = await _tableService.GetAllCustomersAsync();
            return View(customers);
        }

        // GET: /Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Customer/Create
        [HttpPost]
        public async Task<IActionResult> Create(CustomerProductEntity entity)
        {
            if (!ModelState.IsValid) return View(entity);

            entity.PartitionKey = "Customer";
            entity.RowKey = Guid.NewGuid().ToString(); // Unique ID

            await _tableService.AddCustomerAsync(entity);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Customer/Edit/{rowKey}
        public async Task<IActionResult> Edit(string rowKey)
        {
            var customer = await _tableService.GetCustomerAsync("Customer", rowKey);
            if (customer == null) return NotFound();
            return View(customer);
        }

        // POST: /Customer/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(CustomerProductEntity entity)
        {
            if (!ModelState.IsValid) return View(entity);

            await _tableService.UpsertCustomerAsync(entity);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Customer/Delete/{rowKey}
        [HttpPost]
        public async Task<IActionResult> Delete(string rowKey)
        {
            await _tableService.DeleteCustomerAsync("Customer", rowKey);
            return RedirectToAction(nameof(Index));
        }
    }
}
