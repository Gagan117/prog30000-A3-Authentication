using gaganvirAssignment3.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gaganvirAssignment3.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult Cart()
        {
            var cartProducts = Models.Cart.Products;
            return View(cartProducts);
        }

        public IActionResult AddToCart(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            Models.Cart.Products.Add(product);
            return RedirectToAction("Cart");
        }

        public IActionResult RemoveFromCart(int id)
        {
            var product = Models.Cart.Products.FirstOrDefault(p => p.ProductId == id);
            if (product != null)
            {
                Models.Cart.Products.Remove(product);
            }
            return RedirectToAction("Cart");
        }
    }
}
