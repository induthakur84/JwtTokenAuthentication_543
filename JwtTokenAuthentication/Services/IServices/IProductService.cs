using JwtTokenAuthentication.DTO;

namespace JwtTokenAuthentication.Services.IServices
{
    public interface IProductService
    {

        Task<ProductResponseDto> GetById(int Id);

        Task<ProductResponseDto> Create(ProductCreateDto productCreateDto);


        Task<ProductResponseDto> Update(ProductUpdateDto productUpdateDto);


        Task<IEnumerable<ProductResponseDto>> GetAll();
        Task<bool> DeleteById(int Id);
    }
}
