using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WSSC.V4.DMS.Workflow;
using WSSC.V4.SYS.DBFramework;
using ObjectTransferWCF.Models;

namespace ObjectTransferWCF.Services
{
    public class AccountingObjectsList
    {
        private DBList accountingObjects;

        public AccountingObjectsList()
        {
            DBSite dbSite = new DBSite("http://wsstest");
            DBWeb dbWeb = dbSite.GetWeb("/dms/requires");
            accountingObjects = dbWeb.GetList("AccountingObjects");
        }

        public bool Exists(AccountingObjectModel accObject)
        {

            return true;
        }

        public int Add(AccountingObjectModel accObject)
        {
            return 0;
        }

        public int Update(string inventaryNumber, AccountingObjectModel accObject)
        {
            return 0;
        }

        public int Delete(string inventaryNumber)
        {
            return 0;
        }

    }
}