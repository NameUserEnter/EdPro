using System.ComponentModel.DataAnnotations;

namespace EdPro.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
