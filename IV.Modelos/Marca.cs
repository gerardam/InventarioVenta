using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IV.Modelos
{
	public class Marca
	{
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Nombre categoria")]
        public string Nombre { get; set; }

        [Required]
        public bool Estado { get; set; }
    }
}
