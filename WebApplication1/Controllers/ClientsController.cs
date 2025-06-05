using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

[ApiController]
[Route("api/clients")]
public class ClientsController : ControllerBase
{
    private readonly YourDbContext _context;

    public ClientsController(YourDbContext context)
    {
        _context = context;
    }

    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        var client = await _context.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == idClient);

        if (client == null)
        {
            return NotFound("Client not found.");
        }

        if (client.ClientTrips.Any())
        {
            return BadRequest("Cannot delete client with assigned trips.");
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();

        return Ok("Client deleted successfully.");
    }
}