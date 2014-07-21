using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Host.Services;
using Service.Host.ServiceReference1;
using System.Threading;
using Service.Host.Models;

namespace Service.Host.Services
{
    class Test
    {
        private Generator generator;
        List<ThreadModel> threadsWrapper;
        int operationsCount;
        int maxServicesCount;

        public Test(int _maxServicesCount, int _operationsCount)
        {
            operationsCount = _operationsCount;
            maxServicesCount = _maxServicesCount;
        }

        public void TestItem()
        {
            Random rnd = new Random();
            int num = Convert.ToInt32(Thread.CurrentThread.Name);
            threadsWrapper[num].service = new ObjectTransferServiceClient();
            OpenService(num);
            for (int i = 0; i < operationsCount; i++)
                switch (rnd.Next(3))
                {
                    case 0:
                        threadsWrapper[num].service.CreateAccountingObject("");
                        break;
                    case 1:
                        threadsWrapper[num].service.UpdateAccountingObject("", "");
                        break;
                    case 2:
                        threadsWrapper[num].service.DeleteAccountingObject("");
                        break;
                }
            CloseService(num);
        }

        public void Run()
        {
            Random rnd = new Random();
            while (true)
                if ((threadsWrapper.Count() < maxServicesCount) && (rnd.Next() < 0.2))
                {
                    threadsWrapper[threadsWrapper.Count() - 1].thread.Name = (threadsWrapper.Count() - 1).ToString();
                    threadsWrapper[threadsWrapper.Count() - 1].thread.Start();
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
