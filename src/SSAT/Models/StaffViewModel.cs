using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSAT.Models
{
    public class StaffViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string JobTitle { get; set; }
        public int SalaryGrade { get; set; }
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }
    }
}
