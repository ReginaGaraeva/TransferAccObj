using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Host.Interfaces
{
    interface IGenerator
    {
        string GetWord(bool ru, int maxLength = 0);
        string GetString( string language, int countWords = 0);
        string GetDate();
    }
}
