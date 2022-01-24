using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskPlanner.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        [Required(ErrorMessage ="Efternavn skal udfyldes")]
        public string LastName { get; set; }

        [Display(Name = "Worker Id")]
        public string WorkerName
        {
            get { return  LastName + Id.ToString(); }
        }

        [Required(ErrorMessage = "Alder skal udfyldes")]
        public int Age { get; set; }
    }
}
