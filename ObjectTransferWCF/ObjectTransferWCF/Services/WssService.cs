using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WSSC.V4.DMS.Workflow;
using WSSC.V4.SYS.DBFramework;
using ObjectTransferWCF.Models;

namespace ObjectTransferWCF.Services
{
    public class WSSList
    {
        private DBList dbList;
        private DBUser dbUser;
        private DMSContext dmsContext;
        private DMSLogic dmsLogic;

        public WSSList()
        {           
            DBSite dbSite = new DBSite("http://wsstest");
            Console.WriteLine("Соединился с wsstest");
            DBWeb dbWeb = dbSite.GetWeb("/dms/requests");
            Console.WriteLine("Загрузил страницу /dms/requests");
            dbList = dbWeb.GetList("AccountingObjects");
            Console.WriteLine("Загрузил список объектов учета");
            dmsContext = new DMSContext(dbWeb);
            Console.WriteLine("Создал dmsContext");
            dmsLogic = new DMSLogic();
            Console.WriteLine("Создал dmsLogic");
        }

        public bool Exists(AccountingObjectModel accObject)
        {
            foreach (var l in dbList.Items)
            {
                if (l.GetValue("InventaryNumber").ToString() == accObject.InventaryNumber)
                    return true;
            }
            return false;
        }

        public int Add(AccountingObjectModel accObject)
        {
            if (!Exists(accObject))
            {
                //Название настройки процессов, по которой создаем документ
                string settingName = "requests";
                //Создаем документ по процессу
                bool hasAccess = false;               
                dmsLogic.CreateDMSItem(dmsContext, settingName, dbList.CreateItem(), out hasAccess);
                DBItem dbItem = dmsLogic.DMSItem.Item;
                //Заполняем поля карточки
                dbItem.SetValue("InventaryNumber", accObject.InventaryNumber);
                dbItem.SetValue("Description", accObject.Description);
                dbItem.SetValue("PostingDate", accObject.PostingDate);
                dbItem.SetValue("DeprecationDate", accObject.DeprecationDate);
                dbItem.SetValue("Owner", accObject.Owner);
                dbItem.SetValue("IsDeleted", accObject.Deleted);
                //принимаем решение "Зарегистрировать документ"
                dmsLogic.AutoAcceptSolution("Зарегистрировать документ");
                return 0;
            }
            else
                return 0;
        }

        public int Update(string inventaryNumber, AccountingObjectModel accObject)
        {
            if (Exists(accObject))
            {

            }
            else
            {
            }
            return 0;
        }

        public int Delete(string inventaryNumber)
        {
            return 0;
        }

    }
}