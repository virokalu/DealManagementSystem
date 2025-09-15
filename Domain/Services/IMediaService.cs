using DealManagementSystem.Domain.Models;
using DealManagementSystem.Domain.Services.Communication;

namespace DealManagementSystem.Domain.Services;
public interface IMediaService
{
    Task<Response<Media>> UpdateAsync(int id, MediaDto mediaDto);
    Task<Response<Media>> DeleteAsync(int id, string? itemId);
}