using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EdPro.Models
{
    public partial class EducationProgram
    {
        public EducationProgram()
        {
            Subjects = new HashSet<Subject>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Назва")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Спеціальність")]
        public int SpecialityId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "ЕДБО")]
        public string Edbo { get; set; } = null!;
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Тип навчальної програми")]
        public int EdPrTypeId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Факультет")]
        public int FacultyId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Дата створення")]
        public DateTime ImplementationDate { get; set; }
        [Display(Name = "Тип навчальної програми")]
        public virtual EdProgramType EdPrType { get; set; } = null!;
        [Display(Name = "Факультет")]
        public virtual Faculty Faculty { get; set; } = null!;
        [Display(Name = "Спеціальність")]
        public virtual Speciality Speciality { get; set; } = null!;
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
