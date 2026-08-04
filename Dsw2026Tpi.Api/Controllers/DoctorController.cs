using Dsw2026Tpi.Application.Dtos;
using Dsw2026Tpi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2026Tpi.Api.Controllers;


[Route("api/doctors")]
[Authorize]
public class DoctorController : AppController
{
   
    private readonly IDoctorService _doctorService;

  
    public DoctorController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var doctors = await _doctorService.GetAllAsync();
        return Ok(doctors);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var doctor = await _doctorService.GetByIdAsync(id);

        if (doctor == null)
            return NotFound(new { message = "Médico no encontrado" });

        return Ok(doctor);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Administrador,ADMINISTRADOR")]
    public async Task<IActionResult> Create([FromBody] DoctorModel.Request model)
    {
        var newDoctor = await _doctorService.CreateAsync(model);

        return CreatedAtAction(nameof(GetById), new { id = newDoctor.Id }, newDoctor);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] DoctorModel.Request model)
    {
        try
        {
            await _doctorService.UpdateAsync(id, model);
            var updated = await _doctorService.GetByIdAsync(id);
            return Ok(updated);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _doctorService.DeleteAsync(id);
            return Ok("ok");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}