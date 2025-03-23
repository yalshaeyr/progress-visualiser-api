using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgressVisualiserApi.Database.Models
{
    [Table("MetricData", Schema = "PV")]
    public class MetricData
    {
        [Key]
        public int Id { get; set; }

        public double Value { get; set; }

        [Required]
        [ForeignKey("MetricId")]
        public int MetricId { get; set; }

        public DateTimeOffset RecordedAt { get; set; }
    }
}
