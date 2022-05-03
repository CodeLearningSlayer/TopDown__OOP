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
    public partial class Form1 : Form
    {
        Game Shooter;
        Graphics gr;
        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.DoubleBuffer |
                          ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint,
                          true);
            this.UpdateStyles();
        
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Shown(object sender, EventArgs e)
        {

            
            Shooter = new Game(this); //передать форму this
            Shooter.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //метод отправки в Game
            //direction 
            
            if (e.KeyCode == Keys.W)
            {
                Player.goUp = true;
            }
            if (e.KeyCode == Keys.A)
            {
                Player.goLeft = true;
            }
            if (e.KeyCode == Keys.S)
            {
                Player.goDown = true;
            }
            if (e.KeyCode == Keys.D)
            {
                Player.goRight = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Shooter.watchCursor(MousePosition.X - this.Location.X, MousePosition.Y - this.Location.Y);        
        }


        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                Player.goUp = false;
            }
            if (e.KeyCode == Keys.A)
            {
                Player.goLeft = false;
            }
            if (e.KeyCode == Keys.S)
            {
                Player.goDown = false;
            }
            if (e.KeyCode == Keys.D)
            {
                Player.goRight = false;
            }
        }
    }
}
