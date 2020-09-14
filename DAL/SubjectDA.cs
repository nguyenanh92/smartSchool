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
    public class SubjectDA
    {
        public List<Subject> GetById(int tearchId)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Config.connStr, CommandType.StoredProcedure, "IT_COIN.pkg_subject.proc_getbyuserid",
                    new OracleParameter("p_teacherid", OracleDbType.Decimal, tearchId, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));

                EnumerableRowCollection<DataRow> list = ds.Tables[0].AsEnumerable();

                List<Subject> zoom = new List<Subject>();
                zoom = (from dr in list
                    select new Subject()
                    {
                        Id = Convert.ToInt32(dr["ID"]),
                        TeacherId = Convert.ToInt32(dr["TeacherId"]),
                        Name = dr["Name"].ToString(),
                        Description = dr["Description"].ToString(),
                        Status = dr["Status"].ToString(),
                    }).ToList();

                return zoom;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public decimal Update(int subjectId, string status)
        {
            try
            {
                OracleParameter paramReturn =
                    new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Config.connStr, CommandType.StoredProcedure,
                    "IT_COIN.pkg_subject.proc_update",
                    new OracleParameter("p_id", OracleDbType.Int32, subjectId, ParameterDirection.Input),
                    new OracleParameter("p_status", OracleDbType.Varchar2, status, ParameterDirection.Input),
                    paramReturn);
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }
        }
    }
}
