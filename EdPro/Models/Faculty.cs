using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EdPro.Models
{
    public partial class Faculty
    {
        public Faculty()
        {
            EducationPrograms = new HashSet<EducationProgram>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Назва")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Університет")]
        public int UniversityId { get; set; }
        [Display(Name = "Університет")]

        public virtual University University { get; set; } = null!;
        public virtual ICollection<EducationProgram> EducationPrograms { get; set; }
    }
}
