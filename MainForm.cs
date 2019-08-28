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
                WorkWithSQlite.writeFileInfo(_pathfile);
                textBox.Text = Encoding.UTF8.GetString(File.ReadAllBytes(_pathfile));
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("File not selected.", "Error!", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK);
            }
        }

        private void SaveFileToDBAsToolStripMenuItem_Click(object sender, EventArgs e) // учитывать возможность повторения имен файлов
        {
            WorkWithSQlite.FileName = null; // Сохраняет файл с новым именем
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
                // записывает все файлы которые хранятся в базе данных
                Dictionary<string, string> _filesArray = WorkWithSQlite.findAllFilesInDB(); 
                foreach (var item in _filesArray)
                {
                    if (item.Key == WorkWithSQlite.FileName) // Ищет повторения
                    {
                        RequestForm requestForm = new RequestForm();
                        requestForm.ShowDialog();
                        if (requestForm.DialogResult == DialogResult.Cancel)
                        {
                            WorkWithSQlite.FileName = null;
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
                WorkWithSQlite.writeFileInfo(_pathfile); // Записывает формат и имя файла
                WorkWithSQlite.uploadFileToDBAsync(_pathfile); // Загружает файл в базу данных
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("File not selected.", "Error!", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK);
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close(); // Закрывает форму
        } 

        private void VievToolStripMenuItem_Click(object sender, EventArgs e) // Работает с форматом текста в textBox
        {
            fontDialog1.ShowDialog();
            textBox.Font = fontDialog1.Font;
            textBox.Text += "";
        }

        private void TextBox_TextChanged(object sender, EventArgs e) // Срабатывает при изменении текста в textBox
        {
            if (WorkWithSQlite.FileFormat.ToUpper()== "XML" || WorkWithSQlite.FileFormat.ToUpper() == "JSON")
            {
                var currentSelStart = textBox.SelectionStart;
                var currentSelLength = textBox.SelectionLength;
                textBox.SelectAll();                              
                textBox.SelectionColor = SystemColors.WindowText;
                // Ищет теги и раскрашивает их
                coloredText(Regex.Matches(textBox.Text, @"\<(\w+.?\w+)\>|<\/(\w+)[^>]*>"), Color.Blue);
                // Ищет коментарии и раскрашивает их
                coloredText(Regex.Matches(textBox.Text, @"\" + '"' + @"(.*?)\" + '"'), Color.Red); 

                textBox.Select(currentSelStart, currentSelLength);
                textBox.SelectionColor = SystemColors.WindowText;
            }
        }

        private void coloredText(MatchCollection matches, Color colorText) // Красит текст
        {
            foreach (var match in matches.Cast<Match>())
            {
                textBox.Select(match.Index, match.Length);
                textBox.SelectionColor = colorText;
            }
        }

        // Форматирует текст содержащий тэги формата xml и json
        private void AutoTabToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            if (WorkWithSQlite.FileFormat.ToUpper() == "XML")
            {
                // Отправляет текст из textBox на форматирование по заданным шаблонам
                textBox.Lines = autoTab(textBox.Lines, @"\<(\w+.?\w+)\>", @"<\/(\w+)[^>]*>"); 
            }
            if (WorkWithSQlite.FileFormat.ToUpper() == "JSON")
            {
                // Отправляет текст из textBox на форматирование по заданным шаблонам
                textBox.Lines = autoTab(textBox.Lines, @"\{", @"\}"); 
            }
        }

        /* str - Строка в которой производится поиск, startTag - Шаблон открывающего элемента,
         endTag - Шаблон закрывающего элемента */
        private string[] autoTab(string[] str, string startTag, string endTag) // Выставляет знаки табуляции
        {
            int j = 0; // Счетчик необходимого кол-ва табуляций
            int inOneStr=-1; // Проверяет наличие открывающего и закрывающего тега в одной строке
            var findTab = new Regex("\\t"); // Задание элемента для поиска

            for (int i = 0; i < str.Length; i++)
            {
                str[i] = findTab.Replace(str[i], ""); // Обнуляет все табы
            }
            for (int i = 0; i < str.Length; i++)
            {
                var match = Regex.Match(str[i], startTag); // Ищет в строке совпадение по шаблону
                if (match.Success)
                {
                    str[i] = tabCount(str[i], j); // Если совпадение найдено ставит знак табуляции
                    inOneStr = i; // Записывает номер строки с открывающим тегом
                    j++; // увеличивает уровень табуляции на 1
                }
                else
                {
                    str[i] = tabCount(str[i], j); //  Если совпадение не найдено уровень табуляции остается неизменным
                }
                match = Regex.Match(str[i], endTag); // Ищет в строке совпадение по шаблону
                if (match.Success)
                {
                    if (inOneStr != i) // Проверяет наличие открываюшего и закрываюшего тега в одной строке
                    {
                        str[i] = findTab.Replace(str[i], "", 1, 0); // Если есть совпадение то удаляет знак табуляции если таковой есть 
                        j--; // уменьшает уровень табуляции на 1
                    }
                    else
                    {
                        j--;
                    }
                }
            }
            return str;
        }

        private string tabCount(string s, int j) // Ставит необходимое количество отступов
        {
            for (int i = 0; i < j; i++)
            {
                s = "\t" + s;
            }
            return s;
        }

    }
}
