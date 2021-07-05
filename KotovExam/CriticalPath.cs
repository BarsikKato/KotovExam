using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace KotovExam
{
    public class CriticalPath
    {
        string readPath;
        string savePath;

        public struct Activity //Структура работ (дуг)
        {
            public string eventStart, eventEnd;
            public int time;
        }
        public struct Path //Структура путей
        {
            public string path, lastPoint;
            public int length;
        }

        /// <summary>
        /// Конструктор класса решения критического пути.
        /// </summary>
        public CriticalPath(string readPath, string savePath)
        {
            this.readPath = readPath;
            this.savePath = savePath;
        }

        public List<Activity> activities = new List<Activity>(); //Список всех работ (в графике это дуги)
        public List<Path> pathes = new List<Path>(); //Список всех путей

        /// <summary>
        /// Метод считывания данных из файла.
        /// </summary>
        public void ReadData()
        {
            if (!File.Exists(readPath))
            {
                MessageBox.Show("Файл не найден!");
                Environment.Exit(0);
            }
            var lines = File.ReadAllLines(readPath);
            try
            {
                foreach (var line in lines)
                {
                    string[] str = line.Split(';');
                    activities.Add(new Activity { eventStart = str[0], eventEnd = str[1], time = Convert.ToInt32(str[2]) });
                    Debug.WriteLine("Считывание строки {0} успешно произведено.", activities.Count);
                }
                Debug.WriteLine("Файл по пути успешно считан.", readPath);
            }
            catch
            {
                MessageBox.Show("Неверный формат записи данных!");
                Debug.WriteLine("Не удалось считать файл по пути {0}.", readPath);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Метод записи в файл.
        /// </summary>
        /// <param name="savingPath"></param>
        public void WriteToFile(List<Path> savingPath)
        {
            if (!File.Exists(savePath)) File.Create(savePath).Close();
            try
            {
                using (StreamWriter sw = new StreamWriter(savePath, false, UnicodeEncoding.UTF8))
                {
                    if (savingPath.Count == 1) //Для записи одного пути
                    {
                        sw.WriteLine("Найденный критический путь имеет вид:");
                        sw.WriteLine(savingPath[0].path);
                        sw.WriteLine("Его длина составляет: " + savingPath[0].length);
                    }
                    else //Для записи нескольких путей
                    {
                        sw.WriteLine("Найденные критические пути имеют вид:");
                        foreach (Path savPath in savingPath)
                            sw.WriteLine(savPath.path);
                        sw.WriteLine("Длина каждого из них составляет: " + savingPath[0].length);
                    }
                    Debug.WriteLine("Файл успешно записан по пути.", savePath);
                }
            }
            catch
            {
                Debug.WriteLine("Не удалось записать файл по пути.", savePath);
                MessageBox.Show("Не удалось записать данные в файл!");
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Метод поиска стартовой точки.
        /// </summary>
        /// <returns></returns>
        public string FindStartingPos() //Метод для поиска начальной точки
        {
            string tempStartPos = " ", lastPoint = "";
            int countCheck = 0;
            foreach (Activity activity in activities)   //Если нет таких дуг, которые бы входили в данную точку, то она начальная.
            {
                if (activities.Where(x => x.eventEnd == activity.eventStart).Count() == 0)
                {
                    tempStartPos = activity.eventStart;
                    countCheck++;
                    if (countCheck > 1 && lastPoint != activity.eventStart) //Проверка на несколько начальных точек.
                    {
                        MessageBox.Show("В введенных данных присутствует несколько начальных точек. Решение невозможно.");
                        Environment.Exit(0);
                    }
                    lastPoint = activity.eventStart;
                }
            }
            if (countCheck == 0) //Проверка на отсутствие начальной точки.
            {
                MessageBox.Show("Начальная точка отсутствует.");
                Environment.Exit(0);
            }
            Debug.WriteLine("Стартовая позиция найдена в точке " + tempStartPos);
            return tempStartPos;
        }

        /// <summary>
        /// Метод поиска конечной точки.
        /// </summary>
        /// <returns></returns>
        public string FindEndingPos() //Метод для поиска конечной точки
        {
            string tempEndPos = "", lastPoint = "";
            int countCheck = 0;
            foreach (Activity activity in activities)   //Если нет таких дуг, которые бы исходили из данной точки, то она конечная.
            {
                if (activities.Where(x => x.eventStart == activity.eventEnd).Count() == 0)
                {
                    tempEndPos = activity.eventEnd;
                    countCheck++;
                    if (countCheck > 1 && lastPoint != activity.eventEnd) //Проверка на несколько конечных точек.
                    {
                        MessageBox.Show("В введенных данных присутствует несколько конечных точек. Решение невозможно.");
                        Environment.Exit(0);
                    }
                    lastPoint = activity.eventEnd;
                }
            }
            if (countCheck == 0) //Проверка на отсутствие конечной точки.
            {
                MessageBox.Show("Конечная точка отсутствует.");
                Environment.Exit(0);
            }
            Debug.WriteLine("Конечная позиция найдена в точке " + tempEndPos);
            return tempEndPos;
        }

        /// <summary>
        /// Метод подсчета путей.
        /// </summary>
        public void CalculatePathes() //Метод подсчета путей
        {
            foreach (Activity activity in activities.Where(x => x.eventStart == FindStartingPos())) //Сначала в список путей заносятся все начальные дуги
            {
                pathes.Add(new Path { path = activity.eventStart + "--" + activity.eventEnd, lastPoint = activity.eventEnd, length = activity.time });
                Debug.WriteLine("Промежуточный путь записан: " + pathes[pathes.Count - 1].path);
            }
            for (int i = 0; i < pathes.Count; i++) //Затем программа начинает обход по всем записанным путям (в ходе выполнения цикла их количество пополняется)
            {
                foreach (Activity activity in activities.Where(x => x.eventStart == pathes[i].lastPoint)) //В список путей заносятся новые пути, которые исходят из проверяемого в данных момент
                {
                    //Таким образом в список заносятся все промежуточные пути, зато работает
                    pathes.Add(new Path { path = pathes[i].path + "--" + activity.eventEnd, lastPoint = activity.eventEnd, length = pathes[i].length + activity.time });
                    Debug.WriteLine("Промежуточный путь записан: " + pathes[pathes.Count - 1].path);
                }
            }
        }

        /// <summary>
        /// Метод поиска критического пути.
        /// </summary>
        /// <returns></returns>
        public List<Path> FindCriticalPath() //Метод поиска критического пути
        {
            int maxLength = 0;
            string endPos = FindEndingPos();
            foreach (Path path in pathes.Where(x => x.lastPoint == endPos)) //Проверяет все пути, конечная точка которых совпадает с концом сети
            {
                Debug.WriteLine("Текущая длина пути {0}, текущая максимальная длина {1}", path.length, maxLength);
                if (path.length > maxLength) maxLength = path.length; //Вычисляет самый длинный путь из представленных
            }
            Debug.WriteLine("Найденная максимальная длина равна {0}.", maxLength);
            List<Path> criticalPath = new List<Path>();
            foreach (Path path in pathes.Where(x => x.length == maxLength && x.lastPoint == endPos)) //Заносит в массив критические пути.
            {
                Debug.WriteLine("Путь записан в список критических.", path.path);
                criticalPath.Add(path);
            }
            return criticalPath;
        }

        /// <summary>
        /// Вычислить критический путь.
        /// </summary>
        public void CalculateCriticalPath()
        {
            Debug.Listeners.Add(new TextWriterTraceListener(File.Create("log.txt")));
            Debug.AutoFlush = true;
            ReadData();
            CalculatePathes();
            var criticalPath = FindCriticalPath();
            WriteToFile(criticalPath);
        }
    }
}
