using Dsw2026Tpi.Application.Dtos;
using Dsw2026Tpi.Application.Interfaces;
using Dsw2026Tpi.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2026Tpi.Api.Controllers;

[Route("api/doctor-availabilities")] 
public class DoctorAvailabilityController : ControllerBase
{
    private readonly IDoctorAvailabilityService _service;

    public DoctorAvailabilityController(IDoctorAvailabilityService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var availabilities = await _service.GetAllAsync();
        return Ok(availabilities);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var availability = await _service.GetByIdAsync(id);

        if (availability == null)
            return NotFound(new { message = "Disponibilidad no encontrada" });

        return Ok(availability);
    }

    [HttpGet("doctor/{doctorId:guid}")]
    public async Task<IActionResult> GetByDoctorId(Guid doctorId)
    {
        var availabilities = await _service.GetByDoctorIdAsync(doctorId);
        return Ok(availabilities);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DoctorAvailabilityModel.Request model)
    {
        try
        {
            var newAvailability = await _service.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = newAvailability.Id }, newAvailability);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] DoctorAvailabilityModel.Request model)
    {
        try
        {
            await _service.UpdateAsync(id, model);
            var updated = await _service.GetByIdAsync(id);
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
            await _service.DeleteAsync(id);
            return Ok("ok");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}