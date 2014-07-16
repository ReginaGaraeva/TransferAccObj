using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ObjectTransferWCF.Interfaces;

namespace ObjectTransferWCF.Services
{
    public class LogService : ILogService
    {
        public void WriteInfo(string message)
        {
            using (var context = new ObjectTransferDBEntities())
            {
               context.SaveChanges();
               context.Logs.Add(
                   new Logs
                   {
                       Message = message,
                       Date = DateTime.Now,
                       MessageType = false,
                       StackTrace = null,
                   });
            }
        }

        public void WriteError(string message)
        {
            using (var context = new ObjectTransferDBEntities())
            {
                context.Logs.Add(
                    new Logs
                    {
                        Message = message,
                        Date = DateTime.Now,
                        MessageType = true,
                        StackTrace = null,
                    });
                context.SaveChanges();
            }
        }

        public void DeleteLogById(int id)
        {
            using (var context = new ObjectTransferDBEntities())
            {
                context.Logs.Remove((from logs in context.Logs 
                                     where logs.ID == id 
                                     select logs).FirstOrDefault());
                context.SaveChanges();
            }
        }
    }
}