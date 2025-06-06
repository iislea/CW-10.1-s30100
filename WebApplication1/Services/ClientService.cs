using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class ClientService
{
    private readonly YourDbContext _context;

    public ClientService(YourDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CanDeleteClientAsync(int idClient)
    {
        return !await _context.ClientTrips.AnyAsync(ct => ct.IdClient == idClient);
    }

    public async Task<bool> ClientExistsByPeselAsync(string pesel)
    {
        return await _context.Clients.AnyAsync(c => c.Pesel == pesel);
    }

    public async Task<bool> IsClientInTripAsync(string pesel, int idTrip)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == pesel);
        if (client == null) return false;

        return await _context.ClientTrips.AnyAsync(ct =>
            ct.IdClient == client.IdClient && ct.IdTrip == idTrip);
    }

    public async Task AssignClientToTripAsync(Client newClient, int idTrip, DateTime? paymentDate)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == newClient.Pesel);

        if (client == null)
        {
            _context.Clients.Add(newClient);
            await _context.SaveChangesAsync();
            client = newClient;
        }

        _context.ClientTrips.Add(new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = paymentDate
        });

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }
}
