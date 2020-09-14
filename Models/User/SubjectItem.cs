using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.User
{
    public class SubjectItem
    {
        public long Id { get; set; }
        public long MeetingId { get; set; }
        public string Start_url { get; set; }
        public string Join_url { get; set; }
        public string Created_at { get; set; }
        public string Password { get; set; }
    }
}
