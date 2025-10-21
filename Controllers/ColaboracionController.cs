using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.DTOs;
using backend.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize] //exige token jwt para todos los enpoints (salvo los [AllowAnonymous])
public class ColaboracionController : ControllerBase
{

    private readonly ColaboracionRepository _colaboracionRepository;

    public ColaboracionController(ColaboracionRepository colaboracionRepository)
    {
        _colaboracionRepository = colaboracionRepository;
    }

    /// <summary>
    /// Crea una colaboración.
    /// </summary>
    /// <param name="colaboracionDTO">Descripción, categoría numética, Id de la etapa a la que pertenece, Id de la organización a la que pertenece</param>
    /// <returns>La colaboración creada.</returns>
    /// <response code="200">Creación exitosa.</response>
    /// <response code="409">Ya existiía una colaboración para esa etapa y esa organización.</response>
    /// <response code="500">Error del sistema.</response> 
    /// <remarks>
    /// Ejemplo de request:
    /// 
    ///     POST /Colaboracion
    ///     {
    ///         "descripcion": "Cabar el pozo para la pileta",
    ///         "categoriaColaboracion": 2,
    ///         "organizacioId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///         "etapaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    ///      }
    ///
    /// Ejemplo de response:
    ///
    ///     {
    ///         "id": "f23a945b-5557-4312-bb5a-94eabda06e2d",
    ///         "descripcion": "Cabar el pozo para la pileta",
    ///         "categoriaColaboracion": 2,
    ///         "organizacioId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///         "etapaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///         "organizacionComprometidaId": null,
    ///         "fechaRealizacion": "null
    ///      }
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> CrearColaboracion(CrearColaboracionDTO colaboracionDTO)
    {
        try
        {
            bool yaExiste = await _colaboracionRepository.Exist(c => c.EtapaId == colaboracionDTO.EtapaId && c.OrganizacioId == colaboracionDTO.OrganizacioId);

            if (yaExiste)
                return Conflict("La colaboración para esa organización y etapa ya se encuentra cargada");

            Colaboracion creada = await _colaboracionRepository.AddAsync(new Colaboracion()
            {
                Id = Guid.NewGuid(),
                Descripcion = colaboracionDTO.Descripcion,
                CategoriaColaboracion = colaboracionDTO.CategoriaColaboracion,
                OrganizacioId = colaboracionDTO.OrganizacioId,
                EtapaId = colaboracionDTO.EtapaId
            });

            return Ok(creada);
        }
        catch
        {
            return StatusCode(500, "Falló la carga de la colaboración");
        }
    }
    /// <summary>
    /// Recupera una colaboración.
    /// </summary>
    /// <param name="id">Id de lacolaboración .</param>
    /// <returns>La colaboración recuperada.</returns>
    /// <response code="200">Recuperación exitosa.</response>
    /// <response code="404">Colaboración no encontrada.</response>
    /// <response code="500">Error del sistema.</response> 
    /// <remarks>
    /// Ejemplo de request:
    /// 
    ///     GET /Colaboracion/eyJhbGciOiJIUzI1NiIsInR..."
    ///
    /// Ejemplo de response:
    ///
    ///     {
    ///         "id": "f23a945b-5557-4312-bb5a-94eabda06e2d",
    ///         "descripcion": "Cabar el pozo para la pileta",
    ///         "categoriaColaboracion": 2,
    ///         "organizacioId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///         "etapaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///         "organizacionComprometidaId": null,
    ///         "fechaRealizacion": null
    ///     }
    /// </remarks>

    [HttpGet("{id}")]
    public async Task<IActionResult> RecuperarColaboracion(Guid id)
    {
        try
        {
            Colaboracion? buscado = await _colaboracionRepository.GetAsync(id);

            if (buscado == null)
                return NotFound();

            return Ok(buscado);
        }
        catch
        {
            return StatusCode(500, "Falló la recuperación de la colaboración");
        }
    }

