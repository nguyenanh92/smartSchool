using System;
using Common.Helper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Common.Connection;
using Oracle.DataAccess.Client;

namespace Models.User
{
    public class CheckLogin
    {
        public  int CheckUserLogin(LoginViewModel model)
        {
            CurrentSession.ClearAll();
            string pashPassWord = Password.HashPassword(model.password);
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Config.connStr, CommandType.StoredProcedure, "IT_COIN.pkg_s_user.proc_user_login",
                    new OracleParameter("p_username", OracleDbType.Varchar2, model.username, ParameterDirection.Input),
                    new OracleParameter("p_password", OracleDbType.Varchar2, pashPassWord, ParameterDirection.Input),
                    paramReturn);
                return Convert.ToInt32(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }

        }

        public User GetUser(string username)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Config.connStr, CommandType.StoredProcedure, "IT_COIN.pkg_s_user.proc_user_getbyusername",
                    new OracleParameter("p_username", OracleDbType.Varchar2, username, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));

                EnumerableRowCollection<DataRow> list = ds.Tables[0].AsEnumerable();

                User users = new User();
                users = (from dr in list
                    select new User()
                    {
                        id = Convert.ToInt32(dr["ID"]),
                        username = dr["USERNAME"].ToString(),
                    }).FirstOrDefault();

                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new User();
            }

        }
    }
}