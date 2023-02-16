using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EdPro.Models
{
    public partial class Speciality
    {
        public Speciality()
        {
            EducationPrograms = new HashSet<EducationProgram>();
            LearningOutcomes = new HashSet<LearningOutcome>();
            SpecialityCompetences = new HashSet<SpecialityCompetence>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Назва")]
        public string Name { get; set; } = null!;

        public virtual ICollection<EducationProgram> EducationPrograms { get; set; }
        public virtual ICollection<LearningOutcome> LearningOutcomes { get; set; }
        public virtual ICollection<SpecialityCompetence> SpecialityCompetences { get; set; }
    }
}
