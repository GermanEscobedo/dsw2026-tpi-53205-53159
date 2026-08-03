using Dsw2026Tpi.Application.Dtos;
using Dsw2026Tpi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2026Tpi.Api.Controllers;


[Route("api/medical-records")]
[Authorize(Roles = "Admin,Doctor")]
public class MedicalRecordController : ControllerBase
{
    private readonly IMedicalRecordService _service;

    public MedicalRecordController(IMedicalRecordService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var records = await _service.GetAllAsync();
        return Ok(records);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var record = await _service.GetByIdAsync(id);

        if (record == null)
            return NotFound(new { message = "Historia clínica no encontrada" });

        return Ok(record);
    }

    [HttpGet("patient/{patientId:guid}")]
    public async Task<IActionResult> GetByPatientId(Guid patientId)
    {
        var records = await _service.GetByPatientIdAsync(patientId);
        return Ok(records);
    }

    [HttpGet("doctor/{doctorId:guid}")]
    public async Task<IActionResult> GetByDoctorId(Guid doctorId)
    {
        var records = await _service.GetByDoctorIdAsync(doctorId);
        return Ok(records);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MedicalRecordModel.Request model)
    {
        try
        {
            var newRecord = await _service.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = newRecord.Id }, newRecord);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] MedicalRecordModel.Request model)
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