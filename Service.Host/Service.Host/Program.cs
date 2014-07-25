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

namespace Service.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            //using (var service = new Service.Host.ServiceReference1.ObjectTransferServiceClient())
            //{
            //    //service.ClientCredentials.UserName.UserName = @"wssdom\Администратор";
            //    //service.ClientCredentials.UserName.Password = @"P@ssw0rd";
            //    Generator generator = new Generator();
            //    Console.WriteLine(service.CreateAccountingObject(generator.GenerateInventaryNumber(7), generator.GenerateDescription(),
            //        generator.GetDate(), generator.GetDate(), generator.GenerateOwner()));
            //}
            //DBSite dbSite = new DBSite("http://wsstest");
            //Console.WriteLine("Соединился с wsstest");
            //DBWeb dbWeb = dbSite.GetWeb("/dms/requests");
            //Console.WriteLine("Загрузил страницу /dms/requests");
            //Console.WriteLine(dbWeb.ToString());
            //DBList dbList = dbWeb.GetList("AccountingObjects");
            //Console.WriteLine("Загрузил список объектов учета");
            //DMSContext dmsContext = new DMSContext(dbWeb);
            //Console.WriteLine("Создал dmsContext");
            //DMSLogic dmsLogic = new DMSLogic();
            //Console.WriteLine("Создал dmsLogic");
            //Console.WriteLine(String.Format("Печатаю список объектов учета {0}...",dbList.ItemsCount));
            //foreach (var l in dbList.Items)
            //{
            //    Console.WriteLine(l.ToString());
            //}
            //Console.WriteLine("Напечатал ^^");
            Generator generator = new Generator();
            WSSList objectList = new WSSList();
            objectList.Add(new ObjectTransferWCF.Models.AccountingObjectModel()
            {
                InventaryNumber = generator.GenerateInventaryNumber(7),
                Description = generator.GenerateDescription(),
                PostingDate = Convert.ToDateTime(generator.GetDate()),
                DeprecationDate = Convert.ToDateTime(generator.GetDate()),
                Owner = generator.GenerateOwner(),
                Deleted = false
            });
            Console.WriteLine("Создал объект учета");

            Console.ReadLine();
        }
    }
}
