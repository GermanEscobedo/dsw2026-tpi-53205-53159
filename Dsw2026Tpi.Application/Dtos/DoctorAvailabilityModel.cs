namespace Dsw2026Tpi.Application.Dtos;

public class DoctorAvailabilityModel
{
    
    public record Request(
        Guid DoctorId,
        DayOfWeek DayOfWeek,
        TimeSpan StartTime,
        TimeSpan EndTime
    );

    public record Response(
        Guid Id,
        Guid DoctorId,
        DayOfWeek DayOfWeek,
        TimeSpan StartTime,
        TimeSpan EndTime
    );
}