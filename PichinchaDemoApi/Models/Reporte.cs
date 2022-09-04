using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PichinchaDemoApi.Models;

public class Reporte
{
    public string NumeroCuenta { get; set; }
    public string IdentificacionCliente { get; set; }
    public string TipoCuenta { get; set; }
    public bool Estado { get; set; }
    public decimal TotalIngresos { get; set; }
    public decimal TotalEgresos { get; set; }
    public decimal SaldoActual { get; set; }
}
