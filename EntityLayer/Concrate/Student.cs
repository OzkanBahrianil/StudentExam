using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrate
{
    public class Student
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Student_number { get; set; }
        public string Faculty_Name { get; set; }
        public string Section_name { get; set; }
        public int UserID { get; set; }
        public AppUser User { get; set; }
    }
}
