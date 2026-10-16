using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayManagementAPI.Models
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }

        [Required]
        public int OrganiserID { get; set; }

        [Required, MaxLength(150)]
        public string EventName { get; set; } = string.Empty;

        public string EventDescription { get; set; } = string.Empty;

        public DateTime EventDate { get; set; }

        [MaxLength(150)]
        public string EventLocation { get; set; } = string.Empty;

        [MaxLength(50)]
        public string EventType { get; set; } = string.Empty; // Run, Walk, Cycle

        [ForeignKey("OrganiserID")]
        public User Organiser { get; set; } = null!;
        public ICollection<Track> Tracks { get; set; } = new List<Track>();
        public ICollection<Participant> Participants { get; set; } = new List<Participant>();
    }
}