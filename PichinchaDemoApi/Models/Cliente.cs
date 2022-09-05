namespace PichinchaDemoApi.Models;

public class Cliente : Persona
{
    public string Contrasena { get; set; } = string.Empty;
    public bool Estado { get; set; }

}