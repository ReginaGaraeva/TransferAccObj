using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Service.Host;
using Service.Host.Services;
using Service.Host.Models;
using WSSC.V4.SYS.DBFramework;
using WSSC.V4.DMS.Workflow;
using ObjectTransferWCF.Services;
using System.ServiceModel.Activation;
using ObjectTransferWCF.Models;
using ObjectTransferWCF;

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
                Test test = new Test(2, 5);
                Console.WriteLine("Запускаю тест");
                test.Run();
                //Generator generator = new Generator();
                //Console.WriteLine(service.CreateAccountingObject(generator.GenerateInventaryNumber(7), generator.GenerateDescription(),
                //    generator.GetDate(), generator.GetDate(), "Кижапкина Елена" ));
            }
            Console.ReadLine();
        }
    }
}
