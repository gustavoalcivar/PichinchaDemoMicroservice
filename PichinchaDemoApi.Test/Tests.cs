using System.Text;
using Newtonsoft.Json;
using PichinchaDemoApi.Models;

namespace PichinchaDemoApi.UnitTest;

public class Tests
{
    private string serviceUrl = string.Empty;
    
    [SetUp]
    public void InitialSetup()
    {
        serviceUrl = "http://localhost:3000";
    }

    [Test]
    public async Task ObtenerTodosLosClientes()
    {
        // Arrange
        var httpClient = new HttpClient { BaseAddress = new Uri(serviceUrl) };

        // Act
        var response = await httpClient.GetAsync("/api/Clientes");
        var responseJson = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
        List<Cliente> clientes = JsonConvert.DeserializeObject<List<Cliente>>(responseJson);

        // Assert
        Assert.IsTrue(clientes.Any());
    }

    [Test]
    public async Task InsertarCliente()
    {
        // Arrange
        var httpClient = new HttpClient { BaseAddress = new Uri(serviceUrl) };
        int longitud = 10;
        Guid miGuid = Guid.NewGuid();
        string token = Convert.ToBase64String(miGuid.ToByteArray());
        token = token.Replace("=", "").Replace("+", "");
        Cliente nuevoCliente = new Cliente
        {
            Nombre = "Prueba",
            Genero = "Masculino",
            Edad = 35,
            Identificacion = token.Substring(0, longitud),
            Direccion = "Guayaquil",
            Telefono = "0969696969",
            Contrasena = "pass1234",
            Estado = true
        };

        int numeroClientesAntes;
        int numeroClientesDespues;

        // Act
        var response = await httpClient.GetAsync("/api/Clientes");
        var responseJson = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
        List<Cliente> clientes = JsonConvert.DeserializeObject<List<Cliente>>(responseJson);
        numeroClientesAntes = clientes.Count();

        var request = JsonConvert.SerializeObject(nuevoCliente);
        var content = new StringContent(request, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/api/Clientes", content);
        
        response = await httpClient.GetAsync("/api/Clientes");
        responseJson = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
        clientes = JsonConvert.DeserializeObject<List<Cliente>>(responseJson);
        numeroClientesDespues = clientes.Count();

        // Assert
        Assert.That(numeroClientesDespues, Is.EqualTo(numeroClientesAntes + 1));
    }

    [Test]
    public async Task ObtenerUnaCuenta()
    {
        // Arrange
        var httpClient = new HttpClient { BaseAddress = new Uri(serviceUrl) };

        // Act
        var response = await httpClient.GetAsync("/api/Cuentas/1");
        var responseJson = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
        Cuenta cuenta = JsonConvert.DeserializeObject<Cuenta>(responseJson);

        // Assert
        Assert.IsTrue(cuenta.SaldoInicial >= 0);
    }

    [Test]
    public async Task BorrarCuenta()
    {
        // Arrange
        var httpClient = new HttpClient { BaseAddress = new Uri(serviceUrl) };
        
        // Act
        var response = await httpClient.DeleteAsync("/api/Cuentas/1");
        var responseJson = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
        Cuenta cuenta = JsonConvert.DeserializeObject<Cuenta>(responseJson);

        // Assert
        Assert.IsFalse(cuenta.Estado);
    }

    [Test]
    public async Task NuevaCuentaYMovimiento()
    {
        // Arrange
        var httpClient = new HttpClient { BaseAddress = new Uri(serviceUrl) };
        int longitud = 12;
        Guid miGuid = Guid.NewGuid();
        string token = Convert.ToBase64String(miGuid.ToByteArray());
        token = token.Replace("=", "").Replace("+", "");
        var numeroNuevaCuenta = token.Substring(0, longitud);
        Cuenta nuevaCuenta = new Cuenta
        {
            NumeroCuenta = numeroNuevaCuenta,
            TipoCuenta = "Ahorro",
            SaldoInicial = 0,
            Estado = true,
            IdentificacionCliente = "1716392566"
        };

        // Act
        var request = JsonConvert.SerializeObject(nuevaCuenta);
        var content = new StringContent(request, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/api/Cuentas", content);

        var nuevoMovimiento = new Movimiento
        {
            TipoMovimiento = "Depósito",
            Valor = 100,
            CuentaOrigen = numeroNuevaCuenta
        };

        request = JsonConvert.SerializeObject(nuevoMovimiento);
        content = new StringContent(request, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/api/Movimientos", content);

        var response = await httpClient.GetAsync("/api/Cuentas");
        var responseJson = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
        List<Cuenta> cuentas = JsonConvert.DeserializeObject<List<Cuenta>>(responseJson);
        Cuenta? cuenta = cuentas.LastOrDefault();
        if(cuenta == null) cuenta = new Cuenta();
        
        // Assert
        Assert.IsTrue(cuenta.SaldoInicial == 100);
    }

    [Test]
    public async Task BorrarMovimiento()
    {
        // Arrange
        var httpClient = new HttpClient { BaseAddress = new Uri(serviceUrl) };
        
        int numeroMovimientosAntes;
        int numeroMovimientosDespues;

        // Act
        var response = await httpClient.GetAsync("/api/Movimientos/");
        var responseJson = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
        List<Movimiento> movimientos = JsonConvert.DeserializeObject<List<Movimiento>>(responseJson);
        numeroMovimientosAntes = movimientos.Count();

        await httpClient.DeleteAsync("/api/Movimientos/1");
        
        response = await httpClient.GetAsync("/api/Movimientos");
        responseJson = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
        movimientos = JsonConvert.DeserializeObject<List<Movimiento>>(responseJson);
        numeroMovimientosDespues = movimientos.Count();

        // Assert
        Assert.That(numeroMovimientosDespues, Is.EqualTo(numeroMovimientosAntes - 1));
    }
}