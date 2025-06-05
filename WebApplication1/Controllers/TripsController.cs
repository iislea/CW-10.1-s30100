using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
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
        public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var totalTrips = await _context.Trips.CountAsync();
            var totalPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

            var trips = await _context.Trips
                .Include(t => t.IdCountries)
                .Include(t => t.ClientTrips)
                    .ThenInclude(ct => ct.IdClientNavigation)
                .OrderByDescending(t => t.DateFrom)
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
                })
                .ToListAsync();

            return Ok(new
            {
                pageNum = page,
                pageSize = pageSize,
                allPages = totalPages,
                trips = trips
            });
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AssignClientToTrip(int idTrip, [FromBody] ClientAssignRequest request)
        {
            var trip = await _context.Trips
                .Include(t => t.ClientTrips)
                .FirstOrDefaultAsync(t => t.IdTrip == idTrip);

            if (trip == null)
            {
                return NotFound("Trip not found.");
            }

            if (trip.DateFrom <= DateTime.Now)
            {
                return BadRequest("Cannot assign to a trip that already started.");
            }

            if (trip.ClientTrips.Count >= trip.MaxPeople)
            {
                return BadRequest("Trip has reached the maximum number of people.");
            }

            var existingClient = await _context.Clients
                .Include(c => c.ClientTrips)
                .FirstOrDefaultAsync(c => c.Pesel == request.Pesel);

            if (existingClient != null &&
                existingClient.ClientTrips.Any(ct => ct.IdTrip == idTrip))
            {
                return BadRequest("Client already assigned to this trip.");
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
                RegisteredAt = DateTime.Now,
                PaymentDate = request.PaymentDate
            };

            _context.ClientTrips.Add(clientTrip);
            await _context.SaveChangesAsync();

            return Ok("Client assigned to trip.");
        }
    }
}
