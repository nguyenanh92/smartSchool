using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ZoomInfo
    {
        public int InfoId { get; set; }
        public int TeacherId { get; set; }
        public string Info { get; set; }

        public string Id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Email { get; set; }
        public int Type { get; set; }
        public string role_name { get; set; }
        public long Pmi { get; set; }
        public bool Use_pmi { get; set; }
        public string Personal_meeting_url { get; set; }
        public string Timezone { get; set; }
        public int Verified { get; set; }
        public string Dept { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Last_login_time { get; set; }
        public string Host_key { get; set; }
        public string Jid { get; set; }
        public List<object> Group_ids { get; set; }
        public List<object> Im_group_ids { get; set; }
        public string Account_id { get; set; }
        public string Language { get; set; }
        public string Phone_country { get; set; }
        public string Phone_number { get; set; }
        public string Status { get; set; }
    }
}
