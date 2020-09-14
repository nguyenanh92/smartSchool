using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ZoomConfiguration
{
    public class JWT
    {
        public static string APIKey = ConfigurationManager.AppSettings["APIKey"];
        public static string APISecret = ConfigurationManager.AppSettings["APISecret"];

    }
}
