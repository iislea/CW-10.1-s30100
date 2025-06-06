using WebApplication1.Data;
using WebApplication1.DTO;

namespace WebApplication1.Services;

using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

public class TripService
{
    private readonly YourDbContext _context;

    public TripService(YourDbContext context)
    {
        _context = context;
    }

    public async Task<object> GetTripsAsync(int page, int pageSize)
    {
        var query = _context.Trips
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .Include(t => t.IdCountries)
            .OrderByDescending(t => t.DateFrom);

        var totalTrips = await query.CountAsync();
        var allPages = (int)Math.Ceiling((double)totalTrips / pageSize);

        var trips = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new
            {
                t.Name,
                t.Description,
                t.DateFrom,
                t.DateTo,
                t.MaxPeople,
                Countries = t.IdCountries.Select(c => new { c.Name }),
                Clients = t.ClientTrips.Select(ct => new
                {
                    ct.IdClientNavigation.FirstName,
                    ct.IdClientNavigation.LastName
                })
            }).ToListAsync();

        return new
        {
            pageNum = page,
            pageSize,
            allPages,
            trips
        };
    }

    public async Task<bool> TripExistsAsync(int idTrip)
    {
        return await _context.Trips.AnyAsync(t => t.IdTrip == idTrip);
    }

    public async Task<bool> IsTripInFuture(int idTrip)
    {
        var trip = await _context.Trips.FirstOrDefaultAsync(t => t.IdTrip == idTrip);
        return trip != null && trip.DateFrom > DateTime.Now;
    }
}
