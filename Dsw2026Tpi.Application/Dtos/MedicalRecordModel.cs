namespace Dsw2026Tpi.Application.Dtos;

public class MedicalRecordModel
{
    public record Request(
        Guid PatientId,
        Guid DoctorId,
        Guid? AppointmentId,
        DateTime ConsultationDate,
        string Diagnosis,
        string Treatment,
        string? Observations
    );

    public record Response(
        Guid Id,
        Guid PatientId,
        Guid DoctorId,
        Guid? AppointmentId,
        DateTime ConsultationDate,
        string Diagnosis,
        string Treatment,
        string? Observations
    );
}