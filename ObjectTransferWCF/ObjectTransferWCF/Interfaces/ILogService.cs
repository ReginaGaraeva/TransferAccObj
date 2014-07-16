using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectTransferWCF.Interfaces
{
    interface ILogService
    {
        void WriteInfo(string message);
        void WriteError(string message);
        List<ILogService> GetLogsByDate(DateTime dateFrom, DateTime dateEnd = null);
        bool DeleteLogById(int id);
    }
}
