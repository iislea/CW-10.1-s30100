using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Services;

public interface ITripService
{
    Task<IEnumerable<TripDto>> GetTripsAsync();
    Task<bool> AssignClientToTripAsync(int idTrip, ClientTripRequestDto request);
}