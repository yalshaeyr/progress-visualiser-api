using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgressVisualiserApi.Database.Models
{
    [Table("Users", Schema = "PV")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public required string Username { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}