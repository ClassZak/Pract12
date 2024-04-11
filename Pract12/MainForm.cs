﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using MiniGame;
using System.Xml.Linq;

namespace Pract12
{
    public partial class MainForm : Form
    {
        public static string[] text;
        public static bool textIsLoading = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textIsLoading)
            {
                MessageBox.Show("Текст ещё загружается", "Подождите", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SavingDialogForm savingDialogForm = new SavingDialogForm();
            savingDialogForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textIsLoading)
            {
                MessageBox.Show("Текст ещё загружается", "Подождите", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                LoadText(openFileDialog.FileName);
        }



        private async void LoadText(string fileName)
        {
            if (textIsLoading)
            {
                MessageBox.Show("Текст ещё загружается","Подождите",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            textIsLoading=true;
            this.textBox1.ReadOnly = true;
            StreamReader sr = new StreamReader(fileName);

            
            await Task.Run(() =>
            {
                text = sr.ReadToEndAsync().Result.Split('\n');
                while (!sr.EndOfStream) ;



                if (this.textBox1.InvokeRequired)
                {
                    string s=string.Join("\n", text);
                    Thread.Sleep(s.Length/1000); 
                    this.textBox1.Invoke(new Action(() =>
                    {
                        this.textBox1.Text = s;
                    }));
                }
                else
                {
                    this.textBox1.Text = string.Join("\n", text);
                }



                //Needs to be remade using threads
                while (this.textBox1.Text == "") ;
                if (this.textBox1.InvokeRequired)
                    this.textBox1.Invoke(new Action(() =>
                    {
                        this.textBox1.ReadOnly = false;
                        textIsLoading = false;
                    }));
                MessageBox.Show("Текст загружен", "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(!MiniGame.MainWindow.notClosed)
            {
                MiniGame.MainWindow mainWindow=new MainWindow();
                mainWindow.Show();
            }
        }
    }
}
