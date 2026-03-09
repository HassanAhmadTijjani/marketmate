using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopTracker.Data;
using ShopTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ShopTracker.Controllers
{
    [Authorize]
    public class SalesController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // 

        // GET: SalesController
        public async Task<IActionResult> Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var products = await _context.Products
                .Where(p => p.UserId == userId && p.QuantityInStock > 0).ToListAsync();

            ViewBag.Products = products;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int productId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId && p.UserId == userId);

            if (product == null)
                return NotFound();

            if (product.QuantityInStock < quantity)
            {
                ModelState.AddModelError("", "Not enough stock available.");

                var products = await _context.Products
                    .Where(p => p.UserId == userId && p.QuantityInStock > 0)
                    .ToListAsync();

                ViewBag.Products = products;

                return View();
            }

            // Reduce stock
            product.QuantityInStock -= quantity;

            var saleItem = new SaleItem
            {
                ProductId = product.ProductId,
                Quantity = quantity,
                PriceAtSale = product.SellingPrice,
                CostAtSale = product.CostPrice
            };

            var sale = new Sale
            {
                UserId = userId,
                TotalAmount = quantity * product.SellingPrice,
                SaleItems = [saleItem]
            };

            _context.Sales.Add(sale);

            await _context.SaveChangesAsync();

            TempData["success"] = "Sale recorded successfully!";
            return RedirectToAction("Index", "Product");
        }

        // GET: History
        public async Task<IActionResult> History()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var saleItems = _context.SaleItems.AsNoTracking()
                .Include(s => s.Product)
                .Include(s => s.Sale)
                .Where(s => s.Sale.UserId == userId).OrderByDescending(s => s.Sale.DateCreated);

            return View(await saleItems.ToListAsync());
        }

    }
}
