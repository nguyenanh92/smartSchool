using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Models;

namespace BLL
{
    public class ZoomInfoBus

    {
        private static ZoomInfoDA _zoomInfoDa = new ZoomInfoDA();

        public decimal Insert(int teacherId , string info)
        {
            try
            {
                return _zoomInfoDa.Insert(teacherId ,info);
            }
            catch (Exception e) 
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public ZoomInfo GetZoomInfoByTearchId(int tearchId)
        {
            try
            {
                return _zoomInfoDa.GetZoomInfoByTearchId(tearchId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public decimal Update(int teacherId, string info)
        {
            try
            {
                return _zoomInfoDa.Update(teacherId, info);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
