using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Host.Services;
using Service.Host.ServiceReference1;
using System.Threading;

namespace Service.Host.Services
{
    class Test
    {
        private Generator generator;
        List<ObjectTransferServiceClient> services;
        List<Thread> threads;

        public void Run(int maxServicesCount, int operationsCount)
        {
        }

        public void Stop()
        {
            foreach (var thread in threads)
                thread.Abort();
        }

        public void StartService()
        {

        }

        public void StopService()
        {

        }
    }
}
