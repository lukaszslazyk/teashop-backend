using Microsoft.AspNetCore.Mvc;

namespace Teashop.Backend.UI.Api.Product.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController
    {
        [HttpGet]
        public void GetAllProducts()
        {
            return;
        }
    }
}
