using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ObjectTransferWCF.Interfaces;

namespace ObjectTransferWCF.Services
{
    public class LogService : ILogService
    {
        void WriteInfo(string message)
        {
            using (var context = new ObjectTransferDBEntities())
            {
               context.SaveChanges();
            }
        }

        void WriteError(string message)
        {
            using (var context = new ObjectTransferDBEntities())
            {
                context.SaveChanges();
            }
        }

        void DeleteLogById(int id)
        {

        }
    }
}