using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EdPro.Models
{
    public partial class EpSubjectCompetence
    {
        [Display(Name = "Компетентність")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Предмет")]
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Компетентність")]
        public int SpecialityCompetenceId { get; set; }
        [Display(Name = "Компетентність")]

        public virtual SpecialityCompetence SpecialityCompetence { get; set; } = null!;
        [Display(Name = "Предмет")]
        public virtual Subject Subject { get; set; } = null!;
    }
}
