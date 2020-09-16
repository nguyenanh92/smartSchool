using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Connection;
using Common.Helper;
using Models;
using Models.ZoomConnect;
using Oracle.DataAccess.Client;

namespace DAL
{
    public class ZoomInfoDA
    {
        public decimal Insert(int teacherId, string info)
        {
            try
            {
                OracleParameter paramReturn =
                    new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Config.connStr, CommandType.StoredProcedure,
                    "IT_COIN.pkg_zoom_info.proc_s_zoom_info_insert",
                    new OracleParameter("p_teacherid", OracleDbType.Int32, teacherId, ParameterDirection.Input),
                    new OracleParameter("p_info", OracleDbType.Varchar2, info,
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
        public decimal Update(int teacherId, string info)
        {
            try
            {
                OracleParameter paramReturn =
                    new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Config.connStr, CommandType.StoredProcedure,
                    "IT_COIN.pkg_zoom_info.proc_s_zoom_info_update",
                    new OracleParameter("p_teacherid", OracleDbType.Int32, teacherId, ParameterDirection.Input),
                    new OracleParameter("p_info", OracleDbType.Varchar2, info,
                        ParameterDirection.Input),paramReturn);
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }
        }
        public ZoomInfo GetZoomInfoByTearchId(int teacherId)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Config.connStr, CommandType.StoredProcedure, "IT_COIN.pkg_zoom_info.proc_getbyuserid",
                    new OracleParameter("p_teacherid", OracleDbType.Decimal, teacherId, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));

                EnumerableRowCollection<DataRow> list = ds.Tables[0].AsEnumerable();

                ZoomInfo zoom = new ZoomInfo();
                zoom = (from dr in list
                        select new ZoomInfo()
                        {
                            InfoId = Convert.ToInt32(dr["ID"]),
                            TeacherId = Convert.ToInt32(dr["TEACHERID"]),
                            Info = dr["Info"].ToString()
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
