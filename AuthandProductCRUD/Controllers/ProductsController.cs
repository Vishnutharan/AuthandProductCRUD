using AuthandProductCRUD.Model;
using AuthandProductCRUD.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Security.Claims;

namespace AuthandProductCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // Get All - for authenticated users
        [HttpGet]
        public ActionResult<List<Product>> GetAll() => _productService.GetAll();

        // Get by ID - for authenticated users
        [HttpGet("{id}")]
        public ActionResult<Product> Get(string id)
        {
            var product = _productService.GetById(id);
            return product == null ? NotFound() : product;
        }

        // Create - Admin only
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody] Product product)
        {
            var userId = GetUserIdFromToken();
            product.UserId = userId;
            _productService.Create(product);
            return Ok("Product created successfully.");
        }

        // Update - Admin only
        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult Update(string id, [FromBody] Product productIn)
        {
            var existing = _productService.GetById(id);
            if (existing == null) return NotFound();

            productIn.Id = id;
            productIn.UserId = GetUserIdFromToken(); // Optionally update user
            _productService.Update(id, productIn);
            return Ok("Product updated.");
        }

        // Delete - Admin only
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            var product = _productService.GetById(id);
            if (product == null) return NotFound();

            _productService.Delete(id);
            return Ok("Product deleted.");
        }

        private string GetUserIdFromToken()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = identity?.FindFirst(ClaimTypes.Name)?.Value;

            // You need a service to get user ID from username
            // Example:
            var userService = HttpContext.RequestServices.GetRequiredService<UserService>();
            var user = userService.GetUser(userName, null); // password null if not needed
            return user?.Id ?? string.Empty;
        }
    }
}