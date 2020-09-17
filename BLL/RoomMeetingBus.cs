using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Models;

namespace BLL
{
    public class RoomMeetingBus
    {
        private static RoomMeetingDA _roomMeetingDa = new RoomMeetingDA();

        public decimal Insert(int subjectId, string info)
        {
            try
            {
                return _roomMeetingDa.Insert(subjectId, info);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public MeetingInfo GetInfMeetingBySubjectId(int subjectId)
        {
            try
            {
                return _roomMeetingDa.GetInfMeetingBySubjectId(subjectId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public decimal Update(int subjectId, string info)
        {
            try
            {
                return _roomMeetingDa.Update(subjectId, info);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
