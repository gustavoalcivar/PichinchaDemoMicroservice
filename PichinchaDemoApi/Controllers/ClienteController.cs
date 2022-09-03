using Microsoft.AspNetCore.Mvc;
using PichinchaDemoApi.Models;

namespace PichinchaDemoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly DataContext _context;

    public ClienteController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Cliente>>> ObtenerClientes()
    {
        return Ok(await _context.Clientes.ToListAsync());
    }

    [HttpGet("{clienteId}")]
    public async Task<ActionResult<Cliente>> ObtenerCliente(int clienteId)
    {
        var cliente = await _context.Clientes.FindAsync(clienteId);
        if(cliente == null)
            return BadRequest("Cliente no encontrado.");
        return Ok(cliente);
    }

    [HttpPost]
    public async Task<ActionResult<List<Cliente>>> AgregarCliente(Cliente cliente)
    {
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return Ok(await _context.Clientes.ToListAsync());
    }

    [HttpPut]
    public async Task<ActionResult<List<Cliente>>> EditarCliente(Cliente cliente)
    {
        var clienteBuscado = await _context.Clientes.FindAsync(cliente.ClienteId);
        if(clienteBuscado == null)
            return BadRequest("Cliente no encontrado.");
        clienteBuscado.Contrasena = cliente.Contrasena;
        clienteBuscado.Estado = cliente.Estado;
        await _context.SaveChangesAsync();
        return Ok(await _context.Clientes.ToListAsync());
    }

    [HttpDelete("{clienteId}")]
    public async Task<ActionResult<List<Cliente>>> EliminarCliente(int clienteId)
    {
        var cliente = await _context.Clientes.FindAsync(clienteId);
        if(cliente == null)
            return BadRequest("Cliente no encontrado.");
        cliente.Estado = false;
        await _context.SaveChangesAsync();
        return Ok(await _context.Clientes.ToListAsync());
    }
}