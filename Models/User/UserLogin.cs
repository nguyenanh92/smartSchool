using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.User
{
    [Serializable]
    public class UserLogin
    {
        public int ID { get; set; }
        public string username { get; set; }
        public bool remember { get; set; }
    }
}
