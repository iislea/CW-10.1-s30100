using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Services;


[ApiController]
[Route("api/trips")]
public class TripsController : ControllerBase
{
    private readonly TripService _tripService;
    private readonly ClientService _clientService;

    public TripsController(TripService tripService, ClientService clientService)
    {
        _tripService = tripService;
        _clientService = clientService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _tripService.GetTripsAsync(page, pageSize);
        return Ok(result);
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AssignClient(int idTrip, [FromBody] ClientDto dto)
    {
        if (await _clientService.ClientExistsByPeselAsync(dto.Pesel))
            return BadRequest("Client with this PESEL already exists.");

        if (await _clientService.IsClientInTripAsync(dto.Pesel, idTrip))
            return BadRequest("Client already assigned to this trip.");

        if (!await _tripService.TripExistsAsync(idTrip) || !await _tripService.IsTripInFuture(idTrip))
            return BadRequest("Trip does not exist or has already started.");

        var client = new Client
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Telephone = dto.Telephone,
            Pesel = dto.Pesel
        };

        await _clientService.AssignClientToTripAsync(client, idTrip, dto.PaymentDate);
        return Ok("Client assigned.");
    }
}
