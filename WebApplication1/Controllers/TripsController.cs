using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using WebApplication1.Models;
    using WebApplication1.DTO;

    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly YourDbContext _context;

        public TripsController(YourDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            var trips = await _context.Trips
                .Include(t => t.IdCountries)
                .Include(t => t.ClientTrips)
                .ThenInclude(ct => ct.IdClientNavigation)
                .Select(t => new
                {
                    t.Name,
                    t.Description,
                    t.DateFrom,
                    t.DateTo,
                    t.MaxPeople,
                    Countries = t.IdCountries.Select(c => new
                    {
                        c.Name
                    }),
                    Clients = t.ClientTrips.Select(ct => new
                    {
                        ct.IdClientNavigation.FirstName,
                        ct.IdClientNavigation.LastName
                    })
                })
                .ToListAsync();

            return Ok(trips);
        }
        
        [HttpDelete("/api/clients/{idClient}")]
        public async Task<IActionResult> DeleteClient(int idClient)
        {
            var client = await _context.Clients
                .Include(c => c.ClientTrips)
                .FirstOrDefaultAsync(c => c.IdClient == idClient);

            if (client == null)
            {
                return NotFound("client not found.");
            }

            if (client.ClientTrips.Any())
            {
                return BadRequest("ccannot delete client assigned to a trip.");
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return Ok("client deleted.");
        }
        
        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AssignClientToTrip(int idTrip, [FromBody] ClientAssignRequest request)
        {
            var trip = await _context.Trips
                .Include(t => t.ClientTrips)
                .FirstOrDefaultAsync(t => t.IdTrip == idTrip);

            if (trip == null)
            {
                return NotFound("trip not found.");
            }

            var existingClient = await _context.Clients
                .Include(c => c.ClientTrips)
                .FirstOrDefaultAsync(c => c.Pesel == request.Pesel);

            if (existingClient != null &&
                existingClient.ClientTrips.Any(ct => ct.IdTrip == idTrip))
            {
                return BadRequest("client already assigned to this trip.");
            }

            if (existingClient == null)
            {
                existingClient = new Client
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Telephone = request.Telephone,
                    Pesel = request.Pesel
                };

                _context.Clients.Add(existingClient);
                await _context.SaveChangesAsync();
            }

            var clientTrip = new ClientTrip
            {
                IdClient = existingClient.IdClient,
                IdTrip = trip.IdTrip,
                RegisteredAt = DateTime.Now
            };

            _context.ClientTrips.Add(clientTrip);
            await _context.SaveChangesAsync();

            return Ok("client assigned to trip.");
        }
    }
}
