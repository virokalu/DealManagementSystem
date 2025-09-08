using DealManagementSystem.Domain.Models;
using DealManagementSystem.Domain.Services.Communication;

namespace DealManagementSystem.Domain.Services;

public interface IDealService
{
  Task<IEnumerable<Deal>> ListAsync();
  Task<Response<Deal>> SaveAsync(Deal deal, IFormFile? imageFile);
  Task<Response<Deal>> FindByIdAsync(int id);
  Task<Response<Deal>> FindBySlugAsync(string slug);
  Task<Response<Deal>> UpdateAsync(int id, Deal deal, IFormFile? imageFile);
  Task<Response<Deal>> DeleteAsync(int id);
}

