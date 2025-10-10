using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace backend.Models;

public class Colaboracion
{
    [Key] public Guid Id { get; set; }
    public required string Descripcion { get; set; } = string.Empty;
    public required CategoriaColaboracion CategoriaColaboracion { get; set; }
    public required Guid OrganizacioId { get; set; } //de la que forma parte
    public required Guid EtapaId { get; set; } //de la que forma parte
    public Guid? OrganizacionComprometidaId { get; set; } //tendrá valor cuando alguien se haga cargo de ella --> NO SABEMOS SI SERÁ UN ID
    [Column(TypeName = "timestamp")] public DateTime? FechaRealizacion { get; set; } //será null si no se realizó
    public bool Realizada() => this.FechaRealizacion != null;
}
public enum CategoriaColaboracion{
    Economica = 1, Material = 2, ManoDeObra = 3, Otra = 4
}
