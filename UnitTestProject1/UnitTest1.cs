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
            CriticalPath cp = new CriticalPath(@"Данные задачи.csv", null);
            cp.ReadData();
            Assert.AreEqual("1", cp.FindStartingPos());
        }
        [TestMethod]
        public void TestMethod2() //Тест поиска конечной точки
        {
            CriticalPath cp = new CriticalPath(@"Данные задачи.csv", null);
            cp.ReadData();
            Assert.AreEqual("7", cp.FindEndingPos());
        }
    }
}
