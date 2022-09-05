using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PichinchaDemoApi.Models;
using PichinchaDemoApi.Repository;

namespace PichinchaDemoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly UnitOfWork unitOfWork;

    public ClientesController(DataContext context)
    {
        unitOfWork = new UnitOfWork(context);
    }

    [HttpGet]
    public async Task<ActionResult<List<Cliente>>> ObtenerClientes()
    {
        return Ok(await unitOfWork.ClienteRepository.ObtenerTodos());
    }

    [HttpGet("{clienteId}")]
    public async Task<ActionResult<Cliente>> ObtenerCliente(int clienteId)
    {
        var cliente = await unitOfWork.ClienteRepository.Obtener(clienteId);
        if(cliente == null)
            return BadRequest("Cliente no encontrado.");
        return Ok(cliente);
    }

    [HttpPost]
    public async Task<ActionResult<Cliente>> AgregarCliente(Cliente cliente)
    {
        var clientes = await unitOfWork.ClienteRepository.ObtenerTodos(c => c.Identificacion == cliente.Identificacion);
        if(clientes.Any())
            return BadRequest("Ya existe un cliente con la misma identificación.");
        await unitOfWork.ClienteRepository.Agregar(cliente);
        await unitOfWork.Guardar();
        return Ok(cliente);
    }

    [HttpPut]
    public async Task<ActionResult<Cliente>> EditarCliente(Cliente cliente)
    {
        var clienteBuscado = await unitOfWork.ClienteRepository.Obtener(cliente.ClienteId);
        if(clienteBuscado == null)
            return BadRequest("Cliente no encontrado.");
        clienteBuscado.Nombre = cliente.Nombre;
        clienteBuscado.Genero = cliente.Genero;
        clienteBuscado.Edad = cliente.Edad;
        clienteBuscado.Direccion = cliente.Direccion;
        clienteBuscado.Telefono = cliente.Telefono;
        clienteBuscado.Contrasena = cliente.Contrasena;
        clienteBuscado.Estado = cliente.Estado;
        await unitOfWork.Guardar();
        return Ok(clienteBuscado);
    }

    [HttpDelete("{clienteId}")]
    public async Task<ActionResult<Cliente>> EliminarCliente(int clienteId)
    {
        var cliente = await unitOfWork.ClienteRepository.Obtener(clienteId);
        if(cliente == null)
            return BadRequest("Cliente no encontrado.");
        cliente.Estado = false;
        await unitOfWork.Guardar();
        return Ok(cliente);
    }

    [HttpGet("reporte")]
    public async Task<ActionResult<List<Reporte>>> Reporte(string identificacionCliente, DateTime fechaInicio, DateTime fechaFin)
    {
        var cuentas = await unitOfWork.CuentaRepository.ObtenerTodos();
        var cuentasCliente = cuentas.Where(c => c.IdentificacionCliente == identificacionCliente);
        var movimientos = await unitOfWork.MovimientoRepository.ObtenerTodos();
        var resultado = new List<Reporte>();
        foreach (var cuenta in cuentasCliente)
        {
            var movimientosCuenta = movimientos
                .Where(m => m.CuentaOrigen == cuenta.NumeroCuenta
                && m.Fecha >= fechaInicio && m.Fecha <= fechaFin.AddDays(1));
            var totalIngresos = movimientosCuenta.Where(m => m.Valor > 0).Sum(m => m.Valor);
            var totalEgresos = movimientosCuenta.Where(m => m.Valor < 0).Sum(m => m.Valor);
            var c = new Reporte {
                NumeroCuenta = cuenta.NumeroCuenta,
                IdentificacionCliente = identificacionCliente,
                TipoCuenta = cuenta.NumeroCuenta,
                Estado = cuenta.Estado,
                TotalIngresos = totalIngresos,
                TotalEgresos = totalEgresos,
                SaldoActual = cuenta.SaldoInicial
            };
            resultado.Add(c);
        }
        return Ok(resultado);
    }
}