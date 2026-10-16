using System.ComponentModel.DataAnnotations;

namespace RaceDayManagementAPI.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(150)]
        public string UserEmail { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string UserRole { get; set; } = "Participant"; // Organiser or Participant

        public string? UserBio { get; set; }

        public ICollection<Event> OrganizedEvents { get; set; } = new List<Event>();
        public ICollection<Participant> Participations { get; set; } = new List<Participant>();
    }
}