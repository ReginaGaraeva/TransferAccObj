using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WSSC.V4.DMS.Workflow;
using WSSC.V4.SYS.DBFramework;
using ObjectTransferWCF.Models;

namespace ObjectTransferWCF.Services
{
    public class WSSList
    {
        DBSite dbSite;
        private DBList dbList;

        public WSSList()
        {           
            dbSite = new DBSite("http://wsstest");
            DBWeb dbWeb = dbSite.GetWeb("/dms/requests");
            dbList = dbWeb.GetList("AccountingObjects");
        }

        public int Exists(string InventaryNumber)
        {
            for (int i = 0; i < dbList.ItemsCount; i++)
            {
                if (dbList.Items[i].GetValue("InventaryNumber").ToString() == InventaryNumber)
                    return i;
            }
            return -1;
        }

        public int Add(AccountingObjectModel accObject)
        {
            HttpContext.Current = null;
            try
            {
                int i = Exists(accObject.InventaryNumber);
                if (i == -1)
                {
                    DBItem dbItem = dbList.CreateItem();
                    dbItem.SetValue("InventaryNumber", accObject.InventaryNumber);
                    dbItem.SetValue("Description", accObject.Description);
                    dbItem.SetValue("PostingDate", accObject.PostingDate);
                    dbItem.SetValue("DeprecationDate", accObject.DeprecationDate);
                    dbItem.SetValue("Owner", GetOwnerID(accObject.Owner));
                    dbItem.SetValue("IsDeleted", accObject.Deleted);
                    dbItem.Update(); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    return 1;
                }
                else
                {
                    return 4;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //return 3;
            }          
        }

        public int Update(string OldInventaryNumber, AccountingObjectModel accObject)
        {
            HttpContext.Current = null;
            int i = Exists(OldInventaryNumber);
            if (i == -1)
            {
                if (Add(accObject) == 1)
                    return 7;
                else
                    return 8;
            }
            else
            {
                try
                {
                    dbList.Items[i].SetValue("InventaryNumber", accObject.InventaryNumber);
                    dbList.Items[i].SetValue("Description", accObject.Description);
                    dbList.Items[i].SetValue("PostingDate", accObject.PostingDate);
                    dbList.Items[i].SetValue("DeprecationDate", accObject.DeprecationDate);
                    dbList.Items[i].SetValue("Owner", GetOwnerID(accObject.Owner));
                    dbList.Items[i].SetValue("IsDeleted", accObject.Deleted);
                    dbList.Items[i].Update();
                    return 5;
                }
                catch
                {
                    return 6;
                }
            }
        }

        public int Delete(string InventaryNumber)
        {
            HttpContext.Current = null;
            int i = Exists(InventaryNumber);
            if (i == -1)
            {
                return 11;
            }
            else
            {
                try
                {
                    dbList.Items[i].SetValue("IsDeleted", true);
                    dbList.Items[i].Update();
                    return 9;
                }
                catch
                {
                    return 10;
                }
            }
            
        }

        public void RollbackCreate(string inventaryNumber)
        {
            dbList.Items.Where(x => x.GetValue("InventaryNumber").ToString() == inventaryNumber).FirstOrDefault().Delete();            
        }

        public void RollbackUpdate(string inventaryNumber, AccountingObjectModel oldAccObject)
        {
            Update(inventaryNumber, oldAccObject);
        }

        public void RollbackDelete(string inventaryNumber)
        {
            dbList.Items.Where(x => x.GetValue("InventaryNumber").ToString() == inventaryNumber).FirstOrDefault().SetValue("IsDeleted", false);
        }

        public AccountingObjectModel GetItem(string inventaryNumber)
        {
            DBItem dbItem = dbList.Items.Where(x => x.GetValue("InventaryNumber") == inventaryNumber).FirstOrDefault();
            if (dbItem != null)
                return new AccountingObjectModel()
                {
                    InventaryNumber = dbItem.GetValue("InventaryNumber").ToString(),
                    Description = dbItem.GetValue("Desription").ToString(),
                    PostingDate = Convert.ToDateTime(dbItem.GetValue("PostingDate")),
                    DeprecationDate = Convert.ToDateTime(dbItem.GetValue("DeprecationDate")),
                    Owner = dbItem.GetValue("Owner").ToString(),
                    Deleted = Convert.ToBoolean(dbItem.GetValue("IsDeleted"))
                };
            else
                return null;
        }

        private int? GetOwnerID(string fio)
        {
            DBWeb dbWeb = dbSite.GetWeb("/");           
            DBList dbList = dbWeb.GetList("Users");            
            var users = dbList.Items.Where(x => x.GetValue("Имя пользователя").ToString() == fio).ToList();           
            if (users.Count() > 1)
                return null;
            else if (users.Count() == 0)
                return -1;
            else
                return Convert.ToInt32(users.First().GetValue("ID"));
        }

    }
}