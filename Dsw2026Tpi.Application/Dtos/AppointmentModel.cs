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
    public record PatientDetailResponse(string Dni, string FullName);
    public record SpecialtyDetailResponse(Guid SpecialtyId, string Name);
    public record DoctorDetailResponse(Guid DoctorId, string Name, SpecialtyDetailResponse Specialty);
    public record SearchItemResponse(Guid AppointmentsId, string AppointmentsStatus, PatientDetailResponse Patient, DoctorDetailResponse Doctor);
    public record SearchResponse(int PageSize, int PageIndex, IEnumerable<SearchItemResponse> Data, int Total);
}