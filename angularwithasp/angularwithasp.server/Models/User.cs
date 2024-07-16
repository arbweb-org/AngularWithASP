using System.ComponentModel.DataAnnotations;

namespace angularwithasp.server.Models
{
    public class User
    {
        public long Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required, MaxLength(254)]
        public string Email { get; set; }
    }
}