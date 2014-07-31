﻿using System;
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
        private DBList dbList;

        public WSSList()
        {           
            DBSite dbSite = new DBSite("http://wsstest");
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
                    dbItem.SetValue("Owner", accObject.Owner);
                    dbItem.SetValue("IsDeleted", accObject.Deleted);
                    dbItem.Update();
                    return 1;
                }
                else
                {
                    return 4;
                }
            }
            catch
            {
                return 3;
            }
        }

        public int Update(string OldInventaryNumber, AccountingObjectModel accObject)
        {
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
                    dbList.Items[i].SetValue("Owner", accObject.Owner);
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
            dbList.Items.Where(x => x.GetValue("InventaryNumber") == inventaryNumber).FirstOrDefault().Delete();            
        }

        public void RollbackUpdate(string inventaryNumber, AccountingObjectModel oldAccObject)
        {
            Update(inventaryNumber, oldAccObject);
        }

        public void RollbackDelete(string inventaryNumber)
        {
            dbList.Items.Where(x => x.GetValue("InventaryNumber") == inventaryNumber).FirstOrDefault().SetValue("IsDeleted", false);
        }

    }
}