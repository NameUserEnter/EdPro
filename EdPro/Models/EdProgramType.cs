using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EdPro.Models
{
    public partial class EdProgramType
    {
        public EdProgramType()
        {
            EducationPrograms = new HashSet<EducationProgram>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Тип навчальної програми")]
        public string TypeName { get; set; } = null!;

        public virtual ICollection<EducationProgram> EducationPrograms { get; set; }
    }
}
