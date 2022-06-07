using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrate
{
    public class Student
    {
        
        public int studentId { get; set; }
        public string studentName { get; set; }
        public string studentSurname { get; set; }
        public string studentNumber { get; set; }
        public string studentFacultyName { get; set; }
        public string studentDepartment { get; set; }
        public int UserID { get; set; }
        public AppUser User { get; set; }
    }
}
