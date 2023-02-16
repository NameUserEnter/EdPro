using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EdPro.Models
{
    public partial class ControlType
    {
        public ControlType()
        {
            Subjects = new HashSet<Subject>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Тип контролю")]
        public string ControlTypeName { get; set; } = null!;

        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
