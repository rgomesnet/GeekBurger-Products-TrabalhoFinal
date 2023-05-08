using AutoFixture;
using FluentAssertions;
using GeekBurger.Products.Application.GetProduct;
using GeekBurger.Products.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GeekBurger.Products.UnitTests;

public class ProductsControllerUnitTests
{
    private readonly Fixture _fixture;
    private readonly IGetProductService _getProductService;
    private readonly ProductsController _productsController;

    public ProductsControllerUnitTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _productsController = new ProductsController();
        _getProductService = Mock.Of<IGetProductService>();
    }

    [Fact]
    public async Task OnGetProductsByStoreName_WhenListIsEmpty_ShouldReturnNotFound()
    {
        //arrange
        var storeName = "Paulista";

        var products = new List<ProductToGet>();

        Mock.Get(_getProductService)
            .Setup(_ => _.GetProductsByStoreName(It.IsAny<string>()))
            .ReturnsAsync(products);

        var expected = new NotFoundObjectResult("Nenhum dado encontrado");

        //act
        var response = await _productsController.GetProductsByStoreName(storeName, _getProductService);

        //assert            
        Assert.IsType<NotFoundObjectResult>(response);
        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task OnGetProductsByStoreName_WhenListIsNotEmpty_ShouldReturnOneProduct()
    {
        //arrange
        var storeName = "Paulista";
        var products =
            _fixture.Build<ProductToGet>()
                    .With(x => x.Name, storeName)
                    .Without(x => x.Items)
                    .CreateMany(1)
                    .ToList();

        Mock.Get(_getProductService)
           .Setup(_ => _.GetProductsByStoreName(It.IsAny<string>()))
           .ReturnsAsync(products);

        var expected = new OkObjectResult(new List<ProductToGet>
            {
                new ProductToGet{
                StoreId = products.First().StoreId,
                ProductId = products.First().ProductId,
                Image = products.First().Image,
                Name = products.First().Name,
                Price = products.First().Price
            }});

        //act
        var response = await _productsController.GetProductsByStoreName(storeName, _getProductService);

        Assert.IsType<OkObjectResult>(response);
        response.Should().BeEquivalentTo(expected);
    }
}