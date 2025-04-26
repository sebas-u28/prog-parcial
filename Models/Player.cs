using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace prog_parcial.Models
{
    [Table("t_player")]
    public class Player
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [NotNull]
        public string? Nombre { get; set; }
        [NotNull]
        public int Edad { get; set; }
        [NotNull]
        public string? Posicion { get; set; }
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    }
}