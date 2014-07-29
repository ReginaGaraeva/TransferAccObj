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

            Generator generator = new Generator();
            WSSList objectList = new WSSList();
            LogService logService = new LogService();
            string inventaryNumber = generator.GenerateInventaryNumber(7), description = generator.GenerateDescription(),
                owner = generator.GenerateOwner();
            DateTime postingDate = Convert.ToDateTime(generator.GetDate()), deprecationDate = Convert.ToDateTime(generator.GetDate());


            objectList.Update("10384848", new ObjectTransferWCF.Models.AccountingObjectModel()
            {
                InventaryNumber = inventaryNumber,
                Description = description,
                PostingDate = postingDate,
                DeprecationDate = deprecationDate,
                Owner = owner,
                Deleted = false
            });
            Console.WriteLine("Вызываю запись лога...");
            logService.WriteInfo(String.Format("Обновлен объект учета\nИнвентарный номер: {0}\nОписание: {1}\nДата оприходования: {2}\nДата амортизации: {3}\nМОЛ: {4}",
            inventaryNumber, description, postingDate, deprecationDate, owner));
            Console.WriteLine("Лог записан");
            //objectList.Add(new ObjectTransferWCF.Models.AccountingObjectModel()
            //{
            //    InventaryNumber = generator.GenerateInventaryNumber(7),
            //    Description = generator.GenerateDescription(),
            //    PostingDate = Convert.ToDateTime(generator.GetDate()),
            //    DeprecationDate = Convert.ToDateTime(generator.GetDate()),
            //    Owner = generator.GenerateOwner(),
            //    Deleted = false
            //});

            Console.ReadLine();
        }
    }
}
