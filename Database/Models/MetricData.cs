using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgressVisualiserApi.Database.Models
{
    [Table("MetricData", Schema = "PV")]
    public class MetricData
    {
        [Key]
        public int Id { get; set; }

        public float Value { get; set; }

        [Required]
        public int MetricId { get; set; }

        public DateTime RecordedAt { get; set; }

        [ForeignKey("MetricId")]
        public required Metric Metric { get; set; }
    }
}
