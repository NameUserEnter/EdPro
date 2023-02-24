using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EdPro.Models
{
    public partial class Subject
    {
        public Subject()
        {
            EpSubjectCompetences = new HashSet<EpSubjectCompetence>();
            EpSubjectLoutcomes = new HashSet<EpSubjectLoutcome>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Назва")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Навчальна програма")]
        public int EprogramId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Кредити")]
        public int Credit { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Контроль")]
        public int ControlId { get; set; }
        [Display(Name = "Контроль")]
        public virtual ControlType Control { get; set; } = null!;
        [Display(Name = "Навчальна програма")]
        public virtual EducationProgram Eprogram { get; set; } = null!;
        public virtual ICollection<EpSubjectCompetence> EpSubjectCompetences { get; set; }
        public virtual ICollection<EpSubjectLoutcome> EpSubjectLoutcomes { get; set; }
    }
}
