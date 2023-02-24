using System.ComponentModel.DataAnnotations;

namespace EdPro.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запам'ятати?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

    }
}
