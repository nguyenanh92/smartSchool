using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Models.User;
using Models.ZoomConnect;

namespace BLL
{
    public class ZoomConnectBus
    {
        private static ZoomConnectDA _zoomConfigDa = new ZoomConnectDA();

        public decimal Insert(ZoomConnect model)
        {
            try
            {
                return _zoomConfigDa.Insert(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public ZoomConnect GetById(int UserId)
        {
            try
            {
                return _zoomConfigDa.GetById(UserId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
