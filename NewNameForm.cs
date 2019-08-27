using System;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class NewNameForm : Form
    {
        public NewNameForm()
        {
            InitializeComponent();
        }

        private void ButtonAccept_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(textBoxFileName.Text)) // Проверка на заполненость поля нового имени файла
            {
                WorkWithSQlite.FileName = textBoxFileName.Text; // Передача в статический класс нового имени файла
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Empty filename field!", "Error", MessageBoxButtons.OK);
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; // Возврат вызывающему коду информацию об отмене именования файла
            this.Close();
        }
    }
}
