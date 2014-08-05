using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ObjectTransferWCF.Models
{
    [Serializable]
    public class MethodModel
    {
        public string Signature;
        public string[] Params;
        public string Info;

        public MethodModel() { }
    }
}