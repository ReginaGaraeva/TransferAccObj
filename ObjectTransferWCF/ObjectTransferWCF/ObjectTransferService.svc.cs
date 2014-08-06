using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WSSC.V4.SYS.DBFramework;
using WSSC.V4.DMS.Workflow;
using ObjectTransferWCF.Services;
using System.ServiceModel.Activation;
using ObjectTransferWCF.Models;
using System.IO;
using System.Xml.Serialization;

namespace ObjectTransferWCF
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ObjectTransferService : IObjectTransferService
    {
        WSSList objectList;
        LogService logService;
        Dictionary<int, ResponseModel> responseList = new Dictionary<int, ResponseModel>();

        #region ResponseList
        private void InitResponseList()
        {
            responseList.Add(1, new ResponseModel()
            {
                Message = "Accounting object successfully created.",
                isError = false
            });
            responseList.Add(2, new ResponseModel()
            {
                Message = "Fail in creating accounting object.",
                isError = true
            });
            responseList.Add(3, new ResponseModel()
            {
                Message = "Error with WSS.",
                isError = true
            });
            responseList.Add(4, new ResponseModel()
            {
                Message = "Accounting object already exists.",
                isError = false
            });
            responseList.Add(5, new ResponseModel()
            {
                Message = "Accounting object successfully updated.",
                isError = false
            });
            responseList.Add(6, new ResponseModel()
            {
                Message = "Fail in updating accounting object.",
                isError = true
            });
            responseList.Add(7, new ResponseModel()
            {
                Message = "Accounting object does not exist. Accounting object sucsessfully created.",
                isError = false
            });
            responseList.Add(8, new ResponseModel()
            {
                Message = "Accounting object does not exist. Accounting object was not create.",
                isError = true
            });
            responseList.Add(9, new ResponseModel()
            {
                Message = "Accounting object successfully deleted.",
                isError = false
            });
            responseList.Add(10, new ResponseModel()
            {
                Message = "Fail in deleting accounting object.",
                isError = true
            });
             responseList.Add(11, new ResponseModel()
            {
                Message = "Fail in deleting accounting object. Object not found.",
                isError = true
            });
             responseList.Add(12, new ResponseModel()
             {
                 Message = "Connection failed. Database is not available.",
                 isError = true
             });

        }
        #endregion

        public MethodModel[] GetMethodsInfo()
        {
            //XmlSerializer formatter = new XmlSerializer(typeof(MethodModel[]));
            ////десериализация
            //using (FileStream fs = new FileStream("persons.xml", FileMode.OpenOrCreate))
            //{
            //    MethodModel[] piy = (Person[])formatter.Deserialize(fs);

            //    foreach (Person p in newPersons)
            //    {
            //        Console.WriteLine("Имя: {0} --- Возраст: {1}", p.Name, p.Age);
            //    }
            //}
            return new MethodModel[1];
        }
        public ObjectTransferService()
        {
            try
            {
                objectList = new WSSList();
            }
            catch(Exception ex)
            {
                throw new Exception("Не удалось создать список объектов учета. "+ex.Message);
            }
            logService = new LogService();
            InitResponseList();           
        }


        private bool CheckDbConnect()
        {           
            using (var context = new ObjectTransferDBEntities())
            {
                try
                {
                    context.Database.Connection.Open();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }


        public string CreateAccountingObject(string inventaryNumber, string description, string postingDate,
            string deprecationDate, string owner)
        {
            try
            {              
                if (!CheckDbConnect())
                    return responseList[12].Message;
                int response = 
                    objectList.Add(new Models.AccountingObjectModel()
                    {
                        InventaryNumber = inventaryNumber,
                        Description = description,
                        PostingDate = Convert.ToDateTime(postingDate),
                        DeprecationDate = Convert.ToDateTime(deprecationDate),
                        Owner = owner,
                        Deleted = false
                    });
                try
                {
                    logService.WriteInfo(String.Format(responseList[response].Message + ". Inventary number: {0}. Description: {1}. Posting date: {2}. Deprecation date: {3}. Owner: {4}.",
                        inventaryNumber, description, postingDate, deprecationDate, owner));
                }
                catch
                {
                    if (response == 1) objectList.RollbackCreate(inventaryNumber);
                    return responseList[12].Message;//ошибка соединения с базой данных
                }
                return responseList[response].Message;//объект учета успешно создан
            }
            catch
            {
                try
                {
                    logService.WriteInfo(String.Format(responseList[2].Message + ". Inventary number: {0}. Description: {1}. Posting date: {2}. Deprecation date: {3}. Owner: {4}.",
                        inventaryNumber, description, postingDate, deprecationDate, owner));
                }
                catch
                {
                    return responseList[12].Message;//ошибка соединения с базой данных
                }
                return responseList[2].Message;//ошибка создания объекта учета
            }
            
        }

        public string UpdateAccountingObject(string oldInventaryNumber, string inventaryNumber, string description, string postingDate,
            string deprecationDate, string owner)
        {
            try
            {
                if (!CheckDbConnect())
                    return responseList[12].Message;
                //Ниже получается двойная работа, ищем объект учета по инвентарному номеру, чтобы в случае ошибки сделать откат изменений,
                //но в Update тоже выполняется поиск этого объекта в Exists. Надо что-то как-то переделать)) 
                AccountingObjectModel oldAccObj = objectList.GetItem(oldInventaryNumber);
                int response = 
                    objectList.Update(oldInventaryNumber, new Models.AccountingObjectModel()
                    {
                        InventaryNumber = inventaryNumber,
                        Description = description,
                        PostingDate = Convert.ToDateTime(postingDate),
                        DeprecationDate = Convert.ToDateTime(deprecationDate),
                        Owner = owner,
                        Deleted = false
                    });
                try
                {
                    logService.WriteInfo(String.Format(responseList[response].Message + ". Object's inventary number: {0}. New data: inventary number: {1}. Description: {2}. Posting date: {3}. Deprecation date: {4}. Owner: {5}.",
                        oldInventaryNumber, inventaryNumber, description, postingDate, deprecationDate, owner));
                }
                catch
                {
                    if (response == 5) objectList.RollbackUpdate(inventaryNumber, oldAccObj);
                    return responseList[12].Message;//ошибка соединения с базой данных
                }
                return responseList[response].Message;//объект учета успешно обновлен
            }
            catch
            {
                try
                {
                    logService.WriteInfo(String.Format(responseList[6].Message + ". Object's inventary number: {0}. New data: inventary number: {1}. Description: {2}. Posting date: {3}. Deprecation date: {4}. Owner: {5}.",
                        oldInventaryNumber, inventaryNumber, description, postingDate, deprecationDate, owner));
                }
                catch
                {
                    return responseList[12].Message;//ошибка соединения с базой данных
                }
                return responseList[6].Message;//не удалось обновить объект учета
            }
        }

        public string DeleteAccountingObject(string inventaryNumber)
        {
            try
            {
                if (!CheckDbConnect())
                    return responseList[12].Message;
                int response = 
                    objectList.Delete(inventaryNumber);
                try
                {
                    logService.WriteInfo(String.Format(responseList[response].Message + ". Object's inventary number: {0}", inventaryNumber));                   
                }
                catch
                {
                    if (response == 9) objectList.RollbackDelete(inventaryNumber);
                    return responseList[12].Message;//ошибка соединения с базой данных
                }
                return responseList[response].Message;//объект удален успешно
            }
            catch
            {
                try
                {
                    logService.WriteInfo(String.Format(responseList[10].Message + ". Object's inventary number: {0}", inventaryNumber));
                }
                catch
                {
                    return responseList[12].Message;//ошибка соединения с базой данных
                }
                return responseList[10].Message;//ошибка соединения с базой данных
            }
        }
        
    }
}
