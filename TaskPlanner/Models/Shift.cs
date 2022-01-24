using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskPlanner.Models
{
    public class Shift
    {

        [Key]
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "StartTime")]
        [DisplayFormat(DataFormatString = "{0:d/M/yyyy HH:mm}", ApplyFormatInEditMode=true)]
        [Required(ErrorMessage = "Start tidspunkt skal udfyldes")]
        public DateTime StartTime { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "EndTime")]
        [DisplayFormat(DataFormatString = "{0:d/M/yyyy HH:mm}", ApplyFormatInEditMode=true)]
        [Required(ErrorMessage = "Slut tidspunkt skal udfyldes")]
        public DateTime EndTime { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        public string? ShiftTasks { get; set; }

        [Required(ErrorMessage = "Udfyld medarbejder")]
        public int Worker { get; set; }
    }

}
