using JwtTokenAuthentication.DTO;
using JwtTokenAuthentication.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace JwtTokenAuthentication.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _applicaitonDbContext;

        public ProductService(ApplicationDbContext applicaitonDbContext)
        {
            _applicaitonDbContext = applicaitonDbContext;
        }
        public async Task<ProductResponseDto> Create(ProductCreateDto productCreateDto)
        {
            var product = new Product
            {
                Name = productCreateDto.Name,
                Description = productCreateDto.Description,
                Author = productCreateDto.Author,
            };

            _applicaitonDbContext.Products.Add(product);
            await _applicaitonDbContext.SaveChangesAsync();


            return new ProductResponseDto
            {
                Name = product.Name,
                Description = product.Description,
                Author = product.Author,
            };
        }

        public async Task<bool> DeleteById(int Id)
        {
            var product = await _applicaitonDbContext.Products.FindAsync(Id);

            if (product == null)
            {
                return false;
            }

            _applicaitonDbContext.Products.Remove(product);
            await _applicaitonDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAll()
        {
           return await _applicaitonDbContext.Products.Select(p=>
           new  ProductResponseDto
           {          
               Name=p.Name,
               Description=p.Description,
               Author = p.Author,
           }).ToListAsync();    
        }

        public async Task<ProductResponseDto> GetById(int Id)
        {
            var product = await _applicaitonDbContext.Products.FindAsync(Id);

            if(product == null)
            {
                return null;
            }

            return new ProductResponseDto
            {
                Name = product.Name,
                Description = product.Description,
                Author = product.Author,
            };
        }

        public async Task<ProductResponseDto> Update(ProductUpdateDto productUpdateDto)
        {
            var product= await _applicaitonDbContext.Products.FindAsync(productUpdateDto.Id);

            if(product == null)
            {
                return null;
            }

            product.Name = productUpdateDto.Name;
            product.Description = productUpdateDto.Description;
            product.Author = productUpdateDto.Author;


            await _applicaitonDbContext.SaveChangesAsync();


            return new ProductResponseDto
            {


                Name = product.Name,
                Description = product.Description,
                Author = product.Author,
            };

        }
    }
}
