using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace TextEditor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            WorkWithSQlite.createOrOpenDB(); //Создает базу данных если таковой нету по указанному пути в .config
        }

        private void OpenFromBDToolStripMenuItem_Click(object sender, EventArgs e) // Open from DB открывает из базы файл
        {
            FormSelectFile formSelect = new FormSelectFile();
            formSelect.ShowDialog();
            textBox.Text = WorkWithSQlite.downloadFileFromDBAsync().Result;
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e) // Открывает файл для загрузки в текстовый редактор
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();  // Открывает проводник для выбора файла который нужно загрузить
                fileDialog.ShowDialog();
                string _pathfile = fileDialog.FileName;
                textBox.Text = Encoding.UTF8.GetString(File.ReadAllBytes(_pathfile));
                WorkWithSQlite.writeFileInfo(_pathfile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK);
            }
        }

        private void SaveFileToDBAsToolStripMenuItem_Click(object sender, EventArgs e) // учитывать возможность повторения имен файлов
        {
            WorkWithSQlite.FileName = null; // Сохраняет файл с новым именем возможно проблема тут
            if (fileNameFormatCheck())
            {
                if (checkingDuplicateFiles())
                {
                    WorkWithSQlite.uploadFileToDBAsync(null, textBox.Text);
                }
            }
        }

        private void SaveFileToolStripMenuItem_Click(object sender, EventArgs e) // Сохраняет файл в базу данных
        {
            if (fileNameFormatCheck()) // Проверяет наличие имени и формата файла
            {
                if (checkingDuplicateFiles()) // Проверяет нету ли повторяющихся файлов
                {
                    WorkWithSQlite.uploadFileToDBAsync(null, textBox.Text); // загрузает файл в базу данных
                }
            }
        }

        private bool checkingDuplicateFiles() // Проверяет на наличие файла с таким же именем как у сохраняемого
        {
            try
            {
                Dictionary<string, string> _filesArray = WorkWithSQlite.findAllFilesInDB(); // записывает все файлы которые хранятся в базу данных
                foreach (var item in _filesArray)
                {
                    if (item.Key == WorkWithSQlite.FileName) // Ищет повторения
                    {
                        RequestForm requestForm = new RequestForm();
                        requestForm.ShowDialog();
                        if (requestForm.DialogResult == DialogResult.Cancel)
                        {
                            WorkWithSQlite.FileName = null; // тут что то может быть?
                            return fileNameFormatCheck();
                        }
                        else if (requestForm.DialogResult == DialogResult.OK)
                        {
                            WorkWithSQlite.updateFileInDBAsync(textBox.Text);
                            return true;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error!", MessageBoxButtons.OK);
                return true;
            }
        }

        private bool fileNameFormatCheck() // Проверяет наличие имени и формата файла
        {
            if (String.IsNullOrWhiteSpace(WorkWithSQlite.FileFormat)) // Если формат не указан автоматически присвоит .txt
            {
                WorkWithSQlite.FileFormat = "txt";
            }
            while (true)
            {
                if (String.IsNullOrWhiteSpace(WorkWithSQlite.FileName)) // Если имя не заданно предложит его написать
                {
                    NewNameForm nameForm = new NewNameForm();
                    nameForm.ShowDialog();
                    if (nameForm.DialogResult == DialogResult.Cancel)
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
        }

        private void SelectFileToLoadInDBToolStripMenuItem_Click(object sender, EventArgs e) // загружает выбранный файл в базу данных
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.ShowDialog();
                string _pathfile = fileDialog.FileName;
                WorkWithSQlite.writeFileInfo(_pathfile);
                WorkWithSQlite.uploadFileToDBAsync(_pathfile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK);
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        } // Закрывает форму

        private void VievToolStripMenuItem_Click(object sender, EventArgs e) // Работает с форматом текста в textBox
        {
            fontDialog1.ShowDialog();
            textBox.Font = fontDialog1.Font;
            textBox.Text += "";
        }

        private void TextBox_TextChanged(object sender, EventArgs e) // Срабатывает при изменении текста в textBox
        {            
            
            var currentSelStart = textBox.SelectionStart;
            var currentSelLength = textBox.SelectionLength;
            textBox.SelectAll();                                                         // Не является оптимальным решением
            textBox.SelectionColor = SystemColors.WindowText;
            coloredText(Regex.Matches(textBox.Text, @"\<(\w+.?\w+)\>|<\/(\w+)[^>]*>"), Color.Blue); // Ищет теги и раскрашивает их
            coloredText(Regex.Matches(textBox.Text, @"\" + '"' + @"(.*?)\" + '"'), Color.Red); // Ищет коментарии и раскрашивает их

            textBox.Select(currentSelStart, currentSelLength);
            textBox.SelectionColor = SystemColors.WindowText;
        }

        private void coloredText(MatchCollection matches, Color colorText) // Красит текст
        {
            foreach (var match in matches.Cast<Match>())
            {
                textBox.Select(match.Index, match.Length);
                textBox.SelectionColor = colorText;
            }
        }

        private void AutoTabToolStripMenuItem_Click(object sender, EventArgs e) // Форматирует текст содержащий тэги формата xml
        {
            int j = 0; // Счетчик необходимого кол-ва табуляций
            string[] s = textBox.Lines; // Загружает строки
            var findTab = new Regex("\\t"); // Задание элемента для поиска

            for (int i = 0; i < s.Length; i++)
            {
                s[i] = findTab.Replace(s[i], ""); // Обнуляет все табы
            }
            for (int i = 0; i < s.Length; i++)
            {
                var match = Regex.Match(s[i], @"\<(\w+.?\w+)\>"); // Ищет в строке совпадение по шаблону. Пример: <config> или <con.fig>
                if (match.Success) 
                {
                    s[i] = tabCount(s[i], j); // Если совпадение найдено ставит знак табуляции
                    j++; // увеличивает уровень табуляции на 1
                }
                else
                {
                    s[i] = tabCount(s[i], j); //  Если совпадение не найдено уровень табуляции остается неизменным
                }
                match = Regex.Match(s[i], @"<\/(\w+)[^>]*>"); // Ищет в строке совпадение по шаблону. Пример: </config>
                if (match.Success) 
                {
                    s[i] = findTab.Replace(s[i], "", 1, 0); // Если есть совпадение то удаляет знак табуляции если таковой есть 
                    j--; // уменьшает уровень табуляции на 1
                }
            }
            textBox.Lines = s; // Возвращает отформатированный текст
        }

        private string tabCount(string s, int j) // Ставит необходимое количество отступов
        {
            for (int i = 0; i < j; i++)
            {
                s = "\t" + s;
            }
            return s;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //var currentSelStart = textBox.SelectionStart;
            //var currentSelLength = textBox.SelectionLength;
            //if (e.KeyCode == Keys.Back)
            //{
            //    textBox.Select(currentSelStart - 1, 1);
            //    if (Convert.ToChar(textBox.SelectedText)=='"')
            //    {
            //        if (currentSelStart-2>=0)
            //        {
            //            textBox.Select(currentSelStart - 2, 1);
            //            if (textBox.SelectionColor == Color.Red)
            //            {
            //                MessageBox.Show("1", ""); // находит удаление " ковычки
            //            }
            //        }
                   
            //    }
            //    else if (textBox.SelectedText == "<" || textBox.SelectedText == ">")
            //    {
            //        if (textBox.SelectionColor==Color.Blue)
            //        {

            //        } 
            //    }
            //}
        }
    }
}
