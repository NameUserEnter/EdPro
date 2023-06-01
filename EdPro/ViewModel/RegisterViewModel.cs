using System.ComponentModel.DataAnnotations;

namespace EdPro.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Range(1945, 2017, ErrorMessage = "Рік народжуння повинен бути від 1945 до 2017")]
        [Display(Name = "Рік народжуння")]
        public int? Year { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Compare("Password",ErrorMessage = "Паролі не співпадають")]
        [Display(Name = "Підтвердження пароля")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

    }
}
