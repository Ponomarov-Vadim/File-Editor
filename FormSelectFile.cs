using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class FormSelectFile : Form
    {
        private Dictionary<string, string> filesInDB = WorkWithSQlite.findAllFilesInDB();

        public FormSelectFile()
        {
            InitializeComponent();
        }

        private void FormSelectFile_Load(object sender, EventArgs e)
        {
            try { 
                foreach (var item in filesInDB)
                {
                    listOfFiles.Items.Add(String.Format("{0}.{1}", item.Key, item.Value)); // при загрузке формы заполнить список 
                }                                                                          // файлов находящихся в базе данных   
            }
            catch (Exception)
            {
                MessageBox.Show("Data Base is Empty!", "Error!", MessageBoxButtons.OK);
                this.Close();
            }
        }

        private void ButtonSelect_Click(object sender, EventArgs e)
        {
            if (!(listOfFiles.SelectedItem is null)) // проверка выбран файл или нет
            {
                foreach (var item in filesInDB)
                {   // Сравнение выбранного файла с файлом в базе данных и занесение этой информации в статический класс
                    if (String.Format("{0}.{1}", item.Key, item.Value).Equals(listOfFiles.SelectedItem.ToString()))
                    {
                        WorkWithSQlite.FileFormat = item.Value;
                        WorkWithSQlite.FileName = item.Key;
                        this.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("File is not selected. Select file!", "Error!", MessageBoxButtons.OK);
            }
            
        }
    }
}
