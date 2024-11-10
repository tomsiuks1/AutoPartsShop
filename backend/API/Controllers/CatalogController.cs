using API.Extentions;
using Application.Catalog.Item;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models.RequestHelpers;
using Models.Catalog;
using Microsoft.AspNetCore.Authorization;
using Persistence;
using AutoMapper;
using Models.DTOs;

namespace API.Controllers.old
{
    public class CatalogOldController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public CatalogOldController(IMediator mediator, DataContext dataContext)
        {
            _mediator = mediator;
            _dataContext = dataContext;
        }

        // [HttpGet]
        // public async Task<ActionResult<PagedList<CatalogItem>>> GetProducts([FromQuery] CatalogParams catalogParams)
        // {
        //     var catalogItems = await _mediator.Send(new GetCatalogItems.Query(catalogParams));
        //     Response.AddPaginationHeader(catalogItems.MetaData);

        //     return catalogItems;
        // }

        // [Authorize(Roles = "Admin")]
        // [HttpPost]
        // public async Task<ActionResult<CatalogItem>> CreateProduct([FromForm] CreateCatalogItemDto productDto)
        // {
        //     var product = _mapper.Map<CatalogItem>(productDto);

        //     _dataContext.CatalogItems.Add(product);

        //     var result = await _dataContext.SaveChangesAsync() > 0;

        //     if (result) return CreatedAtRoute("GetProduct", new { Id = product.Id }, product);

        //     return BadRequest(new ProblemDetails { Title = "Problem creating new product" });
        // }

        // [Authorize(Roles = "Admin")]
        // [HttpPut]
        // public async Task<ActionResult<CatalogItem>> UpdateProduct([FromForm] UpdateCatalogItemDto productDto)
        // {
        //     var product = await _dataContext.CatalogItems.FindAsync(productDto.Id);

        //     if (product == null) return NotFound();

        //     _mapper.Map(productDto, product);

        //     _mapper.Map(productDto, product);

        //     var result = await _dataContext.SaveChangesAsync() > 0;

        //     if (result) return Ok(product);

        //     return BadRequest(new ProblemDetails { Title = "Problem updating product" });
        // }

        // [Authorize(Roles = "Admin")]
        // [HttpDelete("{id}")]
        // public async Task<ActionResult> DeleteProduct(Guid id)
        // {
        //     var product = await _dataContext.CatalogItems.FindAsync(id);

        //     if (product == null) return NotFound();

        //     _dataContext.CatalogItems.Remove(product);

        //     var result = await _dataContext.SaveChangesAsync() > 0;

        //     if (result) return Ok();

        //     return BadRequest(new ProblemDetails { Title = "Problem deleting product" });
        // }

        // [HttpGet("filters")]
        // public async Task<ActionResult<CatalogItemFilter>> GetFilters()
        // {
        //     return await _mediator.Send(new GetCatalogFilters.Query());
        // }
    }
}