    /// <summary>
    /// Actualizar parte de una colaboración.
    /// </summary>
    /// <param name="id">Id de lacolaboración .</param>
    /// <param name="actualizarColaboracionDTO">Id de la organización que desea comprometerse con ella, indicador de si se desea marcar la colaboración como realizada</param>
    /// <returns>La colaboración actualizada.</returns>
    /// <response code="200">Recuperación exitosa.</response>
    /// <response code="404">Colaboración no encontrada.</response>
    /// <response code="500">Error del sistema.</response> 
    /// <remarks>
    /// Ejemplo de request:
    /// 
    ///     PATCH /Colaboracion/eyJhbGciOiJIUzI1NiIsInR..."
    ///
    /// Ejemplo de response:
    ///
    ///     {
    ///         "id": "f23a945b-5557-4312-bb5a-94eabda06e2d",
    ///         "descripcion": "Cabar el pozo para la pileta",
    ///         "categoriaColaboracion": 2,
    ///         "organizacioId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///         "etapaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///         "organizacionComprometidaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///         "fechaRealizacion": "2025-10-10T13:27:27.240306"
    ///     }
    /// </remarks>
    [HttpPatch("{id}")]
    public async Task<IActionResult> ActualizarColaboracion(Guid id, ActualizarColaboracionDTO actualizarColaboracionDTO)
    {
        try
        {
            if (!actualizarColaboracionDTO.Realizar && actualizarColaboracionDTO.OrganizacionComprometidaId == null)
                return Ok(); //si no hay nada que cambiar, no hago nada

            Colaboracion? buscado = await _colaboracionRepository.GetAsync(id);

            if (buscado == null)
                return NotFound();

            buscado.OrganizacionComprometidaId ??= actualizarColaboracionDTO.OrganizacionComprometidaId; //sólo asigna si estaba en null

            if (actualizarColaboracionDTO.Realizar)
                buscado.FechaRealizacion ??= DateTime.Now; //sólo asigna si estaba en null

            buscado = await _colaboracionRepository.UpdateAsync(buscado, buscado);

            return Ok(buscado);
        }
        catch
        {
            return StatusCode(500, "Falló la recuperación de la colaboración");
        }
    }

    /// <summary>
    /// Recupera todas las colaboraciones asociadas a un proyecto/etapa.
    /// </summary>
    /// <param name="etapaId">Id de la etapa del proyecto.</param>
    /// <returns>Una lista de colaboraciones.</returns>
    /// <response code="200">Recuperación exitosa.</response>
    /// <response code="500">Error del sistema.</response> 
    /// <remarks>
    /// Ejemplo de request:
    /// 
    ///     GET /Colaboracion/proyecto/3fa85f64-5717-4562-b3fc-2c963f66afa6
    ///
    /// Ejemplo de response:
    ///
    ///     [
    ///       {
    ///         "id": "f23a945b-5557-4312-bb5a-94eabda06e2d",
    ///         "descripcion": "Cabar el pozo para la pileta",
    ///         "categoriaColaboracion": 2,
    ///         "organizacioId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///         "etapaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///         "organizacionComprometidaId": null,
    ///         "fechaRealizacion": null
    ///       }
    ///     ]
    /// </remarks>
    [HttpGet("proyecto/{etapaId}")]
    public async Task<IActionResult> RecuperarColaboracionesConProyectoID(Guid etapaId)
    {
        try
        {
            // Se utiliza el método FilterAsync para buscar todas las colaboraciones que coincidan con EtapaId
            IEnumerable<Colaboracion> buscadas = await _colaboracionRepository.FilterAsync(c => c.EtapaId == etapaId);

            // Devuelve la lista (estará vacía si no se encuentra ninguna)
            return Ok(buscadas);
        }
        catch
        {
            return StatusCode(500, "Falló la recuperación de las colaboraciones");
        }
    }
}