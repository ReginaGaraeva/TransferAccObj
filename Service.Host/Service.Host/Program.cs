using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            Type serviceType = typeof(ObjectTransferService);
            Uri serviceUri = new Uri("http://localhost:8080/");
            ServiceHost host = new ServiceHost(serviceType, serviceUri);
            host.Open();
        }
    }
}
