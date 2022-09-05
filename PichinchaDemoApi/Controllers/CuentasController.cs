using Microsoft.AspNetCore.Mvc;
using PichinchaDemoApi.Models;
using PichinchaDemoApi.Repository;

namespace PichinchaDemoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CuentasController : ControllerBase
{
    private readonly UnitOfWork unitOfWork;

    public CuentasController(DataContext context)
    {
        unitOfWork = new UnitOfWork(context);
    }

    [HttpGet]
    public async Task<ActionResult<List<Cuenta>>> ObtenerCuentas()
    {
        return Ok(await unitOfWork.CuentaRepository.ObtenerTodos());
    }

    [HttpGet("{cuentaId}")]
    public async Task<ActionResult<Cuenta>> ObtenerCuenta(int cuentaId)
    {
        var cuenta = await unitOfWork.CuentaRepository.Obtener(cuentaId);
        if(cuenta == null)
            return BadRequest("Cuenta no encontrada.");
        return Ok(cuenta);
    }

    [HttpPost]
    public async Task<ActionResult<Cuenta>> AgregarCuenta(Cuenta cuenta)
    {
        var cuentas = await unitOfWork.CuentaRepository.ObtenerTodos(c => c.NumeroCuenta == cuenta.NumeroCuenta);
        if(cuentas.Any())
            return BadRequest("Ya existe una cuenta con el mismo número.");
        await unitOfWork.CuentaRepository.Agregar(cuenta);
        await unitOfWork.Guardar();
        return Ok(cuenta);
    }

    [HttpPut]
    public async Task<ActionResult<Cuenta>> EditarCuenta(Cuenta cuenta)
    {
        var cuentaBuscada = await unitOfWork.CuentaRepository.Obtener(cuenta.CuentaId);
        if(cuentaBuscada == null)
            return BadRequest("Cuenta no encontrada.");
        cuentaBuscada.TipoCuenta = cuenta.TipoCuenta;
        cuentaBuscada.SaldoInicial = cuenta.SaldoInicial;
        cuentaBuscada.Estado = cuenta.Estado;
        cuentaBuscada.IdentificacionCliente = cuenta.IdentificacionCliente;
        await unitOfWork.Guardar();
        return Ok(cuentaBuscada);
    }

    [HttpDelete("{cuentaId}")]
    public async Task<ActionResult<Cuenta>> EliminarCuenta(int cuentaId)
    {
        var cuenta = await unitOfWork.CuentaRepository.Obtener(cuentaId);
        if(cuenta == null)
            return BadRequest("Cuenta no encontrada.");
        cuenta.Estado = false;
        await unitOfWork.Guardar();
        return Ok(cuenta);
    }
}