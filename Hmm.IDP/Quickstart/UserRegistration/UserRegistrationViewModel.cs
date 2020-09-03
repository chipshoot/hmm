using System.ComponentModel.DataAnnotations;

namespace Hmm.IDP.Quickstart.UserRegistration
{
    public class UserRegistrationViewModel
    {
        [MaxLength(200)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [MaxLength(200)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [MaxLength(250)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(250)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(500)]
        public string Address { get; set; }

        [Required]
        [MaxLength(500)]
        public string Email { get; set; }

        public string ReturnUrl { get; set; }
    }
}