using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var service = new TestClient.ServiceReference1.Service1Client())
            {
                Console.WriteLine(service.GetData(3));
                Console.ReadLine();
                service.Close();
            }
        }
    }
}
