using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ZoomConnect
{
    public class ZoomConnect
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Access_token { get; set; }
        public string Token_type { get; set; }
        public string Refresh_token { get; set; }
        public int Expires_in { get; set; }
        public string Scope { get; set; }
        public string Status { get; set; }

    }
}
