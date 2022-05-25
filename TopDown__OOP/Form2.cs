using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TopDown__OOP
{
    public partial class Form2 : Form
    {
        private bool isButtonPressed = false;
        public Form2()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(this.Form_Closing);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isButtonPressed = true;
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (isButtonPressed == false)
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
