using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Models;
using Models.ZoomConnect;

namespace BLL
{
   public class SubjectBus
    {
        private static SubjectDA _subjectDa = new SubjectDA();

        public List<Subject> GetById(int tearchId)
        {
            try
            {
                return _subjectDa.GetById(tearchId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public decimal Update(int subjectId, string status)
        {
            try
            {
                return _subjectDa.Update(subjectId ,status);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
