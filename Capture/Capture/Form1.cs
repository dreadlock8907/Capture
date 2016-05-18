using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Capture
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            //checkBox1.CheckedChanged += CheckBox1_CheckedChanged;
            radioButton1.Checked = true;
            radioButton1.CheckedChanged += RadioButton1_CheckedChanged;
            checkBox1.CheckedChanged += checkBox_isChecked;
            checkBox1.MouseHover += CheckBox1_MouseHover;
            textBox1.TextChanged += TextBox1_TextChanged;
            textBox2.ReadOnly = true;
           
        }

        private void CheckBox1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("use ALT+SHIFT for automatic control :)", checkBox1);
        }


        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            invokeProcess();
        }
        //процесс преобразования
        public void invokeProcess()
        {
            processingString pS = new processingString();

            if (radioButton1.Checked && !checkBox1.Checked)
                textBox2.Text = pS.processStr(textBox1.Text, true, false);
            else if (radioButton2.Checked && !checkBox1.Checked)
                textBox2.Text = pS.processStr(textBox1.Text, false, false);
            else if (checkBox1.Checked)
                textBox2.Text = pS.processStr(textBox1.Text, false, true);
        }
        //при изменении текста
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != textBox1.MaxLength)
            {
                invokeProcess();
            }
            else
                MessageBox.Show("Превышено количество вводимых символов", "Warning", MessageBoxButtons.OK);
        }
        //при нажатии на кнопку
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //при установки галки
        private void checkBox_isChecked(object Sender, EventArgs e)
        {
            if (checkBox1.CheckState == CheckState.Checked)
            {
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                invokeProcess();
            }
            else
            {
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
            }

            
        }
    }
}
