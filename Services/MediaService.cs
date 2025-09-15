using DealManagementSystem.Domain.Models;
using DealManagementSystem.Domain.Services;
using DealManagementSystem.Domain.Services.Communication;

namespace DealManagementSystem.Services;

public class MediaService : IMediaService
{
    public Task<Response<Media>> DeleteAsync(int id, string itemId)
    {
        
        throw new NotImplementedException();  
    }

    public Task<Response<Media>> UpdateAsync(int id, MediaDto mediaDto)
    {
        throw new NotImplementedException();
    }
}