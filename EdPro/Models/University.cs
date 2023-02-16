using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EdPro.Models
{
    public partial class University
    {
        public University()
        {
            Faculties = new HashSet<Faculty>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Назва")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "ЕДБО")]
        public string Edbo { get; set; } = null!;

        public virtual ICollection<Faculty> Faculties { get; set; }
    }
}
