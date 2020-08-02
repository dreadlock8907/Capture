using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Capture
{
    public class CharsMap
    {
        static CharsMap _self;

        public static CharsMap self
        {
            get
            {
                if(_self == null)
                    _self = new CharsMap();
                return _self;
            }
        }

        //словарь символов будет
        const string eng= "qwertyuiop[]asdfghjkl;'zxcvbnm,. !?=-";
        const string rus = "йцукенгшщзхъфывапролджэячсмитьбю !?=-";
        
        //а вот тут с помощью словаря - будем использовать по ключу аглицкие символы и соответствующее им значение русских
        Dictionary<char, char> eng2rus;
        Dictionary<char, char> rus2eng;

        CharsMap()
        {
            char[] engList = eng.ToCharArray();
            char[] rusList = rus.ToCharArray();

            //добавим элементы массивов в словари(с - ключ, n - значение)
            for (int i = 0; i < engList.Length; i++)
            {
                eng2rus.Add(engList[i], rusList[i]);
                rus2eng.Add(rusList[i], engList[i]);
            }
        }

        public char MatchEngChar(char rus)
        {
            if(rus2eng.ContainsKey(rus))
                return rus2eng[rus];
            return ' ';
        }

        public char MatchRusChar(char eng)
        {
            if(eng2rus.ContainsKey(eng))
                return eng2rus[eng];
            return ' ';
        }
        
    }
    
    
    public class processingString
    {
        WordsDictionary words_dict;
    
        public processingString()
        {
            words_dict = new WordsDictionary();
        }

        //собственно, сам обработчик
        public string processStr(string str, bool is_rus, bool is_auto)
        {
            List<char> outList = new List<char>(); // результат собираем в лист

            var sB = new StringBuilder(); // тут выведем

            char[] charArr = str.ToCharArray(); //форматнем строку в массив, его и будем перебирать

            List<string> listOfStr = new List<string>();
            
            //если активирована галка...
            if (is_auto)
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
                            if (words_dict.English.Contains(st))
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
                                        sB.Append(CharsMap.self.MatchEngChar(c));
                                    }
                                }
                                sB.Append(" ");

                            }
                        }
                    }
                    else
                    {
                        if (words_dict.English.Contains(str))
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
                                    sB.Append(CharsMap.self.MatchEngChar(c));
                            }
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
                            if (words_dict.Russian.Contains(st))
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
                                        sB.Append(CharsMap.self.MatchRusChar(c));
                                }
                                sB.Append(" ");

                            }
                        }
                    }
                }
            }
            //если отключена автоматическая галка
            else
            {
                try
                {
                    foreach (char c in charArr) //во всех парах ключ-значение ищем совпадение
                    {
                        if (is_rus) //проверка на то, русский ли режим
                        {
                            if ((int)c > 1000) //если символ и так русский, то просто добавляем в лист
                                outList.Add(c);
                            else //иначе ищем значение в паре в словаре
                                outList.Add(CharsMap.self.MatchEngChar(c));
                        }
                        else //если английский, то ищем в словаре, значит, английском
                        {
                            if ((int)c < 1000) //если символ и так английский, то просто добавляем в лист
                                outList.Add(c);
                            else //иначе ищем значение в паре в словаре
                                outList.Add(CharsMap.self.MatchRusChar(c));
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
