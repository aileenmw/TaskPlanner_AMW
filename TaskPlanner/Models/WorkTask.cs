using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskPlanner.Models
{
    public class WorkTask
    {

        [Key]
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        [Required(ErrorMessage = "Opgaven mangler et navn (højst tegn")] 
        public string TaskName { get; set; }

        [Required(ErrorMessage = "Minimum alder skal udfyldes")]
        public int MinAge { get; set; }
    }
}
