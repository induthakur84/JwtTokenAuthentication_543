using JwtTokenAuthentication.DTO;
using JwtTokenAuthentication.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtTokenAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Create(ProductCreateDto productCreateDto)
        {
            var result= await _productService.Create(productCreateDto);
            return Ok(result);
        }

        [HttpGet("GetAll")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAll()
        {
            var result= await _productService.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productService.GetById(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpPut]
        [Authorize(Roles = "mainuser")]
        public async Task<IActionResult> Update(ProductUpdateDto productUpdateDto)
        {
            var result = await _productService.Update(productUpdateDto);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result= await _productService.DeleteById(id);
            return Ok(result);
        }
    }
}
