using APIsMainProject.Dtos;
using APIsMainProject.Helper;
using APIsMainProject.ResponseModule;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace APIsMainProject.Controllers
{
    public class ProductsController : BaseController
    {
  
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductsController
            (
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
            {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            }
        [HttpGet("GetProducts")]
        [Cashed(100)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]                 // Just For Documentation
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]           // Just For Documentation
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]         // Just For Documentation
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]// Just For Documentation
        public async Task<ActionResult< Pagination<ProductDto>>> GetProducts([FromQuery]ProductSpecificationParams specificationParams)
        {
            var specification = new ProductsWithBrandAndTypeSpecification(specificationParams);
            var CountSpecification = new ProductWithFiltersForCountSpecifications(specificationParams);
            var TotalCount = await unitOfWork.Repository<Product>().CountAsync(CountSpecification);
            var products =  await unitOfWork.Repository<Product>().ListAsync(specification);
            var Mappedproducts = mapper.Map< IReadOnlyList < ProductDto >> (products);
            var PaginatedData = new Pagination<ProductDto>(specificationParams.PageIndex,
                                                           specificationParams.PageSize,
                                                           TotalCount,Mappedproducts);
            return Ok(PaginatedData);
        }

        [HttpGet("GetProduct")]                                                              
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var specification = new ProductsWithBrandAndTypeSpecification(id);
            var product = await unitOfWork.Repository<Product>().GetEntityBySpecifications(specification);
            if (product is null)
                return NotFound(new ApiResponse(404));
            var Mappedproducts = mapper.Map<ProductDto>(product);
            return Ok(Mappedproducts);
        }

        [HttpGet("GetProductBrands")]
        [Cashed(100)]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
       => Ok(await unitOfWork.Repository<ProductBrand>().GetAllAsync());

        [HttpGet("GetProductTypes")]
        [Cashed(100)]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
       => Ok(await unitOfWork.Repository<ProductType>().GetAllAsync());
    }
}
