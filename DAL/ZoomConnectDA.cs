using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Connection;
using Common.Helper;
using Models.User;
using Models.ZoomConnect;
using Oracle.DataAccess.Client;

namespace DAL
{
    public class ZoomConnectDA
    {
        public decimal Insert(ZoomConnect model)
        {
            try
            {
                OracleParameter paramReturn =
                    new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Config.connStr, CommandType.StoredProcedure,
                    "IT_COIN.pkg_s_user_zoom_connect.proc_s_user_zoom_connect_insert",
                    new OracleParameter("p_user_id", OracleDbType.Int32, model.UserId, ParameterDirection.Input),
                    new OracleParameter("p_access_token", OracleDbType.Varchar2, model.Access_token,
                        ParameterDirection.Input),
                    new OracleParameter("p_token_type", OracleDbType.Varchar2, model.Token_type,
                        ParameterDirection.Input),
                    new OracleParameter("p_refresh_token", OracleDbType.Varchar2, model.Refresh_token,
                        ParameterDirection.Input),
                    new OracleParameter("p_expires_in", OracleDbType.Int32, model.Expires_in, ParameterDirection.Input),
                    new OracleParameter("p_scope", OracleDbType.Varchar2, model.Scope, ParameterDirection.Input),
                    new OracleParameter("p_status", OracleDbType.Varchar2, model.Status, ParameterDirection.Input),
                    paramReturn);
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }
        }
        public decimal Update(ZoomConnect model)
        {
            try
            {
                OracleParameter paramReturn =
                    new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Config.connStr, CommandType.StoredProcedure,
                    "IT_COIN.pkg_s_user_zoom_connect.proc_s_user_zoom_connect_update",
                    new OracleParameter("p_user_id", OracleDbType.Int32, model.UserId, ParameterDirection.Input),
                    new OracleParameter("p_access_token", OracleDbType.Varchar2, model.Access_token,
                        ParameterDirection.Input),
                    new OracleParameter("p_token_type", OracleDbType.Varchar2, model.Token_type,
                        ParameterDirection.Input),
                    new OracleParameter("p_refresh_token", OracleDbType.Varchar2, model.Refresh_token,
                        ParameterDirection.Input),
                    new OracleParameter("p_expires_in", OracleDbType.Int32, model.Expires_in, ParameterDirection.Input),
                    new OracleParameter("p_scope", OracleDbType.Varchar2, model.Scope, ParameterDirection.Input),
                    new OracleParameter("p_status", OracleDbType.Varchar2, model.Status, ParameterDirection.Input),
                    paramReturn);
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }
        }
        public ZoomConnect GetById(int UserId)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Config.connStr, CommandType.StoredProcedure, "IT_COIN.pkg_s_user_zoom_connect.proc_s_user_zoom_connect_getbyuserid",
                    new OracleParameter("p_userid", OracleDbType.Decimal, UserId, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));

                EnumerableRowCollection<DataRow> list = ds.Tables[0].AsEnumerable();

                ZoomConnect zoom = new ZoomConnect();
                zoom = (from dr in list
                    select new ZoomConnect()
                    {
                        UserId = Convert.ToInt32(dr["ID"]),
                        Access_token = dr["ACCESS_TOKEN"].ToString(),
                        Token_type = dr["Token_type"].ToString(),
                        Refresh_token = dr["Refresh_token"].ToString(),
                        Expires_in = Convert.ToInt32(dr["Expires_in"]),
                        Scope = dr["Scope"].ToString(),
                        Status = dr["Status"].ToString(),
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