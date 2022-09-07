using System;
using NUnit.Framework;
using NUnit;
using Microsoft.Extensions.Configuration;

namespace PichinchaDemoApi.UnitTest;

public class ClientesTest
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

        // Assert
        Assert.IsTrue(true);
    }
}