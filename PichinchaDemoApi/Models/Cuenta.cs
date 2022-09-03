namespace PichinchaDemoApi.Models;

public class Cuenta
{
    public int CuentaId { get; set; }
    public string NumeroCuenta { get; set; } = string.Empty;
    public string TipoCuenta { get; set; } = string.Empty;
    public decimal SaldoInicial { get; set; }
    public bool Estado { get; set; }
    public string IdentificacionCliente { get; set; }
}