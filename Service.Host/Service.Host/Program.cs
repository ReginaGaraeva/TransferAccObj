using ObjectTransferWCF;
using ObjectTransferWCF.Models;
using ObjectTransferWCF.Services;
using Service.Host;
using Service.Host.Models;
using Service.Host.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using WSSC.V4.DMS.Workflow;
using WSSC.V4.SYS.DBFramework;

namespace Service.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var service = new Service.Host.ServiceReference1.ObjectTransferServiceClient())
            {
                //service.ClientCredentials.UserName.UserName = @"wssdom\Администратор";
                //service.ClientCredentials.UserName.Password = @"P@$$w0rd";
                Console.WriteLine("Создаю тест");
                Test test = new Test(1, 10);
                Console.WriteLine("Запускаю тест");
                test.Run();
                //Service.Host.ServiceReference1.MethodModel[] m  = service.GetMethodsInfo();
                //foreach (var item in m)
                //{
                //    Console.WriteLine(item.Signature);
                //}
            }
            Console.ReadLine();
        }
    }
}
