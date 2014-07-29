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
            Console.WriteLine("Такой объект учета еще не существует");
            Console.WriteLine("-----------------------------");
            Console.WriteLine(String.Format("Добавляю объект учета\nИнв. номер: {0}\nОписание: {1}\nДата оприходования: {2}\nДата амортизации: {3}\nМОЛ: {4}",
                accObject.InventaryNumber, accObject.Description, accObject.PostingDate, accObject.DeprecationDate, accObject.Owner));
            Console.WriteLine("-----------------------------");
            //Название настройки процессов, по которой создаем документ
            string settingName = "Requests";
            //Создаем документ по процессу
            bool hasAccess = false;
            dmsLogic.CreateDMSItem(dmsContext, settingName, dbList.CreateItem(), out hasAccess);
            DBItem dbItem = dmsLogic.DMSItem.Item;
            //Заполняем поля карточки
            Console.WriteLine("Заполняем поля списка");
            dbItem.SetValue("InventaryNumber", accObject.InventaryNumber);
            dbItem.SetValue("Description", accObject.Description);
            dbItem.SetValue("PostingDate", accObject.PostingDate);
            dbItem.SetValue("DeprecationDate", accObject.DeprecationDate);
            dbItem.SetValue("Owner", accObject.Owner);
            dbItem.SetValue("IsDeleted", accObject.Deleted);
            //принимаем решение "Зарегистрировать документ"
            Console.WriteLine("Регистрируем документ");
            dmsLogic.AutoAcceptSolution("Зарегистрировать документ");
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
                Console.WriteLine("Такой объект существует");
                Console.WriteLine("-----------------------------");
                Console.WriteLine(String.Format("Изменяю объект учета ({0})\nИнв. номер: {1}\nОписание: {2}\nДата оприходования: {3}\nДата амортизации: {4}\nМОЛ: {5}",
                    OldInventaryNumber, accObject.InventaryNumber, accObject.Description, accObject.PostingDate, accObject.DeprecationDate, accObject.Owner));
                Console.WriteLine("-----------------------------");
                dbList.Items[i].SetValue("InventaryNumber", accObject.InventaryNumber);
                dbList.Items[i].SetValue("Description", accObject.Description);
                dbList.Items[i].SetValue("PostingDate", accObject.PostingDate);
                dbList.Items[i].SetValue("DeprecationDate", accObject.DeprecationDate);
                dbList.Items[i].SetValue("Owner", accObject.Owner);
                dbList.Items[i].SetValue("IsDeleted", accObject.Deleted);
                dbList.Items[i].Update();
                Console.WriteLine("Обновление прошло успешно?");
                foreach (var l in dbList.Items)
                {
                    Console.WriteLine("{0}",l.GetValue("InventaryNumber").ToString());
                }
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
            {
                dbList.Items[i].Delete();
                dbList.Items[i].Update();
            }
            Console.WriteLine("Удалили объект учета");
            return 9;
        }

    }
}