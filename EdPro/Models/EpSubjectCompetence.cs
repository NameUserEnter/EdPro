using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EdPro.Models
{
    public partial class EpSubjectCompetence
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Предмет")]
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Спеціальність та компетентність")]
        public int SpecialityCompetenceId { get; set; }
        [Display(Name = "Спецівльність та компетентність")]

        public virtual SpecialityCompetence SpecialityCompetence { get; set; } = null!;
        [Display(Name = "Предмет")]
        public virtual Subject Subject { get; set; } = null!;
    }
}
