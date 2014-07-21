using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Service.Host;
using Service.Host.Services;

namespace Service.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var service = new Service.Host.ServiceReference1.ObjectTransferServiceClient())
            {
                //service.ClientCredentials.UserName.UserName = @"wssdom\Администратор";
                //service.ClientCredentials.UserName.Password = @"P@ssw0rd";
                Generator generator = new Generator();
                Console.WriteLine(service.CreateList(generator.GenerateOwner()));
            }
            Console.ReadLine();
        }
    }
}
