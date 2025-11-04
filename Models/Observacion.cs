using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class Observacion
{
    [Key] public Guid Id { get; set; }
    public required string Descripcion { get; set; } = string.Empty;
    public required Guid ColaboracionId { get; set; }
    [Column(TypeName = "timestamp")] public DateTime FechaCarga { get; set; } = DateTime.Now;
    [Column(TypeName = "timestamp")] public DateTime? FechaRealizacion { get; set; } //será null si no se realizó
    public bool Realizada() => this.FechaRealizacion != null;
}