using Dsw2026Tpi.Application.Dtos;
using Dsw2026Tpi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2026Tpi.Api.Controllers;


[Route("api/specialities")]
public class SpecialityController : ControllerBase
{
    private readonly ISpecialityService _specialityService;

   
    public SpecialityController(ISpecialityService specialityService)
    {
        _specialityService = specialityService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var specialities = await _specialityService.GetAllAsync();
        return Ok(specialities);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var speciality = await _specialityService.GetByIdAsync(id);

        if (speciality == null)
            return NotFound(new { message = "Especialidad no encontrada" });

        return Ok(speciality);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SpecialityModel.Request model)
    {
        var newSpeciality = await _specialityService.CreateAsync(model);

        // Retorna 201 
        return CreatedAtAction(nameof(GetById), new { id = newSpeciality.Id }, newSpeciality);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SpecialityModel.Request model)
    {
        try
        {
            await _specialityService.UpdateAsync(id, model);
            var updated = await _specialityService.GetByIdAsync(id);
            return Ok(updated);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _specialityService.DeleteAsync(id);
            return Ok("ok");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}