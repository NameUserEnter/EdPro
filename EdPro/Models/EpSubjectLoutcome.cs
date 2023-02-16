using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EdPro.Models
{
    public partial class EpSubjectLoutcome
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Предмет")]
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Навчальні результати")]
        public int LearningOutcomeId { get; set; }
        [Display(Name = "Навчальні результати")]
        public virtual LearningOutcome LearningOutcome { get; set; } = null!;
        [Display(Name = "Предмет")]
        public virtual Subject Subject { get; set; } = null!;
    }
}
