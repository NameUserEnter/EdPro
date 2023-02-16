using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EdPro.Models //перевірочка
{
    public partial class Competence // перевірочка перевірочки
    {
        public Competence() // перевірочка переірочки перевірочки
        {
            SpecialityCompetences = new HashSet<SpecialityCompetence>(); //перевірочка перевірочки перевірочки перевірочки
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Компетентність")]
        public string Competence1 { get; set; } = null!;
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Тип компетентності")]
        public int CompetenceTypeId { get; set; }
        [Display(Name = "Тип компетентності")]
        public virtual CompetencesType CompetenceType { get; set; } = null!;
        public virtual ICollection<SpecialityCompetence> SpecialityCompetences { get; set; }
    }
}
