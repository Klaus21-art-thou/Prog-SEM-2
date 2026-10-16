using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayManagementAPI.Models
{
    public class Participant
    {
        [Key]
        public int ParticipantID { get; set; }

        public int UserID { get; set; }
        public int EventID { get; set; }
        public int TrackID { get; set; }
        public int FeeID { get; set; }
        public int? TeamID { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserID")]
        public User User { get; set; } = null!;

        [ForeignKey("EventID")]
        public Event Event { get; set; } = null!;

        [ForeignKey("TrackID")]
        public Track Track { get; set; } = null!;

        [ForeignKey("FeeID")]
        public Fee Fee { get; set; } = null!;

        [ForeignKey("TeamID")]
        public Team? Team { get; set; }

        public Result? Result { get; set; }
    }
}