namespace Dsw2026Tpi.Domain.Entities;

public class Appointment : EntityBase
{
    public Guid PatientId { get; private set; }
    public Patient? Patient { get; private set; }

    public Guid DoctorId { get; private set; }
    public Doctor? Doctor { get; private set; }

    public DateTime AppointmentDateTime { get; private set; }
    public string? Reason { get; private set; }
    public bool IsCancelled { get; private set; }

    public Appointment(Guid patientId, Guid doctorId, DateTime appointmentDateTime, string? reason)
    {
        PatientId = patientId;
        DoctorId = doctorId;
        AppointmentDateTime = appointmentDateTime;
        Reason = reason;
        IsCancelled = false;
    }

    public void Update(DateTime appointmentDateTime, string? reason)
    {
        AppointmentDateTime = appointmentDateTime;
        Reason = reason;
    }

    public void Cancel()
    {
        IsCancelled = true;
    }
}