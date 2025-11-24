using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class Colaboracion
{
    [Key] public Guid Id { get; set; }
    public required string Descripcion { get; set; } = string.Empty;
    [DefaultValue("")]public required string Proyecto { get; set; } = string.Empty; //el nombre, para cuando se auditen las colaboraciones
    public required CategoriaColaboracion CategoriaColaboracion { get; set; }
    public required long OrganizacionId { get; set; } //de la que forma parte
    public required Guid ProyectoId { get; set; } //de la que forma parte
    public required Guid EtapaId { get; set; } //de la que forma parte
    public long? OrganizacionComprometidaId { get; set; } //tendrá valor cuando alguien se haga cargo de ella --> NO SABEMOS SI SERÁ UN ID
    [Column(TypeName = "timestamp")] public DateTime? FechaRealizacion { get; set; } //será null si no se realizó
    public List<Observacion> Observaciones { get; set; } = [];
    public bool Realizada() => this.FechaRealizacion != null;
}
public enum CategoriaColaboracion{
    Economica = 1, Material = 2, ManoDeObra = 3, Otra = 4
}
