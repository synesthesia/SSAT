using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSAT.Models
{
    public class StaffJobDetails
    {
        [Key]
        public int ID { get; set; }

        public int StaffID { get; set; }

        [Required]
        [MinLength(10)]
        public string JobTitle { get; set; }

        public DateTime StartDate { get; set; }

        public int SalaryGrade  { get; set; }
    }
}
