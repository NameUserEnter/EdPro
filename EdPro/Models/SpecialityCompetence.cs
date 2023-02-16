using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EdPro.Models
{
    public partial class SpecialityCompetence
    {
        public SpecialityCompetence()
        {
            EpSubjectCompetences = new HashSet<EpSubjectCompetence>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Спеціальність")]
        public int SpecialityId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Компетентність")]
        public int CompetenceId { get; set; }
        [Display(Name = "Компетентність")]
        public virtual Competence Competence { get; set; } = null!;
        [Display(Name = "Спеціальність")]
        public virtual Speciality Speciality { get; set; } = null!;
        public virtual ICollection<EpSubjectCompetence> EpSubjectCompetences { get; set; }
    }
}
