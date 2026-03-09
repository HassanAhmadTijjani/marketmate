// using System.Security.Claims;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using ShopTracker.Data;
// using ShopTracker.Models;

// namespace ShopTracker.Controllers
// {
//     public class ProductController(ApplicationDbContext context) : Controller
//     {
//         private readonly ApplicationDbContext _context = context;

//         // GET: ProductController
//         [Authorize]
//         public async Task<IActionResult> Index(string searchString)
//         {
//             // Get Current Logged in User
//             var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//             // Filter products that belong only to this user id
//             var products = _context.Products
//                                        .Where(p => p.UserId == userId)
//                                        .AsQueryable();
//             if (!string.IsNullOrEmpty(searchString))
//             {
//                 products = products.Where(p => p.Name.Contains(searchString));
//             }
//             ViewData["CurrentFilter"] = searchString;
//             return View(await products.ToListAsync());
//         }

//         // GET: Products/Detail
//         [Authorize]
//         public async Task<IActionResult> Detail(int? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//             var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//             var product = await _context.Products
//                 .FirstOrDefaultAsync(p => p.ProductId == id && p.UserId == userId);
//             if (product == null)
//             {
//                 return NotFound();
//             }
//             return View(product);
//         }

//         // GET: Create/Product
//         [Authorize]
//         public async Task<IActionResult> Create()
//         {
//             return View(new Product());
//         }


//         // POST: Create/Product
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         [Authorize]
//         public async Task<IActionResult> Create(Product product)
//         {
//             if (ModelState.IsValid)
//             {
//                 // Get current logged in user id
//                 var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//                 // attach product to this user
//                 product.UserId = userId;
//                 _context.Products.Add(product);
//                 await _context.SaveChangesAsync();
//                 TempData["success"] = "Product created successfully!";
//                 return RedirectToAction(nameof(Index));
//             }
//             return View(product);
//         }

//         // GET: Edit/Products
//         [Authorize]
//         public async Task<IActionResult> Edit(int? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//             var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//             var product = await _context.Products
//                 .FirstOrDefaultAsync(p => p.ProductId == id && p.UserId == userId); if (product == null)
//             {
//                 return NotFound();
//             }
//             return View(product);
//         }

//         // POST: Update/Products
//         [Authorize]
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Update(int? id, Product product)
//         {
//             if (id == null || id <= 0)
//             {
//                 return NotFound();
//             }
//             var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//             if (ModelState.IsValid)
//             {
//                 //     try
//                 //     {
//                 //         _context.Update(product);
//                 //         await _context.SaveChangesAsync();
//                 //     }
//                 //     catch (DbUpdateConcurrencyException)
//                 //     {
//                 //         if (!ProductExists(product.ProductId))
//                 //         {
//                 //             return NotFound();
//                 //         }
//                 //         else
//                 //         {
//                 //             throw;
//                 //         }
//                 //     }
//                 //     TempData["success"] = "Product updated successfully!";
//                 //     return RedirectToAction(nameof(Index));

//                 var existingProduct = await _context.Products
//         .FirstOrDefaultAsync(p => p.ProductId == id && p.UserId == userId);

//                 if (existingProduct == null)
//                     return NotFound();

//                 existingProduct.Name = product.Name;
//                 existingProduct.CostPrice = product.CostPrice;
//                 existingProduct.SellingPrice = product.SellingPrice;
//                 existingProduct.QuantityInStock = product.QuantityInStock;

//                 await _context.SaveChangesAsync();
//                 TempData["success"] = "Product updated successfully!";
//                 return RedirectToAction(nameof(Index));
//             }
//             return View(product);
//         }

//         // GET: Delete/Product
//         [Authorize]
//         public async Task<IActionResult> Delete(int? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//             var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//             var product = await _context.Products
//                 .FirstOrDefaultAsync(p => p.ProductId == id && p.UserId == userId); if (product == null)
//             {
//                 return NotFound();
//             }
//             return View(product);
//         }

//         // POST: Delete/Product
//         [Authorize]
//         [HttpPost, ActionName("Delete")]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> DeleteAction(int? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//             var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//             var product = await _context.Products
//                 .FirstOrDefaultAsync(p => p.ProductId == id && p.UserId == userId); if (product != null)
//             {
//                 _context.Products.Remove(product);
//             }
//             await _context.SaveChangesAsync();

//             TempData["success"] = "Product deleted successfully!";
//             return RedirectToAction(nameof(Index));
//         }





//         private bool ProductExists(int id)
//         {
//             return _context.Products.Any(e => e.ProductId == id);
//         }
//     }
// }




using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTracker.Data;
using ShopTracker.Models;

namespace ShopTracker.Controllers
{
    [Authorize]
    public class ProductController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: Product
        public async Task<IActionResult> Index(string searchString)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var products = _context.Products
                .Where(p => p.UserId == userId).OrderByDescending(p => p.DateCreated)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var toLower = searchString.ToLower();
                products = products.Where(p => p.Name.ToLower().Contains(toLower));
            }
            ViewData["CurrentFilter"] = searchString;
            return View(await products.ToListAsync());
        }

        // GET: Product/Detail/5
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id && p.UserId == userId);

            if (product == null) return NotFound();

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            product.UserId = userId;
            product.DateCreated = DateTime.UtcNow;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            TempData["success"] = "Product created successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id && p.UserId == userId);

            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int? id, Product product)
        {
            if (id == null || id <= 0)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id && p.UserId == userId);

            if (existingProduct == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                existingProduct.Name = product.Name;
                existingProduct.CostPrice = product.CostPrice;
                existingProduct.SellingPrice = product.SellingPrice;
                existingProduct.QuantityInStock = product.QuantityInStock;

                await _context.SaveChangesAsync();
                TempData["success"] = "Product updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            // Validation failed → return the existing product to the view
            return View(existingProduct);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id && p.UserId == userId);

            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id && p.UserId == userId);

            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            TempData["success"] = "Product deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}