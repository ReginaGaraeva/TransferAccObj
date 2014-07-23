using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ObjectTransferWCF.Models
{
    public class AccountingObjectModel
    {
        public string InventaryNumber { get; set; }
        public string Description { get; set; }
        public DateTime PostingDate { get; set; }
        public DateTime DeprecationDate { get; set; }
        public string Owner { get; set; }
        public bool Deleted { get; set; }
    }
}