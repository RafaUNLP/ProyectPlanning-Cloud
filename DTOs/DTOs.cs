using System.Net;
using backend.Models;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace backend.DTOs;

public class RespuestaHttp
{
    /// <summary>
    /// Contenido de la respuesta HTTP.
    /// </summary>
    public string? Contenido { get; set; }
    /// <summary>
    /// Código de estado HTTP.
    /// </summary>
    public HttpStatusCode Status { get; set; }
}
public class CrearColaboracionDTO
{
    /// <summary>
    /// Descripción de la colaboración.
    /// </summary>
    public required string Descripcion { get; set; } = string.Empty;
    /// <summary>
    /// Categoría de la colaboración: Economica = 1, Material = 2, ManoDeObra = 3, Otra = 4
    /// </summary>
    public required CategoriaColaboracion CategoriaColaboracion { get; set; }
    /// <summary>
    /// Id de la organización a la que pertenece la colaboración.
    /// </summary>
    public required Guid OrganizacioId { get; set; } //de la que forma parte
    /// <summary>
    /// Id de la etapa a la que pertenece la colaboración.
    /// </summary>
    public required Guid EtapaId { get; set; } //de la que forma parte
}
public class ActualizarColaboracionDTO
{
    /// <summary>
    /// Id de la organización que se compromete a realizar la colaboración.
    /// </summary>
    public Guid? OrganizacionComprometidaId { get; set; }
    /// <summary>
    /// Si se desea marcar la colaboración como realizada o no.
    /// </summary>
    public bool Realizar { get; set; }
}
public class UsuarioDTO
{
    /// <summary>
    /// Nombre único del usuario.
    /// </summary>
    public required string Nombre { get; set; }
    /// <summary>
    /// Contraseña ingresada.
    /// </summary>
    public required string Contraseña { get; set; }
}