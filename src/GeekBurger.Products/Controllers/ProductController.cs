using GeekBurger.Products.Application.AddProduct;
using GeekBurger.Products.Application.DeleteProduct;
using GeekBurger.Products.Application.GetProduct;
using Microsoft.AspNetCore.Mvc;

namespace GeekBurger.Products.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : Controller
    {
        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<IActionResult> GetProduct(
            Guid id,
            [FromServices] IGetProductService service)
        {
            try
            {
                if (Guid.Empty.Equals(id))
                {
                    return BadRequest(nameof(id));
                }

                var productToGet = await service.GetProductById(id);
                return Ok(productToGet);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpPost()]
        public async Task<IActionResult> AddProduct(
            [FromBody] ProductToUpsert productToAdd,
            [FromServices] IAddProductService service)
        {
            try
            {

                if (productToAdd is null)
                {
                    return BadRequest(nameof(productToAdd));
                }

                var productToGet = await service.AddProduct(productToAdd);

                return CreatedAtRoute(
                    "GetProduct",
                    new { id = productToGet.ProductId },
                    productToGet);
            }
            catch (ArgumentException)
            {
                return new UnprocessableEntityResult();
            }
        }

        //[HttpPatch("{id}")]
        //public IActionResult PartiallyUpdateProduct(Guid id, [FromBody] JsonPatchDocument<ProductToUpsert> productPatch)
        //{
        //    Product? product;

        //    if (productPatch is null)
        //        return BadRequest();

        //    if (Guid.Empty.Equals(id))
        //        return BadRequest();

        //    product = _productsRepository.GetProductById(id);

        //    if (product is null)
        //        return NotFound();

        //    ProductToUpsert productToUpdate = product;

        //    productPatch.ApplyTo(productToUpdate);

        //    product = _mapper.Map(productToUpdate, product);

        //    if (product.StoreId == Guid.Empty)
        //        return new Helpers.UnprocessableEntityResult(ModelState);

        //    _productsRepository.Update(product);
        //    _productsRepository.Save();

        //    ProductToGet productToGet = product;

        //    return CreatedAtRoute("GetProduct",
        //        new { id = productToGet.ProductId },
        //        productToGet);
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            Guid id,
            [FromServices] IDeleteProductService service)
        {
            try
            {
                await service.DeleteProductById(id);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }
    }
}
