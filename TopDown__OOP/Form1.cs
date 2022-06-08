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
        private Timer watchMouse;
        private Game Shooter;
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
            TopDown.Focus();
            
            TopDown.ShowDialog();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            watchMouse = new Timer();
            watchMouse.Interval = 40;
            watchMouse.Tick += new EventHandler(watchCursor);
            watchMouse.Start();

            using (Shooter = new Game(this.CreateGraphics()))
            {
                this.KeyDown += new KeyEventHandler(Shooter.KeyDown);
                this.MouseDown += new MouseEventHandler(Shooter.GetAttack);
                Shooter.Start();
            }

        }

        private void watchCursor(object sender, EventArgs e)
        {
            Shooter.watchCursor(Form1.MousePosition.X - this.Location.X, Form1.MousePosition.Y - this.Location.Y);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //метод отправки в Game
            //direction 
            if (e.KeyCode == Keys.W)
            {
                Shooter.hero.goUp = true;
            }
            if (e.KeyCode == Keys.A)
            {
                Shooter.hero.goLeft = true;
            }
            if (e.KeyCode == Keys.S)
            {
                Shooter.hero.goDown = true;
            }
            if (e.KeyCode == Keys.D)
            {
                Shooter.hero.goRight = true;
            }
        }

        


        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                Shooter.hero.goUp = false;
            }
            if (e.KeyCode == Keys.A)
            {
                Shooter.hero.goLeft = false;
            }
            if (e.KeyCode == Keys.S)
            {
                Shooter.hero.goDown = false;
            }
            if (e.KeyCode == Keys.D)
            {
                Shooter.hero.goRight = false;
            }
            if (e.KeyCode == Keys.D5)
            {
                if (Shooter.canISave)
                SaveGame();
            
            }
            if (e.KeyCode == Keys.D6)
            {
                Shooter.upd.Stop();
                
                LoadGame();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (Shooter.isGameOver)
                {
                    watchMouse.Stop();
                    //Shooter.hero = null;
                    this.Shooter.upd.Stop();
                    this.Shooter.G_Form.Dispose();
                    Shooter.Dispose();
                    this.MouseDown -= Shooter.GetAttack;
                    this.KeyDown -= Shooter.KeyDown;
                    Shooter = new Game(this.CreateGraphics());
                    this.KeyDown += Shooter.KeyDown;
                    this.MouseDown += Shooter.GetAttack;
                    GC.Collect();
                    watchMouse.Start();
                    Shooter.Start();
                }
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
            BF.Serialize(FS, Shooter);
            FS.Close();
        }

        private void LoadGame()
        { 
            Console.WriteLine("Loaded");
            Game gameHelper;
            FileStream FS = File.OpenRead("save.zlp");
            BinaryFormatter BF = new BinaryFormatter();
            gameHelper = (Game)BF.Deserialize(FS);
            Shooter.difficulty = gameHelper.difficulty;
            Shooter.hero = gameHelper.hero;
            Shooter.hero.reloadBar = gameHelper.hero.reloadBar;
            Shooter.hero.currGun = gameHelper.hero.currGun;
            Shooter.chngGun = gameHelper.chngGun;
            Shooter.hero.gameInit = false;
            Shooter.entities = gameHelper.entities;
            Console.WriteLine(Shooter.chngGun);
            Shooter.hero.ChangeGun(Shooter.chngGun);
            Shooter.isSerialized = true;
            Shooter.CreateGraphics();
            Shooter.CreateTimer();
            Shooter.ReBuild();
            Shooter.score = gameHelper.score;            
            Shooter.Start();
            Shooter.isSerialized = false;
        }
    }
}
