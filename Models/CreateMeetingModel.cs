using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CreateMeetingModel
    {
        public int SubjectId { get; set; }
        public string Title { get; set; }
        public string Role { get; set; }
        public string Date { get; set; }
        public string Duration { get; set; }
        public string Host_video { get; set; }
        public string Client_video { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
    }
}
