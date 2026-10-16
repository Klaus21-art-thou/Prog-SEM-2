using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayManagementAPI.Models
{
    public class Track
    {
        [Key]
        public int TrackID { get; set; }

        [Required]
        public int EventID { get; set; }

        [Required, MaxLength(100)]
        public string TrackName { get; set; } = string.Empty;

        public decimal DistanceKM { get; set; }

        [ForeignKey("EventID")]
        public Event Event { get; set; } = null!;
        public ICollection<Fee> Fees { get; set; } = new List<Fee>();
        public ICollection<Participant> Participants { get; set; } = new List<Participant>();
    }
}