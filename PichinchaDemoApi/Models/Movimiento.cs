namespace PichinchaDemoApi.Models;

public class Movimiento
{
    public int MovimientoId { get; set; }
    public DateTime Fecha { get; set; }
    public string TipoMovimiento { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public decimal Saldo { get; set; }
    public string CuentaOrigen { get; set; } = string.Empty;
}