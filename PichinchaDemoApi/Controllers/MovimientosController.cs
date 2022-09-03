using System;
using Microsoft.AspNetCore.Mvc;
using PichinchaDemoApi.Models;

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
        movimiento.Fecha = DateTime.Now;
        _context.Movimientos.Add(movimiento);
        await _context.SaveChangesAsync();
        return Ok(await _context.Movimientos.ToListAsync());
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