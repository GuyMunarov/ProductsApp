using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsApp.API.Abstractation;
using ProductsApp.API.Products.Managers;
using ProductsApp.API.Products.Models;

namespace ProductsApp.API.Products;

[Authorize]
public class ProductsController(IProductsManager productsManager) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var products = await productsManager.GetAll(cancellationToken);

        return Ok(products);
    }

    [HttpGet("query")]
    public async Task<IActionResult> Query([FromQuery] ProductQueryRequest queryRequest,
        CancellationToken cancellationToken)
    {
        var products = await productsManager.QueryProducts(
            queryRequest.Name,
            queryRequest.Color,
            queryRequest.CreatedBy,
            cancellationToken);

        return Ok(products);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromQuery] CreateProductRequest queryRequest,
        CancellationToken cancellationToken)
    {
        var product = await productsManager.CreateProduct(
            queryRequest.Name,
            queryRequest.Color,
            cancellationToken);

        return Ok(product);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id,
        CancellationToken cancellationToken)
    {
        var product = await productsManager.GetById(id, cancellationToken);

        return product is not null ? Ok(product) : NotFound();
    }
}