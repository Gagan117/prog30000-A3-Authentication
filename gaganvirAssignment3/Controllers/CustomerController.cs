using gaganvirAssignment3.Data;
using gaganvirAssignment3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Index(string sortPrice)
        {
            //https://stackoverflow.com/questions/17366907/what-is-the-purpose-of-asqueryable
            //https://stackoverflow.com/questions/1106802/why-use-asqueryable-instead-of-list
            var products = _context.Products.ToList();

            //https://stackoverflow.com/questions/11912822/how-to-sort-a-list-of-class-in-c
            //https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/sort-filter-page?view=aspnetcore-9.0
            if (sortPrice == "desc")
            {
                products = products.OrderByDescending(p => p.Price).ToList();
            }
            else
            {
                products = products.OrderBy(p => p.Price).ToList();
            }

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

        public IActionResult Checkout()
        {
            var cartItems = Models.Cart.Products;
            decimal totalAmount = cartItems.Sum(p => p.Price);
            decimal tax = totalAmount * 0.13m;
            decimal finalAmount = totalAmount + tax;

            ViewBag.TotalAmount = totalAmount;
            ViewBag.Tax = tax;
            ViewBag.FinalAmount = finalAmount;

            return View(cartItems);
        }

        [HttpPost]
        public IActionResult PlaceOrder()
        {
            var cartItems = Models.Cart.Products;

            if (cartItems.Count == 0)
            {
                return RedirectToAction("Cart");
            }

            decimal totalAmount = cartItems.Sum(p => p.Price);
            decimal tax = totalAmount * 0.13m;
            decimal finalAmount = totalAmount + tax;

            var newOrder = new Order
            {
                OrderId = OrderData.Orders.Count + 1,
                Products = cartItems.ToList(),
                SubTotal = totalAmount,
                Tax = tax,
                TotalAmount = finalAmount,
                OrderDate = DateTime.Now
            };

            OrderData.Orders.Add(newOrder);
            Models.Cart.Products.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult OrderHistory()
        {
            var orders = OrderData.Orders;
            return View(orders);
        }

    }
}
