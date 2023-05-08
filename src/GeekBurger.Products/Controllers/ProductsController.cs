using GeekBurger.Products.Application.GetProduct;
using Microsoft.AspNetCore.Mvc;

namespace GeekBurger.Products.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public ProductsController()
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetProductsByStoreName(
            [FromQuery] string storeName,
            [FromServices] IGetProductService service)
        {
            var productsByStore = await service.GetProductsByStoreName(storeName);

            if (productsByStore.Count() <= 0)
            {
                return NotFound("Nenhum dado encontrado");
            }

            var productsToGet = productsByStore.Select(p => p as ProductToGet).ToList();

            return Ok(productsToGet);
        }
    }
}
