using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComandesAPI.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nom de l'article és obligatori")]
        [StringLength(100, ErrorMessage = "El nom no pot excedir 100 caràcters")]
        public string Nom { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripció no pot excedir 500 caràcters")]
        public string? Descripcio { get; set; }

        [Required(ErrorMessage = "El preu és obligatori")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El preu ha de ser superior a 0")]
        public decimal Preu { get; set; }

        [Required(ErrorMessage = "L'estoc és obligatori")]
        [Range(0, int.MaxValue, ErrorMessage = "L'estoc no pot ser negatiu")]
        public int Estoc { get; set; }

        [StringLength(20, ErrorMessage = "La categoria no pot excedir 20 caràcters")]
        public string? Categoria { get; set; }

        public bool Actiu { get; set; } = true;

        public DateTime DataCreacio { get; set; } = DateTime.UtcNow;

        public DateTime? DataModificacio { get; set; }
    }
}