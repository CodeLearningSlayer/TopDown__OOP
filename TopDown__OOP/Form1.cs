using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TopDown__OOP
{
    public partial class Form1 : Form
    {
        Game Shooter;
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
            Form2 TopDown = new Form2();
            TopDown.ShowDialog();
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
            if (e.KeyCode == Keys.D5)
            {
                SaveGame();
       
            }
            if (e.KeyCode == Keys.D6)
            {
                Shooter.upd.Stop();
                LoadGame();
            }
        }

        private void SaveGame()
        {
            //if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
            //    return;
            // получаем выбранный файл
            //string filename = saveFileDialog1.FileName;
            Console.WriteLine("Saved");
            FileStream FS = new FileStream("save.zlp", FileMode.Create);
            BinaryFormatter BF = new BinaryFormatter();
            BF.Serialize(FS, Shooter.hero);
            FS.Close();
        }

        private void LoadGame()
        {
            //if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            //    return;
            // получаем выбранный файл
            //string filename = openFileDialog1.FileName;
            Console.WriteLine("Loaded");
            FileStream FS = File.OpenRead("save.zlp");
            BinaryFormatter BF = new BinaryFormatter();
            Shooter.hero = (Player)BF.Deserialize(FS);
            Shooter.CreateTimer();
            Shooter.CreateGraphics(this);
            Shooter.ReBuild();
            Shooter.Start();
            
        }
    }
}
