using System;
using Microsoft.AspNetCore.Mvc;
using PichinchaDemoApi.Models;
using System.Linq;
using PichinchaDemoApi.Repository;

namespace PichinchaDemoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovimientosController : ControllerBase
{
    private readonly UnitOfWork unitOfWork;

    public MovimientosController(DataContext context)
    {
        unitOfWork = new UnitOfWork(context);
    }

    [HttpGet]
    public async Task<ActionResult<List<Movimiento>>> ObtenerMovimientos()
    {
        return Ok(await unitOfWork.MovimientoRepository.ObtenerTodos());
    }

    [HttpGet("{movimientoId}")]
    public async Task<ActionResult<Movimiento>> ObtenerMovimiento(int movimientoId)
    {
        var movimiento = await unitOfWork.MovimientoRepository.Obtener(movimientoId);
        if(movimiento == null)
            return BadRequest("Movimiento no encontrado.");
        return Ok(movimiento);
    }

    [HttpPost]
    public async Task<ActionResult<Movimiento>> AgregarMovimiento(Movimiento movimiento)
    {
        var cuentas = await unitOfWork.CuentaRepository.ObtenerTodos();
        var cuentaBuscada = cuentas.FirstOrDefault(c => c.NumeroCuenta == movimiento.CuentaOrigen);
        if(cuentaBuscada == null)
            return BadRequest("Cuenta de origen no encontrada.");

        if(movimiento.Valor < 0 && cuentaBuscada.SaldoInicial == 0)
            return BadRequest("Saldo no disponible.");

        if(movimiento.Valor < 0 && -movimiento.Valor > cuentaBuscada.SaldoInicial)
            return BadRequest("Saldo insuficiente.");

        var movimientos = await unitOfWork.MovimientoRepository.ObtenerTodos();
        
        var totalRetirosCuenta = movimientos
            .Where(m => m.Fecha.ToString("yyyyMMdd") == DateTime.UtcNow.AddHours(-5).ToString("yyyyMMdd")
            && m.CuentaOrigen == cuentaBuscada.NumeroCuenta
            && m.Valor < 0)
            .Sum(m => m.Valor);
        if(totalRetirosCuenta + movimiento.Valor < -1000)
            return BadRequest("Cupo diario exedido.");

        movimiento.Fecha = DateTime.UtcNow.AddHours(-5);
        movimiento.Saldo = cuentaBuscada.SaldoInicial + movimiento.Valor;
        cuentaBuscada.SaldoInicial = movimiento.Saldo;
        await unitOfWork.MovimientoRepository.Agregar(movimiento);
        await unitOfWork.Guardar();
        return Ok(movimiento);
    }

    [HttpPut]
    public async Task<ActionResult<Movimiento>> EditarMovimiento(Movimiento movimiento)
    {
        var movimientoBuscado = await unitOfWork.MovimientoRepository.Obtener(movimiento.MovimientoId);
        if(movimientoBuscado == null)
            return BadRequest("Movimiento no encontrado.");
        movimientoBuscado.Fecha = movimiento.Fecha;
        movimientoBuscado.TipoMovimiento = movimiento.TipoMovimiento;
        movimientoBuscado.Valor = movimiento.Valor;
        movimientoBuscado.Saldo = movimiento.Saldo;
        movimientoBuscado.CuentaOrigen = movimiento.CuentaOrigen;
        await unitOfWork.Guardar();
        return Ok(movimientoBuscado);
    }

    [HttpDelete("{movimientoId}")]
    public async Task<ActionResult<Movimiento>> EliminarMovimiento(int movimientoId)
    {
        var movimiento = await unitOfWork.MovimientoRepository.Obtener(movimientoId);
        if(movimiento == null)
            return BadRequest("Movimiento no encontrado.");
        await unitOfWork.MovimientoRepository.Eliminar(movimientoId);
        await unitOfWork.Guardar();
        return Ok(movimiento);
    }
}