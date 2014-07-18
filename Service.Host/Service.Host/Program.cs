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
            //var baseAddressHttp = new Uri("http://localhost:13313/ObjectTransferWCF.svc"); 
            //Type serviceType = typeof(ObjectTransferService);
            //ServiceHost host = new ServiceHost(serviceType, baseAddressHttp);
            //host.Open();
            using (var service = new Service.Host.ServiceReference1.ObjectTransferServiceClient())
            {
                service.ClientCredentials.UserName.UserName = @"wssdom\Администратор";
                service.ClientCredentials.UserName.Password = @"P@ssw0rd";
                Console.WriteLine( service.SayHello("Sear!"));
            }
            Console.ReadLine();
        }
    }
}
