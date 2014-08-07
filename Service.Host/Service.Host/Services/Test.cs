﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Host.Services;
using Service.Host.ServiceReference1;
using System.Threading;
using Service.Host.Models;
using WSSC.V4.SYS.DBFramework;
using WSSC.V4.DMS.Workflow;

namespace Service.Host.Services
{
    class Test
    {
        Generator generator;
        List<ThreadModel> threadsWrapper = new List<ThreadModel>();
        int operationsCount;
        int maxServicesCount;
        DBList users;
        DBList objectList;

        public Test(int _maxServicesCount, int _operationsCount)
        {
            operationsCount = _operationsCount;
            maxServicesCount = _maxServicesCount;
            DBSite dbSite = new DBSite("http://wsstest");
            DBWeb dbWeb = dbSite.GetWeb("/dms/requests");
            objectList = dbWeb.GetList("AccountingObjects");
            dbWeb = dbSite.GetWeb("/");
            users = dbWeb.GetList("Users");
        }

        public void TestItem()
        {
            Random rnd = new Random();
            int num = Convert.ToInt32(Thread.CurrentThread.Name);
            threadsWrapper[num].service = new ObjectTransferServiceClient();          
            OpenService(num);
            Console.WriteLine("Запущена служба № {0}", num);
            for (int i = 0; i < operationsCount; i++)
            {
                string oldInventaryNumber = generator.GenerateInventaryNumber(7);
                string inventaryNumber = generator.GenerateInventaryNumber(7);
                string description = generator.GenerateDescription();
                string postingDate = generator.GetDate();
                string deprecationDate = generator.GetDate();
                string owner = users.Items[rnd.Next(users.ItemsCount)].GetValue("ФИО").ToString();
                switch (rnd.Next(4))
                {
                    case 0:
                        Console.WriteLine(String.Format("Добавляю объект учета\nИнв. номер: {0}\nОписание: {1}\nДата оприходования: {2}\nДата амортизации: {3}\nМОЛ: {4}",
                        inventaryNumber, description, postingDate, deprecationDate, owner));
                        threadsWrapper[num].service.CreateAccountingObject(inventaryNumber, description, postingDate, deprecationDate, owner);
                        break;
                    case 1:
                        Console.WriteLine(String.Format("Изменяю объект учета (инв.номер: {0}. Такого объекта не должно существовать)\nИнв. номер: {1}\nОписание: {2}\nДата оприходования: {3}\nДата амортизации: {4}\nМОЛ: {5}",
                        oldInventaryNumber, inventaryNumber, description, postingDate, deprecationDate, owner));
                        threadsWrapper[num].service.UpdateAccountingObject(oldInventaryNumber, inventaryNumber, description,
                            postingDate, deprecationDate, owner);
                        break;
                    case 2:
                        Console.WriteLine("Удаляю объект учета с инв. номером {0}. Такого объекта нет.",inventaryNumber);
                        threadsWrapper[num].service.DeleteAccountingObject(generator.GenerateInventaryNumber(7));
                        break;
                    case 3:
                        inventaryNumber = objectList.Items[rnd.Next(objectList.ItemsCount)].GetValue("InventaryNumber").ToString();
                        Console.WriteLine("Удаляю объект учета с инв. номером {0}", inventaryNumber);
                        threadsWrapper[num].service.DeleteAccountingObject(inventaryNumber);
                        break;
                    case 4:
                        oldInventaryNumber = objectList.Items[rnd.Next(objectList.ItemsCount)].GetValue("InventaryNumber").ToString();
                        Console.WriteLine(String.Format("Изменяю объект учета (инв.номер: {0}.)\nИнв. номер: {1}\nОписание: {2}\nДата оприходования: {3}\nДата амортизации: {4}\nМОЛ: {5}",
                        oldInventaryNumber, inventaryNumber, description, postingDate, deprecationDate, owner));
                        threadsWrapper[num].service.UpdateAccountingObject(oldInventaryNumber, inventaryNumber, description,
                            postingDate, deprecationDate, owner);
                        break;

                }
            }
            CloseService(num);
        }

        public void Run()
        {
            Random rnd = new Random();
            while (true)
                if ((threadsWrapper.Count() < maxServicesCount) && (rnd.Next() < 0.2))
                {
                    threadsWrapper[threadsWrapper.Count() - 1] = new ThreadModel();
                    threadsWrapper[threadsWrapper.Count() - 1].thread = new Thread(TestItem);
                    threadsWrapper[threadsWrapper.Count() - 1].thread.Name = (threadsWrapper.Count() - 1).ToString();
                }
        }

        public void Stop()
        {
            foreach (var threadWrapper in threadsWrapper)
                threadWrapper.thread.Abort();
        }

        public void OpenService(int number)
        {
            if (threadsWrapper[number].service == null) threadsWrapper[number].service = new ObjectTransferServiceClient();
            else
                if (threadsWrapper[number].service.State != System.ServiceModel.CommunicationState.Opened)
                    threadsWrapper[number].service.Open();
        }

        public void CloseService(int number)
        {
            if (threadsWrapper[number].service.State != System.ServiceModel.CommunicationState.Closed)
                threadsWrapper[number].service.Close();
        }
    }
}
