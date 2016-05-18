using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Capture
{

    //задача класса считать файлы со словами и перенести их во внутренние ресурсы памяти
    class myDictionary
    {
        private string _path = Environment.CurrentDirectory + "\\Dictionaries";
        private string _pathEng = Environment.CurrentDirectory + "\\Dictionaries\\eng.txt";
        private string _pathRus = Environment.CurrentDirectory + "\\Dictionaries\\rus.txt";  //путь к файлам

        private List<string> engList = new List<string>();
        private List<string> rusList = new List<string>();

        //определим свойства чтения списков слов из класса
        public List<string> English
        {
            get { return engList; }
        }
        public List<string> Russian
        {
            get { return rusList; }
        }
        //конструктор класса, при вызове экземпляра будет запускаться процесс заполнения файлов листа с сортировкой
        public myDictionary()
        {
            DirectoryInfo direct = new DirectoryInfo(_path);
            FileInfo engFile = new FileInfo(_path + "\\eng.txt");
            FileInfo rusFile = new FileInfo(_path + "\\rus.txt");

            if (!direct.Exists)
            {
                direct.Create();
                engFile.Create();
                rusFile.Create();
            }
            else if (engFile.Exists && rusFile.Exists)
            {
                readFiles();
                engList = formattingDictionary(engList);
            }
            
        }

        //метод прочитает файлы и запишет в листы
        private void readFiles()
        {
            try
            {
                FileStream fsEng = new FileStream(_pathEng, FileMode.Open);
                FileStream fsRus = new FileStream(_pathRus, FileMode.Open);

                StreamReader srEng = new StreamReader(fsEng);
                StreamReader srRus = new StreamReader(fsRus);

                if (engList.Count == 0 && rusList.Count == 0)
                {
                    while (!srEng.EndOfStream)
                    { 
                        engList.Add(srEng.ReadLine());
                    }
                    while (!srRus.EndOfStream)
                    {
                        rusList.Add(srRus.ReadLine());
                    }
                }

                    srEng.Close();
                    srRus.Close();

                    fsEng.Close();
                    fsRus.Close();
            }
            catch
            {
                Exception ex = new Exception();
                string msg = ex.Message;
            }
        }
        private List<string> formattingDictionary(List<string> l)
        {
            List<string> formatList = new List<string>();
            foreach (string str in l)
            {
                try
                {
                    formatList.Add(str.Remove(str.IndexOf(' ')));
                }
                catch
                {
                    Exception ex = new Exception();
                    string errMsg = ex.Message;
                }
            }
            return formatList; 
        }

    }
}
