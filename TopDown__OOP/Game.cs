using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


namespace TopDown__OOP
{
    [Serializable]
    public class Game
    {
        [NonSerialized] private List<BaseCharacter> entities;
        public Player hero;
        [NonSerialized] public Timer upd;
        [NonSerialized] public Timer spawnTimer;
        [NonSerialized] Timer waveTimer;
        int score;
        [NonSerialized] Form form1;
        int ammo;
        int difficulty;
        int diffTime = 0;
        int wave;
        Font myFont;
        Font newWaveFont;
        Font ammoFont;
        PrivateFontCollection private_fonts = new PrivateFontCollection();
        private int attackTime;
        [NonSerialized] Bitmap Images = new Bitmap(1024, 768);
        [NonSerialized] Graphics G_Bitmap;
        [NonSerialized] Graphics G_Form;
        [NonSerialized] public Rectangle healthBar;
        public delegate void MakeShot(double x, double y, int dx, int dy);
        public event MakeShot onAttack;
        public delegate void onHit(int damage);
        [NonSerialized] protected Graphics G_Gamemap;
        [NonSerialized] Image Background;
        public Game(Form form)
        {
            this.form1 = form;
            healthBar = new Rectangle(580, 20, 200, 20);
            this.score = 0;
            this.ammo = 0;
            difficulty = 20;
            wave = 0;
            this.form1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GetAttack);
            G_Form = form.CreateGraphics();
            G_Bitmap = Graphics.FromImage(Images);
            G_Form.DrawImage(Images, new Point(0, 0));
            this.entities = new List<BaseCharacter>();
            spawnTimer = new Timer();
            spawnTimer.Interval = 2000;
            spawnTimer.Tick += new EventHandler(spawnEnemy);
            upd = new Timer();
            upd.Interval = 40;
            upd.Tick += new EventHandler(updateTimer);
            waveTimer = new Timer();
            waveTimer.Interval = 30;
            waveTimer.Tick += new EventHandler(changeLevel);
            Timer healthTime = new Timer();
            this.form1.KeyDown += new KeyEventHandler(this.ChangeGun);
            this.hero = null;
            this.Background = Properties.Resources.background;
            this.hero = new Player(370, 170, 0, 0, G_Bitmap);
            Bullet.CreateGraphics(G_Bitmap);
            LoadFont();
        }

        public void DrawBg()
        {
            G_Bitmap.DrawImage(Background, new Point(0, 0));
        }

        public void DrawHealth()
        {
            G_Bitmap.DrawRectangle(new Pen(Brushes.DarkRed), healthBar);
            G_Bitmap.FillRectangle(Brushes.Red, new Rectangle(healthBar.X, healthBar.Y, healthBar.Width * (hero.health) / 100, healthBar.Height));
        }

        private void LoadFont()
        {
            using (MemoryStream fontStream = new MemoryStream(Properties.Resources.Overdrive_Sunset))
            {
                // create an unsafe memory block for the font data
                System.IntPtr data = Marshal.AllocCoTaskMem((int)fontStream.Length);
                // create a buffer to read in to
                byte[] fontdata = new byte[fontStream.Length];
                // read the font data from the resource
                fontStream.Read(fontdata, 0, (int)fontStream.Length);
                // copy the bytes to the unsafe memory block
                Marshal.Copy(fontdata, 0, data, (int)fontStream.Length);
                // pass the font to the font collection
                private_fonts.AddMemoryFont(data, (int)fontStream.Length);
                // close the resource stream
                fontStream.Close();
                // free the unsafe memory
                Marshal.FreeCoTaskMem(data);
                myFont = new Font(private_fonts.Families[0], 24);
                ammoFont = new Font(private_fonts.Families[0], 24);
                newWaveFont = new Font(private_fonts.Families[0], 60);
            }
        }

        public void DrawScore()
        {
            G_Bitmap.DrawString(score + "/" + difficulty, myFont, new SolidBrush(Color.Red), new Point(340, 10));
        }

        public void DrawCurrentWave()
        {
            G_Bitmap.DrawString("wave " + (int)(wave + 1), myFont, new SolidBrush(Color.Red), new Point(50, 10));
        }

        private void DrawNewWave()
        {
            G_Bitmap.DrawString("wave " + (int)(wave + 1), newWaveFont, new SolidBrush(Color.Red), new Point(270, 200));
            G_Bitmap.DrawString("Press 5 if you want to save", myFont, new SolidBrush(Color.Red), new Point(140, 330));
        }

        private void changeLevel(object sender, EventArgs e)
        {
            if (diffTime < 6000)
            {
                DrawNewWave();
            }
            else
            {
                waveTimer.Stop();
                diffTime = -40;
                spawnTimer.Start();
            }
            diffTime += 40;
            
        }

        public void spawnEnemy(object sender, EventArgs eArgs)
        {
            var rand = new Random();
            Enemy enemy = new Enemy(rand.Next(1025), (rand.Next(2) == 0 ? rand.Next(-50, -10) : rand.Next(750, 768)), 0, 0, G_Bitmap);
            //onCollision += enemy.Attack;
            //offCollision += enemy.SetDefault;
            //hit += enemy.GetHit;
            this.entities.Add(enemy);
        }
        public void createObjects()
        {
            //this.hero = new Player(370, 170, 0, 0, G_Bitmap);
            onAttack += hero.currGun.Shoot;
            this.entities.Add(this.hero);
            spawnTimer.Start();
        }

