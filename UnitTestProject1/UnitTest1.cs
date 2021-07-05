using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using KotovExam;
using System.IO;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        //Все эталонные значения взяты на основе решения представленного в файле "Решение. Критический путь.jpg"
        [TestMethod]
        public void TestMethod1() //Тест поиска начальной точки
        {
            CriticalPath cp = new CriticalPath(@"E:\vsprojects\KotovExam\KotovExam\Данные задачи.csv", null);
            cp.ReadData();
            Assert.AreEqual("1", cp.FindStartingPos());
        }
        [TestMethod]
        public void TestMethod2() //Тест поиска конечной точки
        {
            CriticalPath cp = new CriticalPath(@"E:\vsprojects\KotovExam\KotovExam\Данные задачи.csv", null);
            cp.ReadData();
            Assert.AreEqual("7", cp.FindEndingPos());
        }
        [TestMethod]
        public void TestMethod3() //Тест поиска длины критического пути
        {
            CriticalPath cp = new CriticalPath(@"E:\vsprojects\KotovExam\KotovExam\Данные задачи.csv", null);
            cp.ReadData();
            cp.CalculatePathes();
            var criticalPath = cp.FindCriticalPath();
            Assert.AreEqual(29, criticalPath[0].length);
        }
        [TestMethod]
        public void TestMethod4() //Тест последней точки
        {
            CriticalPath cp = new CriticalPath(@"E:\vsprojects\KotovExam\KotovExam\Данные задачи.csv", null);
            cp.ReadData();
            cp.CalculatePathes();
            var criticalPath = cp.FindCriticalPath();
            Assert.AreEqual(cp.FindEndingPos(), criticalPath[0].lastPoint);
        }
        [TestMethod]
        public void TestMethod5() //Тест сохранения
        {
            CriticalPath cp = new CriticalPath(@"E:\vsprojects\KotovExam\KotovExam\Данные задачи.csv", "TestResult.csv");
            cp.ReadData();
            cp.CalculatePathes();
            var criticalPath = cp.FindCriticalPath();
            cp.WriteToFile(criticalPath);
            Assert.IsTrue(File.Exists("TestResult.csv"));
        }
    }
}
