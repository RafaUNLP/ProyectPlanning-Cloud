using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.DTOs;
using backend.Repositories;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize] //exige token jwt para todos los enpoints (salvo los [AllowAnonymous])
public class ColaboracionController : ControllerBase{

    private readonly ColaboracionRepository _colaboracionRepository;

    public ColaboracionController (ColaboracionRepository colaboracionRepository){
        _colaboracionRepository = colaboracionRepository;
    }

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

    //rasociar a una org o realizarla, o ambas
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
}