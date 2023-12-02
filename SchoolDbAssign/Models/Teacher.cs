using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolDbAssignmnet.Models
{
    public class Teacher
    {
        //What defines the teacher
        public int TeacherId {  get; set; }    
        public string TeacherFname { get; set; }
        public string TeacherLname { get; set; }
        public string TeacherName {  get; set; }
        public decimal Salary {  get; set; }
        public DateTime HireDate { get; set; }
        public string EmpNumber { get; set; }
        public string ClassName { get; set; }
    }
}