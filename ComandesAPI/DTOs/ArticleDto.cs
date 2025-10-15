using System.ComponentModel.DataAnnotations;

namespace ComandesAPI.DTOs
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string? Descripcio { get; set; }
        public decimal Preu { get; set; }
        public int Estoc { get; set; }
        public string? Categoria { get; set; }
        public bool Actiu { get; set; }
        public DateTime DataCreacio { get; set; }
        public DateTime? DataModificacio { get; set; }
    }

    public class CreateArticleDto
    {
        [Required(ErrorMessage = "El nom de l'article és obligatori")]
        [StringLength(100, ErrorMessage = "El nom no pot excedir 100 caràcters")]
        public string Nom { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripció no pot excedir 500 caràcters")]
        public string? Descripcio { get; set; }

        [Required(ErrorMessage = "El preu és obligatori")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El preu ha de ser superior a 0")]
        public decimal Preu { get; set; }

        [Required(ErrorMessage = "L'estoc és obligatori")]
        [Range(0, int.MaxValue, ErrorMessage = "L'estoc no pot ser negatiu")]
        public int Estoc { get; set; }

        [StringLength(20, ErrorMessage = "La categoria no pot excedir 20 caràcters")]
        public string? Categoria { get; set; }

        public bool Actiu { get; set; } = true;
    }

    public class UpdateArticleDto
    {
        [StringLength(100, ErrorMessage = "El nom no pot excedir 100 caràcters")]
        public string? Nom { get; set; }

        [StringLength(500, ErrorMessage = "La descripció no pot excedir 500 caràcters")]
        public string? Descripcio { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El preu ha de ser superior a 0")]
        public decimal? Preu { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "L'estoc no pot ser negatiu")]
        public int? Estoc { get; set; }

        [StringLength(20, ErrorMessage = "La categoria no pot excedir 20 caràcters")]
        public string? Categoria { get; set; }

        public bool? Actiu { get; set; }
    }
}