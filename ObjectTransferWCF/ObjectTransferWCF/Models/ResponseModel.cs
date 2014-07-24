using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ObjectTransferWCF.Models
{
    public class ResponseModel
    {
        public string Message {get; set;}
        public bool isError { get; set; }
    }
}