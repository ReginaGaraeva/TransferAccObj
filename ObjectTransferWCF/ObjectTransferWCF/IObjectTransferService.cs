using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WSSC;
using ObjectTransferWCF.Models;

namespace ObjectTransferWCF
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract(Namespace = "http://objecttransferservice.ru/")]
    public interface IObjectTransferService
    {
        [OperationContract]
        MethodModel[] GetMethodsInfo();

        [OperationContract]
        string CreateAccountingObject(string inventaryNumber, string description, string postingDate,
            string deprecationDate, string owner);

        [OperationContract]
        string UpdateAccountingObject(string oldInventaryNumber, string inventaryNumber, string description, string postingDate,
            string deprecationDate, string owner);

        [OperationContract]
        string DeleteAccountingObject(string inventaryNumber);


    }

}
