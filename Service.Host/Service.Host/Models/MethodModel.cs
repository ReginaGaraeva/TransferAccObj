using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Host.Models
{
    [Serializable]
    public class MethodModel
    {
        public string Signature;
        public string Params;
        public string Info;

        public MethodModel() { }
    }
}
