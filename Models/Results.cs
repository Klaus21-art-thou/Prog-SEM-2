using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayManagementAPI.Models
{
    public class Result
    {
        [Key]
        public int ResultID { get; set; }

        public int ParticipantID { get; set; }

        public int FinishTimeSeconds { get; set; }

        public int FinishingPosition { get; set; }

        [ForeignKey("ParticipantID")]
        public Participant Participant { get; set; } = null!;
    }
}