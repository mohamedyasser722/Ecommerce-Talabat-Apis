using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Interfaces;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this.mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Pagination<ProductToReturnDto>>>> GetProducts([FromQuery] ProductSpecParams productSpec)
        {
            //var Products = await productRepo.GetAllAsync();
            var Spec = new ProductSpecifications(productSpec);
            var Products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(Spec);
            var mapppedData = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(Products);
            var CountSpec = new ProductWithFilterationForCountSpecification(productSpec);
            var Count = await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(CountSpec);
            return Ok(new Pagination<ProductToReturnDto>(productSpec.PagIndex, productSpec.PageSize, Count, mapppedData));
        }

        [HttpGet("{id}"), ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK), ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            //var Product = await productRepo.GetByIdAsync(id);
            var Spec = new ProductSpecifications(id);
            var Product = await _unitOfWork.Repository<Product>().GetByIdWithSpecAsync(Spec);
            if (Product == null)
                return NotFound(new ApiErrorResponse(404));
            var ProductDto = mapper.Map<Product, ProductToReturnDto>(Product);
            return Ok(ProductDto);
        }

        [HttpGet("brands")] //api/products/brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrands()
        {
            var Brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(Brands);
        }

        [HttpGet("categories")] //api/products/categories
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetAllCategories()
        {
            var Categories = await _unitOfWork.Repository<ProductCategory>().GetAllAsync();
            return Ok(Categories);
        }
    }
}
