using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ObjectTransferWCF;

namespace Service.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            Type serviceType = typeof(ObjectTransferService);
            ServiceHost host = new ServiceHost(serviceType);
            host.Open();
        }
    }
}
