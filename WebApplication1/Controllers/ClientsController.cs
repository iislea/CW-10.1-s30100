using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientsController : ControllerBase
{
    private readonly ClientService _clientService;
    private readonly YourDbContext _context;

    public ClientsController(ClientService clientService, YourDbContext context)
    {
        _clientService = clientService;
        _context = context;
    }

    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        if (!await _clientService.CanDeleteClientAsync(idClient))
            return BadRequest("Cannot delete client with active trips.");

        var client = await _context.Clients.FindAsync(idClient);
        if (client == null)
            return NotFound();

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return Ok("Client deleted.");
    }
}
