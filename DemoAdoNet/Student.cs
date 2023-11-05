using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAdoNet
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public DateTime Dob { get; set; }
        public string Nationality { get; set; }
        public DateTime JoinDate { get; set; }
        public bool Status { get; set; }
    }
}
