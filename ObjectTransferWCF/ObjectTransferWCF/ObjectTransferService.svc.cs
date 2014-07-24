﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WSSC.V4.SYS.DBFramework;
using WSSC.V4.DMS.Workflow;
using ObjectTransferWCF.Services;


namespace ObjectTransferWCF
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    public class ObjectTransferService : IObjectTransferService
    {
        AccountingObjectsList objectList;
        LogService logService;

        public ObjectTransferService()
        {
            //try
            //{
            //    objectList = new AccountingObjectsList();
            //}
            //catch
            //{
            //    throw new Exception("Не удалось соединиться с WSS Docs.");
            //}
            //logService = new LogService();
        }

        public string SayHello(string name)
        {
            return "Hello, " + name;
        }

        public string CreateList(string name)
        {
            DBSite dbSite = new DBSite("http://wsstest/");
            //DBWeb dbWeb = dbSite.GetWeb("/dms/contracts");
            //DBList dbList = dbWeb.GetList("Contracts");
            //dbList.CreateItem();
            return "success";
        }

        private bool CheckDbConnect()
        {
            using (var context = new ObjectTransferDBEntities())
            {
                try
                {
                    context.Database.Connection.Open();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }


        public string CreateAccountingObject(string inventaryNumber, string description, string postingDate,
            string deprecationDate, string owner)
        {
            try
            {
                objectList.Add(new Models.AccountingObjectModel()
                {
                    InventaryNumber = inventaryNumber,
                    Description = description,
                    PostingDate = Convert.ToDateTime(postingDate),
                    DeprecationDate = Convert.ToDateTime(deprecationDate),
                    Owner = owner,
                    Deleted = false
                });
                try
                {
                    logService.WriteInfo(String.Format("Добавлен объект учета\nИнвентарный номер: {0}\nОписание: {1}\nДата оприходования: {2}\nДата амортизации: {3}\nМОЛ: {4}",
                        inventaryNumber, description, postingDate, deprecationDate, owner));
                }
                catch
                {
                    return "Не удалось создать объект учета. И лог тоже не записался(";
                }
                return "Объект учета успешно создан.";
            }
            catch
            {
                return "Не удалось создать объект учета.";
            }
            
        }

        public string UpdateAccountingObject(string inventaryNumber, string newAccountingObject)
        {
            return "";
        }

        public string DeleteAccountingObject(string inventaryNumber)
        {
            return "";
        }
        //public string GetData(int value)
        //{
        //    return string.Format("You entered: {0}", value);
        //}

        //public CompositeType GetDataUsingDataContract(CompositeType composite)
        //{
        //    if (composite == null)
        //    {
        //        throw new ArgumentNullException("composite");
        //    }
        //    if (composite.BoolValue)
        //    {
        //        composite.StringValue += "Suffix";
        //    }
        //    return composite;
        //}
    }
}
