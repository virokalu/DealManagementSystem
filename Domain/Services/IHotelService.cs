using DealManagementSystem.Domain.Models;
using DealManagementSystem.Domain.Services.Communication;

namespace DealManagementSystem.Domain.Services;
public interface IHotelService
{
    Task<Response<Hotel>> DeleteAsync(int id);
}