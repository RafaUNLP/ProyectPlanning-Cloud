using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Usuario
{
    [Key] public required string Nombre { get; set; }
    public required string Contraseña { get; set; } //hasheada
}
