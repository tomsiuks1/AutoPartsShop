using API.Extentions;
using Application.Catalog.Cars.Makers;
using Application.Catalog.Cars.Models;
using Application.Catalog.Item;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models.RequestHelpers;
using Models.Catalog;
using Microsoft.AspNetCore.Authorization;
using Persistence;
using AutoMapper;
using Models.DTOs;

namespace API.Controllers
{
    public class CatalogController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public CatalogController(IMediator mediator, DataContext dataContext)
        {
            _mediator = mediator;
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<CatalogItem>>> GetProducts([FromQuery] CatalogParams catalogParams)
        {
            var catalogItems = await _mediator.Send(new GetCatalogItems.Query(catalogParams));
            Response.AddPaginationHeader(catalogItems.MetaData);

            return catalogItems;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CatalogItem>> CreateProduct([FromForm] CreateCatalogItemDto productDto)
        {
            var product = _mapper.Map<CatalogItem>(productDto);

         /*   if (productDto.File != null)
            {
                var imageResult = await _imageService.AddImageAsync(productDto.File);

                if (imageResult.Error != null) return BadRequest(new ProblemDetails
                {
                    Title = imageResult.Error.Message
                });

                product.PictureUrl = imageResult.SecureUrl.ToString();
                product.PublicId = imageResult.PublicId;
            }*/

            _dataContext.CatalogItems.Add(product);

            var result = await _dataContext.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetProduct", new { Id = product.Id }, product);

            return BadRequest(new ProblemDetails { Title = "Problem creating new product" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<CatalogItem>> UpdateProduct([FromForm] UpdateCatalogItemDto productDto)
        {
            var product = await _dataContext.CatalogItems.FindAsync(productDto.Id);

            if (product == null) return NotFound();

            _mapper.Map(productDto, product);

            _mapper.Map(productDto, product);

            /*if (productDto.File != null)
            {
                var imageUploadResult = await _imageService.AddImageAsync(productDto.File);

                if (imageUploadResult.Error != null)
                    return BadRequest(new ProblemDetails { Title = imageUploadResult.Error.Message });

                if (!string.IsNullOrEmpty(product.PublicId))
                    await _imageService.DeleteImageAsync(product.PublicId);

                product.PictureUrl = imageUploadResult.SecureUrl.ToString();
                product.PublicId = imageUploadResult.PublicId;
            }*/

            var result = await _dataContext.SaveChangesAsync() > 0;

            if (result) return Ok(product);

            return BadRequest(new ProblemDetails { Title = "Problem updating product" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _dataContext.CatalogItems.FindAsync(id);

            if (product == null) return NotFound();

            /*if (!string.IsNullOrEmpty(product.PublicId))
                await _imageService.DeleteImageAsync(product.PublicId);*/

            _dataContext.CatalogItems.Remove(product);

            var result = await _dataContext.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting product" });
        }

        [HttpGet("filters")]
        public async Task<ActionResult<CatalogItemFilter>> GetFilters()
        {
            return await _mediator.Send(new GetCatalogFilters.Query());
        }

        [HttpGet, Route("car/makers")]
        public async Task<ActionResult<List<CarMaker>>> GetMakers()
        {
            return await _mediator.Send(new GetCarMakers.Query());
        }

        [HttpGet, Route("car/models")]
        public async Task<ActionResult<List<CarModel>>> GetModels()
        {
            return await _mediator.Send(new GetCarModels.Query());
        }
    }
}
