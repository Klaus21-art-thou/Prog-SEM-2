using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayManagementAPI.Models
{
    public class Fee
    {
        [Key]
        public int FeeID { get; set; }

        [Required]
        public int TrackID { get; set; }

        [Required, MaxLength(100)]
        public string FeeName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal FeeAmount { get; set; }

        [MaxLength(10)]
        public string FeeCurrency { get; set; } = "ZAR";

        [ForeignKey("TrackID")]
        public Track Track { get; set; } = null!;
        public ICollection<Participant> Participants { get; set; } = new List<Participant>();
    }
}