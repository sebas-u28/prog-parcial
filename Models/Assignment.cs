using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace prog_parcial.Models
{
    [Table("t_assignment")]
    public class Assignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Team")]
        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;

        [ForeignKey("Player")]
        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;
    }
}