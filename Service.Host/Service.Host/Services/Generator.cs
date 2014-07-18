using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Host.Interfaces;

namespace Service.Host.Services
{
    class Generator : IGenerator
    {
        private readonly char[] enLetters =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q',
            'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i',
            'j', 'k', 'l', 'm', 'n', 'o', 'p', 'r', 's', 'q', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };

        private readonly char[] ruLetters =
        {
            'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'П',
            'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ', 'Э', 'Ю', 'Я', 'а', 
            'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с',
            'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я'
        };

        private Random rnd = new Random();

        public string GetWord(bool ru, int maxLength = 0)
        {
            string res = "";
            if (maxLength == 0) 
                maxLength = rnd.Next(100)+1;
            if (ru)
                for (int i = 0; i < maxLength; i++)
                {
                    res += ruLetters[rnd.Next(enLetters.Count())];
                }
            else
                for (int i = 0; i < maxLength; i++)
                {
                    res += enLetters[rnd.Next(enLetters.Count())];
                }
            return res;
        }

        public string GetString(string language, int countWords = 0)
        {
            string res = GetWord(language == "ru");
            if (countWords == 0)
                countWords = rnd.Next(100);
            for (int i = 0; i < countWords; i++)
            {
                res += ' ' + GetWord(language == "ru");
            }
            return res;
        }

        public string GetDate()
        {
            return DateTime.Now.AddDays(rnd.Next(1000)).ToShortDateString();
        }

        public string GenerateOwner()
        {
            return GetString("ru", 3) + ' ' + GetDate();
        }

        public string GenerateDescription()
        {
            return GetString("ru");
        }

        public string GenerateInventaryNumber(int length)
        {
            string res = "";
            for (int i = 0; i < length; i++)
                res += (rnd.Next(10) + 1).ToString();
            return res;
        }
    }
}
