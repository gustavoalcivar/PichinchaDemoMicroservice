using Microsoft.AspNetCore.Mvc;
using PichinchaDemoApi.Models;

namespace PichinchaDemoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CuentaController : ControllerBase
{
    private readonly DataContext _context;

    public CuentaController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Cuenta>>> ObtenerCuentas()
    {
        return Ok(await _context.Cuentas.ToListAsync());
    }

    [HttpGet("{cuentaId}")]
    public async Task<ActionResult<Cuenta>> ObtenerCuenta(int cuentaId)
    {
        var cuenta = await _context.Cuentas.FindAsync(cuentaId);
        if(cuenta == null)
            return BadRequest("Cuenta no encontrada.");
        return Ok(cuenta);
    }

    [HttpPost]
    public async Task<ActionResult<List<Cuenta>>> AgregarCuenta(Cuenta cuenta)
    {
        _context.Cuentas.Add(cuenta);
        await _context.SaveChangesAsync();
        return Ok(await _context.Cuentas.ToListAsync());
    }

    [HttpPut]
    public async Task<ActionResult<List<Cuenta>>> EditarCuenta(Cuenta cuenta)
    {
        var cuentaBuscada = await _context.Cuentas.FindAsync(cuenta.CuentaId);
        if(cuentaBuscada == null)
            return BadRequest("Cuenta no encontrada.");
        cuentaBuscada.SaldoInicial = cuenta.SaldoInicial;
        cuentaBuscada.Estado = cuenta.Estado;
        await _context.SaveChangesAsync();
        return Ok(await _context.Cuentas.ToListAsync());
    }

    [HttpDelete("{cuentaId}")]
    public async Task<ActionResult<List<Cuenta>>> EliminarCuenta(int cuentaId)
    {
        var cuenta = await _context.Cuentas.FindAsync(cuentaId);
        if(cuenta == null)
            return BadRequest("Cuenta no encontrada.");
        cuenta.Estado = false;
        await _context.SaveChangesAsync();
        return Ok(await _context.Cuentas.ToListAsync());
    }
}