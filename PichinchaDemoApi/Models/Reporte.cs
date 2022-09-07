namespace PichinchaDemoApi.Models;

public class Reporte
{
    public string NumeroCuenta { get; set; } = string.Empty;
    public string IdentificacionCliente { get; set; } = string.Empty;
    public string TipoCuenta { get; set; } = string.Empty;
    public bool Estado { get; set; }
    public decimal TotalIngresos { get; set; }
    public decimal TotalEgresos { get; set; }
    public decimal SaldoActual { get; set; }
}
