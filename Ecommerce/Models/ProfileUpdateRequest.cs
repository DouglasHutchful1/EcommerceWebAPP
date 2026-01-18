using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class ProfileUpdateRequest
    {
        [Required, StringLength(80)]
        public string Firstname { get; set; } = "";

        [Required, StringLength(80)]
        public string Lastname { get; set; } = "";
    }
}