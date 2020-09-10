using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Connection
{
    public static class Connection
    {
        
            public static string connStr = ConfigurationManager.AppSettings["DefaultConnection"];
 
    }
}
