using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsApp.API.Abstractation;
using ProductsApp.API.Products.Managers;
using ProductsApp.API.Products.Models;

namespace ProductsApp.API.Products;

[Authorize]
public class ProductsController(
    IProductsManager productsManager, 
    IValidator<UpdateProductRequest> updateRequestValidator,
    IValidator<CreateProductRequest> createRequestValidator) : BaseController
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
    public async Task<IActionResult> Post([FromQuery] CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await createRequestValidator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }
        
        var product = await productsManager.CreateProduct(
            request.Name,
            request.Color,
            cancellationToken);

        return Ok(product);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(
        int id,
        [FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await updateRequestValidator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }
        
        var product = await productsManager.UpdateProduct(
            id,
            request.Name,
            request.Color,
            cancellationToken);

        if (product is null)
        {
            return BadRequest();
        }

        return Ok(product);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await productsManager.DeleteProduct(
            id,
            cancellationToken);

        return result
            ? Ok()
            : BadRequest();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id,
        CancellationToken cancellationToken)
    {
        var product = await productsManager.GetById(id, cancellationToken);

        return product is not null ? Ok(product) : NotFound();
    }
}