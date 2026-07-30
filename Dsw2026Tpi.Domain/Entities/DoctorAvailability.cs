namespace Dsw2026Tpi.Domain.Entities;

public class DoctorAvailability : EntityBase
{
    public Guid DoctorId { get; private set; }
    public Doctor? Doctor { get; private set; }

    
    public DayOfWeek DayOfWeek { get; private set; }

    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }

    public DoctorAvailability(Guid doctorId, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
    {
        DoctorId = doctorId;
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
    }

    public void Update(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
    {
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
    }
}