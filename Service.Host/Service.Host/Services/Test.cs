using System;
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
            for (int i = 0; i < operationsCount; i++)
            {
                string inventaryNumber = generator.GenerateInventaryNumber(7);
                string description = generator.GenerateDescription();
                string postingDate = generator.GetDate();
                string deprecationDate = generator.GetDate();
                string owner = generator.GenerateOwner();
                switch (rnd.Next(4))
                {
                    case 0:
                        Console.WriteLine(String.Format("Добавляю объект учета\nИнв. номер: {0}\nОписание: {1}\nДата оприходования: {2}\nДата амортизации: {3}\nМОЛ: {4}",
                        inventaryNumber, description, postingDate, deprecationDate, owner));
                        threadsWrapper[num].service.CreateAccountingObject(inventaryNumber, description, postingDate, deprecationDate, owner);
                        break;
                    case 1:
                        threadsWrapper[num].service.UpdateAccountingObject("", "");
                        break;
                    case 2:
                        threadsWrapper[num].service.DeleteAccountingObject(generator.GenerateInventaryNumber(7));
                        break;
                    case 3:
                        threadsWrapper[num].service.DeleteAccountingObject(users.Items[rnd.Next(users.ItemsCount)].GetValue("ФИО").ToString());
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
