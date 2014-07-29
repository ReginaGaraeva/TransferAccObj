using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WSSC;

namespace ObjectTransferWCF
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract(Namespace = "http://objecttransferservice.ru/")]
    public interface IObjectTransferService
    {
        
        [OperationContract]
        string CreateAccountingObject(string inventaryNumber, string description, string postingDate,
            string deprecationDate, string owner);

        [OperationContract]
        string UpdateAccountingObject(string oldInventaryNumber, string inventaryNumber, string description, string postingDate,
            string deprecationDate, string owner);

        [OperationContract]
        string DeleteAccountingObject(string inventaryNumber);

        //[OperationContract]
        //string GetData(int value);

        //[OperationContract]
        //CompositeType GetDataUsingDataContract(CompositeType composite);

        //// TODO: Добавьте здесь операции служб


    }


    // Используйте контракт данных, как показано в примере ниже, чтобы добавить составные типы к операциям служб.
    //[DataContract]
    //public class CompositeType
    //{
    //    bool boolValue = true;
    //    string stringValue = "Hello ";

    //    [DataMember]
    //    public bool BoolValue
    //    {
    //        get { return boolValue; }
    //        set { boolValue = value; }
    //    }

    //    [DataMember]
    //    public string StringValue
    //    {
    //        get { return stringValue; }
    //        set { stringValue = value; }
    //    }
    //}
}
