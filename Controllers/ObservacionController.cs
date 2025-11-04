using Microsoft.AspNetCore.Mvc;
using backend.DTOs;
using backend.Repositories;
using backend.Models;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class ObservacionController : ControllerBase{

    private readonly ObservacionRepository _observacionRepository;
    private readonly ColaboracionRepository _colaboracionRepository;

    public ObservacionController(ObservacionRepository observacionRepository, ColaboracionRepository colaboracionRepository)
    {
        _observacionRepository = observacionRepository;
        _colaboracionRepository = colaboracionRepository;
    }

    /// <summary>
    /// Carga las observaciones si encuentra el id de colaboración indicado.
    /// </summary>
    /// <param name="observacionesDTO">Observaciones a cargar<.</param>
    /// <returns>Las colaboraciones que se cargaron exitosamente</returns>
    /// <response code="200">Carga exitosa.</response>
    /// <response code="500">Error del sistema.</response> 
    /// <remarks>
    /// Ejemplo de request:
    /// 
    ///     POST /Observacion
    ///        [
    ///            {
    ///                "ColaboracionId": "d2c7f63e-e34f-4701-a029-f8a41012946b",
    ///                "Descripcion": "Seguramente se requieran más personas de las solicitadas para realizar la colaboración"
    ///            },
    ///            {
    ///                "ColaboracionId": "3e56e782-3511-475d-bdff-04a5e8330ea9",
    ///                "Descripcion": "Esta colaboración está muy atrasada, por favor termínenla para poder continuar"
    ///            }
    ///        ]
    ///
    /// Ejemplo de response:
    ///        [
    ///            {
    ///                "Id": "cfcb72c3-2b95-4c76-b27c-0415f5bbd00d",
    ///                "ColaboracionId": "d2c7f63e-e34f-4701-a029-f8a41012946b",
    ///                "Descripcion": "Seguramente se requieran más personas de las solicitadas para realizar la colaboración"
    ///                "FechaCarga": "2025-11-04T12:34:56",
    ///                "FechaRealizacion": null
    ///            },
    ///            {
    ///                "Id": "9634c890-01f1-4960-9bfc-7366c6c3677a",
    ///                "ColaboracionId": "3e56e782-3511-475d-bdff-04a5e8330ea9",
    ///                "Descripcion": "Esta colaboración está muy atrasada, por favor termínenla para poder continuar"
    ///                "FechaCarga": "2025-11-04T12:34:56",
    ///                "FechaRealizacion": null
    ///            }
    ///        ]
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> CrearObservaciones(List<ObservacionDTO> observacionesDTO)
    {
        try
        {
            bool colaboracionExistente;
            List<Observacion> cargadas = [];
            foreach (ObservacionDTO observacion in observacionesDTO)
            {
                colaboracionExistente = await _colaboracionRepository.Exist(c => c.Id == observacion.ColaboracionId);
                if (colaboracionExistente)
                {
                    cargadas.Add(await _observacionRepository.AddAsync(new Observacion() {
                        Id = Guid.NewGuid(),
                        ColaboracionId = observacion.ColaboracionId,
                        Descripcion = observacion.Descripcion,
                        FechaCarga = DateTime.Now
                    }));
                }
            }

           return Ok(cargadas);
        }
        catch
        {
            return StatusCode(500, "Falló la carga de observaciones");
        }
    }
}