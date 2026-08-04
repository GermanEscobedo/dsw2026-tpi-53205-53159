using Dsw2026Tpi.Application.Dtos;
using Dsw2026Tpi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2026Tpi.Api.Controllers;

[Route("api/appointments")]
[Authorize]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _service;

    public AppointmentController(IAppointmentService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> GetAll([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 1)
    {
        var appointments = await _service.GetAllAsync();
        return Ok(appointments);
    }

    [HttpGet("search")]
    [Authorize(Roles = "Admin,Doctor,Patient")]
    public async Task<IActionResult> Search(
          [FromQuery] int pageSize = 10,
          [FromQuery] int pageIndex = 1,
          [FromQuery] Guid? specialtyId = null,
          [FromQuery] Guid? doctorId = null,
          [FromQuery] string? dni = null,
          [FromQuery] DateTime? date = null)
    {
        var result = await _service.SearchAsync(pageSize, pageIndex, specialtyId, doctorId, dni, date);
        return Ok(result);
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
    [Authorize(Roles = "Patient,Admin")]
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
            var updatedAppointment = await _service.GetByIdAsync(id);
            return Ok(updatedAppointment); 
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
            var canceledAppointment = await _service.GetByIdAsync(id);
            return Ok(canceledAppointment);
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
            await _service.CancelAsync(id);
            return Ok("ok");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}