        private void checkDifficulty()
        {
            if (score == difficulty)
            {
                spawnTimer.Stop();
                DeleteEnemies();
                spawnTimer.Interval -= 300;
                waveTimer.Start();
                wave += 1;
                score = 0;
                difficulty = (int)Math.Round(difficulty*1.5);
            }
        }
        
        private void DeleteEnemies()
        {
            foreach(BaseCharacter entity in entities.ToArray())
            {
                if (entity is Enemy)
                    entities.Remove(entity);
            }
        }
        private void collisionCheck()
        {
            foreach (BaseCharacter entity in entities.ToArray())
            {
                if (this.hero.hitbox.IntersectsWith(entity.hitbox) && entity is Enemy && entity != null)
                {
                    Console.WriteLine(entity);
                    attackTime += 1;
                    if (attackTime==10)
                    {
                        hero.GetHit(hero.currGun.damage);
                        attackTime = 0;
                    }
                    entity.Attack(true);
                    

                }
                else if (entity != null && entity is Enemy)
                {
                    entity.Attack(false);
                }
                
                for (int i = 0; i < hero.currGun.bullets.Count; i++)
                    if (entity is Enemy && entity.hitbox.IntersectsWith(hero.currGun.bullets[i].hitbox))
                    {
                        Console.WriteLine(entity.health);
                        if (entity.health > 0)
                        {
                            entity.GetHit(hero.currGun.damage);
                        }
                        else
                        {
                            entities.Remove(entity);
                            score += 1;
                            checkDifficulty();
                        }
                        hero.currGun.bullets.Remove(hero.currGun.bullets[i]);
                    }

            }
        }

        private void ChangeGun(object sender, KeyEventArgs e)
        {
            onAttack -= hero.currGun.Shoot;
            if (e.KeyCode == Keys.D1)
            {
                this.hero.ChangeGun("pistol");
            }
            else if (e.KeyCode == Keys.D2)
            {
                this.hero.ChangeGun("rifle");
            }
            else if (e.KeyCode == Keys.D3)
            {
                this.hero.ChangeGun("shotgun");
            }
            else if (e.KeyCode == Keys.D4)
            {
                this.hero.ChangeGun("no gun");
            }
            else if (e.KeyCode == Keys.R) //перезарядка
            {

            }
            onAttack += hero.currGun.Shoot;

        }
        protected void updateTimer(object sender, EventArgs e)
        {
            G_Form.DrawImage(Images, new Point(0, 0));
            DrawBg();
            foreach (BaseCharacter entity in entities)
            {
                if (entity is Enemy)
                {
                    entity.dx = (int)(this.hero.x - entity.x);
                    entity.dy = (int)(this.hero.y - entity.y);
                    entity.dirVector.X = (this.hero.x - entity.x);
                    entity.dirVector.Y = (this.hero.y - entity.y);
                    entity.dirVector.Normalize();
                }
                entity.Draw();
                entity.Move();
            }
            hero.DrawAmmo(ammoFont);
            watchCursor(Form1.MousePosition.X - form1.Location.X, Form1.MousePosition.Y - form1.Location.X);
            DrawHealth();
            DrawScore();
            DrawCurrentWave();
            collisionCheck();
        }


        private void GetAttack(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                onAttack(hero.x + hero.PlayerImg.Width / 2, hero.y + hero.PlayerImg.Height / 2, hero.dx, hero.dy);
                if (hero.isGunChosen == false)
                    hero.ChangeGun("pistol");

            }
        }
        public void Start()
        { 
            createObjects();
            upd.Start();
        }

        

        public void watchCursor(int posX, int posY)
        {
            this.hero.dx = (int)(posX - this.hero.x);
            this.hero.dy = (int)(posY - this.hero.y);

        }
        public void CreateTimer()
        {
            spawnTimer = new Timer();
            spawnTimer.Interval = 2000;
            spawnTimer.Tick += new EventHandler(spawnEnemy);
            upd = new Timer();
            upd.Interval = 40;
            upd.Tick += new EventHandler(updateTimer);
            hero.currGun.CreateTimer();
        }
        public void CreateGraphics(Form form)
        {
            form1 = form;
            Images = new Bitmap(1024, 768);
            G_Form = form.CreateGraphics();
            G_Bitmap = Graphics.FromImage(Images);
            G_Form.DrawImage(Images, new Point(0, 0));
            Background = Properties.Resources.background;
            entities = new List<BaseCharacter>();
            hero.CreateGraphics(G_Bitmap);
            hero.currGun.CreateBullet();
            Bullet.CreateGraphics(G_Bitmap);
            hero.CreateBullets();
        }
        public void ReBuild()
        {
            foreach (GunEntity gun in hero.guns)
            {
                onAttack -= gun.Shoot;
                gun.CreateBullets();
            }
        }
    }
}
