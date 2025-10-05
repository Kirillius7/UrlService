using System.ComponentModel.DataAnnotations;

namespace UrlService.Models
{
    public class CreateUrlDto
    {
        [Required]
        public string OriginalUrl { get; set; }
    }
}
