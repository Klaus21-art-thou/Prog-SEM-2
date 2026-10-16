namespace RaceDayManagementAPI.DTOs
{
    public record RegisterDto(string Username, string UserEmail, string Password, string UserRole, string? UserBio);
    public record LoginDto(string UserEmail, string Password);
    public record UserProfileDto(int UserID, string Username, string UserEmail, string UserRole, string? UserBio);

    public record CreateEventDto(string EventName, string EventDescription, DateTime EventDate, string EventLocation, string EventType);
    public record CreateTrackDto(int EventID, string TrackName, decimal DistanceKM);
    public record CreateFeeDto(int TrackID, string FeeName, decimal FeeAmount, string FeeCurrency);
    public record CreateTeamDto(string TeamName, string TeamDescription);
    public record EnrollParticipantDto(int EventID, int TrackID, int FeeID, int? TeamID);
    public record RecordResultDto(int ParticipantID, int FinishTimeSeconds, int FinishingPosition);
}