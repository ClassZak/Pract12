using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pract12
{
    public partial class SavingDialogForm : Form
    {
        static string name="";
        static string surname = "";
        static bool saving=false;

        public SavingDialogForm()
        {
            InitializeComponent();
            this.NameTextBoxd.Text= name;
            this.SurnameTextBoxd.Text= surname;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MainForm.textIsLoading)
            {
                MessageBox.Show("Текст ещё загружается", "Подождите", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (saving)
            {
                MessageBox.Show("Текст сохраняется", "Подождите", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            SaveText($"{this.NameTextBoxd.Text}_{this.SurnameTextBoxd.Text}.txt");
        }




        private void SavingDialogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            name=this.NameTextBoxd.Text;
            surname=this.SurnameTextBoxd.Text;
        }
        private async void SaveText(string fullName)
        {
            await Task.Run(() =>
            {
                saving=true;
                StreamWriter streamWriter=new StreamWriter(fullName);
                for(int i = 0;i!=MainForm.text.Length;++i)
                    streamWriter.Write(MainForm.text[i]);
                streamWriter.Close();
                saving = false;
                MessageBox.Show("Текст сохранён!","Сохранение",MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        }
    }
}
