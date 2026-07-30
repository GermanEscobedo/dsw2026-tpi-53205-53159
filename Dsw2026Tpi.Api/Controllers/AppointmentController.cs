using Dsw2026Tpi.Application.Dtos;
using Dsw2026Tpi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2026Tpi.Api.Controllers;


[Route("api/appointments")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _service;

    public AppointmentController(IAppointmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var appointments = await _service.GetAllAsync();
        return Ok(appointments);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var appointment = await _service.GetByIdAsync(id);

        if (appointment == null)
            return NotFound(new { message = "Turno no encontrado" });

        return Ok(appointment);
    }

    [HttpGet("patient/{patientId:guid}")]
    public async Task<IActionResult> GetByPatientId(Guid patientId)
    {
        var appointments = await _service.GetByPatientIdAsync(patientId);
        return Ok(appointments);
    }

    [HttpGet("doctor/{doctorId:guid}")]
    public async Task<IActionResult> GetByDoctorId(Guid doctorId)
    {
        var appointments = await _service.GetByDoctorIdAsync(doctorId);
        return Ok(appointments);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AppointmentModel.Request model)
    {
        try
        {
            var newAppointment = await _service.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = newAppointment.Id }, newAppointment);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] AppointmentModel.Request model)
    {
        try
        {
            await _service.UpdateAsync(id, model);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        try
        {
            await _service.CancelAsync(id);
            return NoContent();
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
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}