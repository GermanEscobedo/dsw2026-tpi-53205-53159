namespace Dsw2026Tpi.Domain.Entities;

public class MedicalRecord : EntityBase
{
    public Guid PatientId { get; private set; }
    public Patient? Patient { get; private set; }

    public Guid DoctorId { get; private set; }
    public Doctor? Doctor { get; private set; }

    public Guid? AppointmentId { get; private set; }
    public Appointment? Appointment { get; private set; }

    public DateTime ConsultationDate { get; private set; }
    public string Diagnosis { get; private set; } = string.Empty;
    public string Treatment { get; private set; } = string.Empty;
    public string? Observations { get; private set; }

    public MedicalRecord(Guid patientId, Guid doctorId, Guid? appointmentId, DateTime consultationDate, string diagnosis, string treatment, string? observations)
    {
        PatientId = patientId;
        DoctorId = doctorId;
        AppointmentId = appointmentId;
        ConsultationDate = consultationDate;
        Diagnosis = diagnosis;
        Treatment = treatment;
        Observations = observations;
    }

    public void Update(string diagnosis, string treatment, string? observations)
    {
        Diagnosis = diagnosis;
        Treatment = treatment;
        Observations = observations;
    }
}