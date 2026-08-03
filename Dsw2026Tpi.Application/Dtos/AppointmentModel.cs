namespace Dsw2026Tpi.Application.Dtos;

public class AppointmentModel
{
    public record Request(
        Guid PatientId,
        Guid DoctorId,
        DateTime AppointmentDateTime,
        string? Reason
    );

    public record Response(
        Guid Id,
        Guid PatientId,
        Guid DoctorId,
        DateTime AppointmentDateTime,
        string? Reason,
        bool IsCancelled
    );
}