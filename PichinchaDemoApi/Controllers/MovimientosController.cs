using System;
using Microsoft.AspNetCore.Mvc;
using PichinchaDemoApi.Models;
using System.Linq;

namespace PichinchaDemoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovimientosController : ControllerBase
{
    private readonly DataContext _context;

    public MovimientosController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Movimiento>>> ObtenerMovimientos()
    {
        return Ok(await _context.Movimientos.ToListAsync());
    }

    [HttpGet("{movimientoId}")]
    public async Task<ActionResult<Movimiento>> ObtenerMovimiento(int movimientoId)
    {
        var movimiento = await _context.Movimientos.FindAsync(movimientoId);
        if(movimiento == null)
            return BadRequest("Movimiento no encontrado.");
        return Ok(movimiento);
    }

    [HttpPost]
    public async Task<ActionResult<List<Movimiento>>> AgregarMovimiento(Movimiento movimiento)
    {
        var cuentaBuscada = await _context.Cuentas.FirstOrDefaultAsync(c => c.NumeroCuenta == movimiento.CuentaOrigen);
        if(cuentaBuscada == null)
            return BadRequest("Cuenta de origen no encontrada.");

        movimiento.Fecha = DateTime.Now;
        if(movimiento.Valor < 0 && cuentaBuscada.SaldoInicial == 0)
            return BadRequest("Saldo no disponible.");

        if(movimiento.Valor < 0 && -movimiento.Valor > cuentaBuscada.SaldoInicial)
            return BadRequest("Saldo insuficiente.");

        var movimientos = await _context.Movimientos.ToListAsync();
        
        var totalRetirosCuenta = movimientos
            .Where(m => m.Fecha.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd")
            && m.CuentaOrigen == cuentaBuscada.NumeroCuenta
            && m.Valor < 0)
            .Sum(m => m.Valor);
        if(totalRetirosCuenta + movimiento.Valor < -1000)
            return BadRequest("Cupo diario exedido.");

        movimiento.Saldo = cuentaBuscada.SaldoInicial + movimiento.Valor;
        cuentaBuscada.SaldoInicial = movimiento.Saldo;
        _context.Movimientos.Add(movimiento);
        await _context.SaveChangesAsync();
        return Ok(movimiento);
    }

    [HttpPut]
    public async Task<ActionResult<List<Movimiento>>> EditarMovimiento(Movimiento movimiento)
    {
        var movimientoBuscado = await _context.Movimientos.FindAsync(movimiento.MovimientoId);
        if(movimientoBuscado == null)
            return BadRequest("Movimiento no encontrado.");
        movimientoBuscado.TipoMovimiento = movimiento.TipoMovimiento;
        movimientoBuscado.Valor = movimiento.Valor;
        movimientoBuscado.Saldo = movimiento.Saldo;
        await _context.SaveChangesAsync();
        return Ok(await _context.Movimientos.ToListAsync());
    }

    [HttpDelete("{movimientoId}")]
    public async Task<ActionResult<List<Movimiento>>> EliminarMovimiento(int movimientoId)
    {
        var movimiento = await _context.Movimientos.FindAsync(movimientoId);
        if(movimiento == null)
            return BadRequest("Movimiento no encontrado.");
        _context.Movimientos.Remove(movimiento);
        await _context.SaveChangesAsync();
        return Ok(await _context.Movimientos.ToListAsync());
    }
}