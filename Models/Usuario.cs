using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace backend.Models;

public class Usuario
{
    [Key] public required string Nombre { get; set; }
    public required string Contraseña { get; set; } //hasheada
}
