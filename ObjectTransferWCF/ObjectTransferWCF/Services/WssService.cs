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

        public int Exists(string InventaryNumber)
        {
            for (int i = 0; i < dbList.ItemsCount; i++)
            {
                if (dbList.Items[i].GetValue("InventaryNumber").ToString() == InventaryNumber)
                    return i;
            }
            return -1;
        }

        public int Add(AccountingObjectModel accObject)
        {
            if (Exists(accObject.InventaryNumber) == -1)
            {
                Console.WriteLine("Такой объект учета еще не существует");
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

        public int Update(string OldInventaryNumber, AccountingObjectModel accObject)
        {
            int i = Exists(OldInventaryNumber);
            if (i == -1)
            {
                Console.WriteLine("Такого объекта для обновления нет, начинаем создание.");
                Add(accObject);
            }
            else
            {
                dbList.Items[i].SetValue("InventaryNumber", accObject.InventaryNumber);
                dbList.Items[i].SetValue("Description", accObject.Description);
                dbList.Items[i].SetValue("PostingDate", accObject.PostingDate);
                dbList.Items[i].SetValue("DeprecationDate", accObject.DeprecationDate);
                dbList.Items[i].SetValue("Owner", accObject.Owner);
                dbList.Items[i].SetValue("IsDeleted", accObject.Deleted);
                Console.WriteLine("Обновление прошло успешно?");
                return 5;
            }
            return 0;
        }

        public int Delete(string InventaryNumber)
        {
            int i = Exists(InventaryNumber);
            if (i == -1)
            {
                Console.WriteLine("Такого объекта для удаления нет");
                return 11;
            }
            else
                dbList.Items[i].Delete();
            Console.WriteLine("Удалили объект учета");
            return 9;
        }

    }
}