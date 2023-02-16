using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EdPro.Models
{
    public partial class LearningOutcome
    {
        public LearningOutcome()
        {
            EpSubjectLoutcomes = new HashSet<EpSubjectLoutcome>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Навчальні результати")]
        public string LearningOutcome1 { get; set; } = null!;
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Назва")]
        public string Loname { get; set; } = null!;
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Спеціальність")]
        public int SpecialityId { get; set; }
        [Display(Name = "Спеціальність")]

        public virtual Speciality Speciality { get; set; } = null!;
        public virtual ICollection<EpSubjectLoutcome> EpSubjectLoutcomes { get; set; }
    }
}
