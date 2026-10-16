using System.ComponentModel.DataAnnotations;

namespace RaceDayManagementAPI.Models
{
    public class Team
    {
        [Key]
        public int TeamID { get; set; }

        [Required, MaxLength(100)]
        public string TeamName { get; set; } = string.Empty;

        public string TeamDescription { get; set; } = string.Empty;

        public ICollection<Participant> Participants { get; set; } = new List<Participant>();
    }
}