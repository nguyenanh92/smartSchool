using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Connection;
using Common.Helper;
using Models;
using Oracle.DataAccess.Client;

namespace DAL
{
    public class RoomMeetingDA
    {
        public decimal Insert(int subjectId, string meeting_info)
        {
            try
            {
                OracleParameter paramReturn =
                    new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Config.connStr, CommandType.StoredProcedure,
                    "IT_COIN.pkg_s_room.proc_s_room_meeting_insert",
                    new OracleParameter("p_subjectid", OracleDbType.Int32, subjectId, ParameterDirection.Input),
                    new OracleParameter("p_meeting_info", OracleDbType.Varchar2, meeting_info,
                        ParameterDirection.Input),
                    paramReturn);
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }
        }
        public decimal Update(int subjectId, string meeting_info)
        {
            try
            {
                OracleParameter paramReturn =
                    new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Config.connStr, CommandType.StoredProcedure,
                    "IT_COIN.pkg_s_room.proc_s_room_meeting_update",
                    new OracleParameter("p_subjectid", OracleDbType.Int32, subjectId, ParameterDirection.Input),
                    new OracleParameter("p_meeting_info", OracleDbType.Varchar2, meeting_info,
                        ParameterDirection.Input), paramReturn);
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }
        }
        public MeetingInfo GetInfMeetingBySubjectId(int subjectId)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Config.connStr, CommandType.StoredProcedure, "IT_COIN.pkg_s_room.proc_s_room_meeting_getbysubjetid",
                    new OracleParameter("p_subjectid", OracleDbType.Decimal, subjectId, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));

                EnumerableRowCollection<DataRow> list = ds.Tables[0].AsEnumerable();

                MeetingInfo zoom = new MeetingInfo();
                zoom = (from dr in list
                        select new MeetingInfo()
                        {
                            MeetingId = Convert.ToInt32(dr["ID"]),
                            SubjectId = Convert.ToInt32(dr["SUBJECTID"]),
                            Meeting_Info = dr["MEETING_INFO"].ToString()
                        }).FirstOrDefault();

                return zoom;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
