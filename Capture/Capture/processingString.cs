using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Capture
{
    public class processingString
    {
        //словарь символов будет
        private string eng = "qwertyuiop[]asdfghjkl;'zxcvbnm,. !?=-";
        private string rus = "йцукенгшщзхъфывапролджэячсмитьбю !?=-";

        //а вот тут с помощью словаря - будем использовать по ключу аглицкие символы и соответствующее им значение русских
        Dictionary<char, char> dictEngToRus = new Dictionary<char, char>();
        Dictionary<char, char> dictRusToEng = new Dictionary<char, char>();

        myDictionary mD = new myDictionary();

        //вот он, конструктор-то для чегооо    
        public processingString()
        {
            //запилим строки в массив символов 
            char[] engList = eng.ToCharArray();
            char[] rusList = rus.ToCharArray();

            //добавим элементы массивов в словари(с - ключ, n - значение)
            for (int i = 0; i < engList.Length; i++)
            {
                dictEngToRus.Add(engList[i], rusList[i]);
                dictRusToEng.Add(rusList[i], engList[i]);
            }
        }

        //собственно, сам обработчик
        public string processStr(string str, bool isRus, bool isAuto)
        {
            List<char> outList = new List<char>(); // результат собираем в лист

            var sB = new StringBuilder(); // тут выведем

            char[] charArr = str.ToCharArray(); //форматнем строку в массив, его и будем перебирать

            List<string> listOfStr = new List<string>();
            
            //если активирована галка...
            if (isAuto)
            {
                //если английская раскладка
                if (InputLanguage.CurrentInputLanguage.Culture.EnglishName == "English (United States)")
                {
                    //нужно, чтобы распознавалась фраза...
                    if (str.Contains(' '))
                    {
                        string[] mystr = str.Split(' ');

                        foreach (string st in mystr)
                        {
                            if (mD.English.Contains(st))
                            {
                                sB.Append(st + ' ');
                            }
                            else
                            {
                                
                                foreach (char c in st)
                                {
                                    if ((int)c > 1000)
                                        sB.Append(c);
                                    else
                                    {
                                        foreach (KeyValuePair<char, char> kvp in dictEngToRus)
                                        {
                                            if (c == kvp.Key)
                                            {
                                                sB.Append(kvp.Value);
                                            }
                                        }
                                    }
                                }
                                sB.Append(" ");

                            }
                        }
                        return sB.ToString();
                    }
                    else
                    {
                        if (mD.English.Contains(str))
                        {
                            return str;
                        }
                        else
                        {
                            foreach (char c in charArr)
                            {
                                if ((int)c > 1000)
                                    sB.Append(c);
                                else
                                {
                                    foreach (KeyValuePair<char, char> kvp in dictEngToRus)
                                    {
                                        if (c == kvp.Key)
                                        {
                                            sB.Append(kvp.Value);
                                        }
                                    }
                                }
                            }
                            //return sB.ToString();
                        }
                    }

                }
                //если раскладка русская
                else
                {
                    //нужно, чтобы распознавалась фраза...
                    if (str.Contains(' '))
                    {
                        string[] mystr = str.Split(' ');

                        foreach (string st in mystr)
                        {
                            if (mD.Russian.Contains(st))
                            {
                                sB.Append(st + ' ');
                            }
                            else
                            {

                                foreach (char c in st)
                                {
                                    if ((int)c < 1000)
                                        sB.Append(c);
                                    else
                                    {
                                        foreach (KeyValuePair<char, char> kvp in dictRusToEng)
                                        {
                                            if (c == kvp.Key)
                                            {
                                                sB.Append(kvp.Value);
                                            }
                                        }
                                    }
                                }
                                sB.Append(" ");

                            }
                        }
                        //return sB.ToString();
                    }
                }
                return sB.ToString();
            }
            //если отключена автоматическая галка
            else
            {
                try
                {
                    foreach (char c in charArr) //во всех парах ключ-значение ищем совпадение
                    {
                        if (isRus) //проверка на то, русский ли режим
                        {
                            if ((int)c > 1000) //если символ и так русский, то просто добавляем в лист
                                outList.Add(c);
                            else //иначе ищем значение в паре в словаре
                            {
                                foreach (KeyValuePair<char, char> kvp in dictEngToRus)
                                {
                                    if (c == kvp.Key)
                                    {
                                        outList.Add(kvp.Value);
                                    }
                                }
                            }
                        }
                        else //если английский, то ищем в словаре, значит, английском
                        {
                            if ((int)c < 1000) //если символ и так английский, то просто добавляем в лист
                                outList.Add(c);
                            else //иначе ищем значение в паре в словаре
                            {
                                foreach (KeyValuePair<char, char> kvp in dictRusToEng)
                                {
                                    if (c == kvp.Key)
                                    {
                                        outList.Add(kvp.Value);
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                    Exception ex = new Exception();
                    Console.WriteLine(ex.Message);
                }
                //вывод
                foreach (char c in outList)
                {
                    sB.Append(c);
                }
                return sB.ToString();
            }

                return sB.ToString();
            }
        

        //метод проверит введенные символы в строке и посчитает на какой язык перевести
        private bool isEng(string str)
        {

            StringBuilder strRus = new StringBuilder();
            StringBuilder strEng = new StringBuilder();

            char[] res = str.ToCharArray();

            foreach (char c in res)
            {
                if ((int)c > 1000)
                {
                    strRus.Append(c);
                }
                else if ((int)c < 1000)
                {
                    strEng.Append(c);
                }                
            }
            if (strEng.Length >= strRus.Length)
            {
                return true;
            }
            else
                return false;
        }
    }
}
