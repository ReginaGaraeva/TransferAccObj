using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Host.Services;
using Service.Host.ServiceReference1;
using System.Threading;

namespace Service.Host.Models
{
    class ThreadModel
    {
        public ObjectTransferServiceClient service;
        public Thread thread;
    }
}
