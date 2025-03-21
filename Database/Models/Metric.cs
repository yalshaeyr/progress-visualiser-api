using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgressVisualiserApi.Database.Models
{
    [Table("Metrics", Schema = "PV")]
    public class Metric
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public required string Name { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        [StringLength(255)]
        public string? Unit { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }
    }
}
