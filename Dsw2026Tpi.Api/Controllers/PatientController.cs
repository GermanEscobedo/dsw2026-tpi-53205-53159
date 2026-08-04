using Dsw2026Tpi.Application.Dtos;
using Dsw2026Tpi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2026Tpi.Api.Controllers;


[Route("api/patients")]
[Authorize(Roles = "Admin,Doctor,ADMINISTRADOR,Administrador")]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;

    
    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var patients = await _patientService.GetAllAsync();
        return Ok(patients); // Retorna 200 OK
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var patient = await _patientService.GetByIdAsync(id);

        if (patient == null)
            return NotFound(); // Retorna 404 

        return Ok(patient);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PatientModel.Request model)
    {
        var newPatient = await _patientService.CreateAsync(model);

        // Retorna 201 
        return CreatedAtAction(nameof(GetById), new { id = newPatient.Id }, newPatient);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PatientModel.Request model)
    {
        try
        {
            await _patientService.UpdateAsync(id, model);
            var updated = await _patientService.GetByIdAsync(id);
            return Ok(updated); // Retorna 204 
        }
        catch (Exception ex)
        {
            //  devuelve 400
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _patientService.DeleteAsync(id);
            return Ok("ok"); // Retorna 204
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}