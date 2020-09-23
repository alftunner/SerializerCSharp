using System;
using System.Collections.Generic;
using System.Text;

namespace SerialToMySQL
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsStudy { get; set; }
        public DateTime Birthday { get; set; }
    }
}
