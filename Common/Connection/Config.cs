using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Connection
{
    public static class Config
    {
        public static string connStr = ConfigurationManager.AppSettings["DefaultConnection"];
    }
}
