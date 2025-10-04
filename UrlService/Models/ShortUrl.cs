using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlService.Models
{
    public class ShortUrl
    {
        [Key] // ключ таблиці
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(2048)]
        public string OriginalUrl { get; set; }

        [Required, MaxLength(20)]
        public string ShortCode { get; set; }

        [Required, MaxLength(50)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
