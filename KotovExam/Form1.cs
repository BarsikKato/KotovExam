using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace KotovExam
{
    public partial class Form1 : Form
    {
        string readPath;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var location = AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog.InitialDirectory = location;
            openFileDialog.Filter = "csv files (*.csv)|*.csv";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog.FileNames.Length > 1)
                    MessageBox.Show("Нельзя загрузить больше одного файла!");
                else if (openFileDialog.FileNames.Length == 1)
                {
                    readPath = openFileDialog.FileNames[0];
                }
                else
                    MessageBox.Show("Выберите файл!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            Stream myStream;
            saveFileDialog.Filter = "csv files (*.csv)|*.csv";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog.OpenFile()) != null)
                {
                    myStream.Close();
                    string path = saveFileDialog.FileName;
                    if (path != null && path != "")
                    {
                        //CriticalPath cp = new CriticalPath(dataPath, path);
                        //cp.CalculateCriticalPath();
                        MessageBox.Show("Решение сохранено!");
                        Environment.Exit(0);
                    }
                }
            }
        }
    }
}
