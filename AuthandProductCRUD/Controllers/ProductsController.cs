using AuthandProductCRUD.Model;
using AuthandProductCRUD.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        [HttpGet]
        public ActionResult<List<Product>> GetAll() => _productService.GetAll();

        [HttpGet("{id}")]
        public ActionResult<Product> GetProductById(string id)
        {
            var product = _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]  
        public IActionResult Create([FromBody] Product product)
        {
            var userId = GetUserIdFromToken();
            product.UserId = userId;
            _productService.Create(product);
            return Ok("Product created successfully.");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]  
        public IActionResult Update(string id, [FromBody] Product productIn)
        {
            var existingProduct = _productService.GetById(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            productIn.Id = id;
            productIn.UserId = GetUserIdFromToken();  
            _productService.Update(id, productIn);
            return Ok("Product updated.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]  
        public IActionResult Delete(string id)
        {
            var product = _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            _productService.Delete(id);
            return Ok("Product deleted.");
        }

        private string GetUserIdFromToken()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = identity?.FindFirst(ClaimTypes.Name)?.Value;

            var userService = HttpContext.RequestServices.GetRequiredService<UserService>();
            var user = userService.GetUser(userName, null);  // password null if not needed
            return user?.Id ?? string.Empty;
        }
    }
}
