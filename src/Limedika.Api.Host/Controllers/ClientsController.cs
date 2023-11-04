using Limedika.Contracts;
using Limedika.Services;
using Microsoft.AspNetCore.Mvc;
using static Limedika.Api.Host.Constants;

namespace Limedika.Api.Host.Controllers;

[ApiController]
[Route("v1/api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    /// <summary>
    /// Imports clients
    /// </summary>
    [HttpPost]
    [Produces(JsonContentTypeHeaderValue, Order = 0, Type = typeof(int))]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> ImportClients()
    {
        await _clientService.ImportClients();

        return Ok();
    }

    /// <summary>
    /// Returns all clients
    /// </summary>
    /// <returns>All clients</returns>
    [HttpGet]
    [Produces(JsonContentTypeHeaderValue, Order = 0, Type = typeof(List<Client>))]
    [ProducesResponseType(typeof(List<Client>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClients()
    {
        List<Client> clients = await _clientService.GetClients();

        return Ok(clients);
    }

    /// <summary>
    /// Updates post codes of clients
    /// </summary>
    [HttpPost]
    [Route("post-codes-update")]
    [Produces(JsonContentTypeHeaderValue, Order = 0, Type = typeof(int))]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdatePostCodes()
    {
        await _clientService.UpdatePostCodes();

        return Ok();
    }
}
