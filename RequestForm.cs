using System;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class RequestForm : Form
    {
        public RequestForm()
        {
            InitializeComponent();
        }

        private void ButtonRename_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; //Возврат исходному коду указания на переименовывание файла
            this.Close();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK; // Возврат вызывающему коду указания обновить запись в базе данных
            this.Close();
        }
    }
}
