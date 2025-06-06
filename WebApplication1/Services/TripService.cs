using WebApplication1.Data;
using WebApplication1.DTO;

namespace WebApplication1.Services;

using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

public class TripService : ITripService
{
    private readonly YourDbContext _context;

    public TripService(YourDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TripDto>> GetTripsAsync()
    {
        return await _context.Trips
            .Include(t => t.ClientTrips)
                .ThenInclude(ct => ct.IdClientNavigation)
            .Include(t => t.IdCountries)
            .Select(t => new TripDto
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(c => c.Name).ToList(),
                Clients = t.ClientTrips.Select(ct => new ClientDto
                {
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName
                }).ToList()
            }).ToListAsync();
    }

    public async Task<bool> AssignClientToTripAsync(int idTrip, ClientTripRequestDto request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Pesel == request.Pesel);

            if (client == null)
            {
                client = new Client
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Telephone = request.Telephone,
                    Pesel = request.Pesel
                };

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
            }

            var trip = await _context.Trips
                .Include(t => t.ClientTrips)
                .FirstOrDefaultAsync(t => t.IdTrip == idTrip);

            if (trip == null || trip.ClientTrips.Any(ct => ct.IdClient == client.IdClient))
                return false;

            var newClientTrip = new ClientTrip
            {
                IdClient = client.IdClient,
                IdTrip = idTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = request.PaymentDate
            };

            _context.ClientTrips.Add(newClientTrip);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}