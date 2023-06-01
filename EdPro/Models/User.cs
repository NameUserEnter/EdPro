using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EdPro.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Range(1945, 2017, ErrorMessage = "Рік народжуння повинен бути від 1945 до 2017")]
        [Display(Name = "Рік народження")]
        public int Year { get; set; }
    }
}
