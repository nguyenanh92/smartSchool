using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Subject
    {
        public int  Id { get; set; }
        public int  TeacherId { get; set; }
        public string  Name { get; set; }
        public string  Description { get; set; }
        public string  Status { get; set; }
    }
}
