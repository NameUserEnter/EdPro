using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EdPro.Models
{
    public partial class CompetencesType
    {
        public CompetencesType()
        {
            Competences = new HashSet<Competence>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Тип компетентності")]
        public string CompType { get; set; } = null!;

        public virtual ICollection<Competence> Competences { get; set; }
    }
}
