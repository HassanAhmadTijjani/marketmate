using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTracker.Data;
using ShopTracker.ViewModels;

namespace ShopTracker.Controllers
{
    public class DashboardController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        // GET: DashboardController
        // public async Task<IActionResult> Index()
        // {
        //     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //     var products = _context.Products.Where(p => p.UserId == userId);
        //     var totalProduct = await _context.Products.CountAsync();
        //     ViewData["totalProduct"] = totalProduct;
        //     var availabProduct = await _context.Products.Where(p => p.QuantityInStock > 0).CountAsync();
        //     ViewData["availabProduct"] = availabProduct;
        //     var outOfStock = await _context.Products.Where(p => p.QuantityInStock == 0).CountAsync();
        //     ViewData["outOfStock"] = outOfStock;
        //     // var today = DateTime.Today;
        //     // var tomorrow = today.AddDays(1);
        //     // var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //     // var nextMonth = startOfMonth.AddMonths(1);
        //     return View();
        // }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var today = DateTime.Today;

            // Calculate how many days we are away from Monday
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;

            // Start of week (Monday)
            var startOfWeek = today.AddDays(-diff);

            // End of week (next Monday)
            var endOfWeek = startOfWeek.AddDays(7);

            var weeklySales = await _context.SaleItems
    .Include(s => s.Sale)
    .Where(s => s.Sale.UserId == userId &&
                s.Sale.DateSold >= startOfWeek &&
                s.Sale.DateSold < endOfWeek)
    .ToListAsync();

            var weeklyRevenue = weeklySales.Sum(s => s.PriceAtSale * s.Quantity);
            var weeklyCost = weeklySales.Sum(s => s.CostAtSale * s.Quantity);
            var weeklyProfit = weeklyRevenue - weeklyCost;
            var weeklyItemsSold = weeklySales.Sum(s => s.Quantity);
            var TotalProductsInStock = await _context.Products.Where(p => p.UserId == userId).CountAsync();


            // // Get all sales of this user
            // var sales = await _context.SaleItems
            //     .Include(s => s.Product)
            //     .Include(s => s.Sale)
            //     .Where(s => s.Sale.UserId == userId)
            //     .ToListAsync();

            // // Get all products of this user
            // var products = await _context.Products
            //     .Where(p => p.UserId == userId)
            //     .ToListAsync();

            // // Prepare the DashboardViewModel
            // var dashboard = new DashboardViewModel
            // {
            //     TotalRevenue = sales.Sum(s => s.PriceAtSale * s.Quantity),
            //     TotalCost = sales.Sum(s => s.CostAtSale * s.Quantity),
            //     TotalItemsSold = sales.Sum(s => s.Quantity),
            //     TotalProductsInStock = products.Sum(p => p.QuantityInStock),
            //     SoldOutProducts = products.Where(p => p.QuantityInStock == 0)
            //                               .Select(p => p.Name)
            //                               .ToList()
            // };
            var dashboard = new DashboardViewModel
            {
                WeeklyRevenue = weeklyRevenue,
                WeeklyProfit = weeklyProfit,
                WeeklyItemsSold = weeklyItemsSold,
                TotalProductsInStock = TotalProductsInStock
            };

            return View(dashboard);
        }

    }
}
