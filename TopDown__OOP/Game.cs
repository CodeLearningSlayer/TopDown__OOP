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
    public class Game:IDisposable
    {
        [NonSerialized] public List<BaseCharacter> entities;
        public Player hero;
        [NonSerialized] public Timer upd;
        [NonSerialized] public Timer spawnTimer;
        [NonSerialized] Timer waveTimer;
        [NonSerialized] public Timer deathTimer;
        public int score { get; set; }
        public bool isGameOver = false;
        private int blinkTime;
        public bool canISave; 
        public int difficulty;
        private bool _isDisposed = false;
        private int diffTime = 0;
        private int wave;
        public string chngGun;
        private Font myFont;
        private Font newWaveFont;
        public bool isSerialized = false;
        private Font ammoFont;
        [NonSerialized] private PrivateFontCollection private_fonts = new PrivateFontCollection();
        private int attackTime;
        [NonSerialized] private Bitmap Images = new Bitmap(1024, 768);
        [NonSerialized] private Graphics G_Bitmap;
        [NonSerialized] public Graphics G_Form;
        [NonSerialized] private Rectangle healthBar;
        public delegate void MakeShot(double x, double y, int dx, int dy);
        public event MakeShot onAttack;
        //public delegate void onHit(int damage);
        [NonSerialized] private Image Background;
        public Game(Graphics G_Form)
        {
            
            healthBar = new Rectangle(580, 20, 200, 20);
            this.score = 0;
            difficulty = 20;
            this.G_Form = G_Form;
            chngGun = "pistol";
            wave = 0;
            G_Bitmap = Graphics.FromImage(Images);
            this.G_Form.DrawImage(Images, new Point(0, 0));
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
            this.hero = null;
            this.Background = Properties.Resources.background;
            this.hero = new Player(370, 170, 0, 0, G_Bitmap);
            Bullet.CreateGraphics(G_Bitmap);
            LoadFont();
            hero.onDie += showDeathScreen;
            deathTimer = new Timer();
            deathTimer.Interval = 40;
            deathTimer.Tick += new EventHandler(drawDeathScreen);
        }

        private void DrawBg()
        {
            G_Bitmap.DrawImage(Background, new Point(0, 0));
        }

        private void DrawHealth()
        {
            G_Bitmap.DrawRectangle(new Pen(Brushes.DarkRed), healthBar);
            G_Bitmap.FillRectangle(Brushes.Red, new Rectangle(healthBar.X, healthBar.Y, healthBar.Width * (hero.health) / 100, healthBar.Height));
        }

        private void showDeathScreen()
        {
            if (entities != null)
            entities.Clear();
            spawnTimer.Stop();
            isGameOver = true;
            deathTimer.Start();
        }

        private void drawDeathScreen(object sender, EventArgs e)
        {
            
            G_Bitmap.DrawString("Game Over", newWaveFont, new SolidBrush(Color.Red), new Point(220, 150));
            blinkTime += 1;
            if (blinkTime > 5 && blinkTime < 20)
            {
                G_Bitmap.DrawString("Press Enter to play again", myFont, new SolidBrush(Color.Red), new Point(230, 240));
            }
            else if (blinkTime == 20) blinkTime = 0;
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

        private void DrawScore()
        {
            G_Bitmap.DrawString(score + "/" + difficulty, myFont, new SolidBrush(Color.Red), new Point(340, 10));
        }

        private void DrawCurrentWave()
        {
            G_Bitmap.DrawString("wave " + (int)(wave + 1), myFont, new SolidBrush(Color.Red), new Point(50, 10));
        }

        private void DrawNewWave()
        {
            G_Bitmap.DrawString("wave " + (int)(wave + 1), newWaveFont, new SolidBrush(Color.Red), new Point(270, 200));
            G_Bitmap.DrawString("Press 5 if you want to save", myFont, new SolidBrush(Color.Red), new Point(210, 350));
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
                canISave = false;
                hero.AddAmmo();
                diffTime = -40;
                spawnTimer.Start();
            }
            diffTime += 40;
            
        }
        public void Dispose()
        {
            Dispose(true);
            
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                onAttack = null;
                if (!_isDisposed)
                {
                    _isDisposed = true;
                    return;
                }
            }
        }
        ~Game()
        {
            Console.WriteLine("Was disposed");
        }

        private void spawnEnemy(object sender, EventArgs eArgs)
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
            //onAttack = null;
            //this.hero = new Player(370, 170, 0, 0, G_Bitmap);
            if (isSerialized == false)
                onAttack += hero.currGun.Shoot;
            else
            {
                onAttack = null;
                onAttack += hero.currGun.Shoot;
            }
            hero.initGame(this);
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
                canISave = true;
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
                        hero.GetHit(10);
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


        
        public void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D1)
            {
                onAttack = null;
                chngGun = "pistol";
                this.hero.ChangeGun(chngGun);
            }
            else if (e.KeyCode == Keys.D2)
            {
                onAttack = null;
                chngGun = "rifle";
                this.hero.ChangeGun(chngGun);
                
            }
            else if (e.KeyCode == Keys.D3)
            {
                chngGun = "shotgun";
                this.hero.ChangeGun(chngGun);
            }
            else if (e.KeyCode == Keys.D4)
            {
                chngGun = "no gun";
                this.hero.ChangeGun(chngGun);
            }
            else if (e.KeyCode == Keys.R) //перезарядка
            {
                hero.checkAmmo(this, true);
            }
        }
        private void updateTimer(object sender, EventArgs e)
        {
            G_Form.DrawImage(Images, new Point(0, 0));
            DrawBg();
            foreach (BaseCharacter entity in entities)
            {
                if (entity is Enemy)
                {
                    entity.SetDirection((int)(this.hero.x - entity.x), (int)(this.hero.y - entity.y));
                    entity.SetVector((this.hero.x - entity.x), (this.hero.y - entity.y));
                }
                entity.Draw();
                entity.Move();
            }
            hero.DrawAmmo(ammoFont);
            
            DrawHealth();
            DrawScore();
            
            DrawCurrentWave();
            collisionCheck();
        }


        public void GetAttack(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (hero.currGun.ammo!=0 || hero.currGun.currAmmo!=0)
                onAttack?.Invoke(hero.x + hero.PlayerImg.Width / 2, hero.y + hero.PlayerImg.Height / 2, hero.dx, hero.dy);
                if (hero.isGunChosen == false)
                    hero.ChangeGun("pistol");
                hero.checkAmmo(this, false);
            }
        }
        public void Start()
        { 
            createObjects();
            upd.Start();
        }


        public void watchCursor(int posX, int posY)
        {
            this.hero.SetDirection(posX - (int)this.hero.x, posY - (int)this.hero.y);
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
            hero.CreateTimer();
            waveTimer = new Timer();
            waveTimer.Interval = 30;
            waveTimer.Tick += new EventHandler(changeLevel);
            deathTimer = new Timer();
            deathTimer.Interval = 40;
            deathTimer.Tick += new EventHandler(drawDeathScreen);
        }
        public void CreateGraphics()
        {
            
            Images = new Bitmap(1024, 768);
            //G_Form = form.CreateGraphics();А
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
            hero.onDie += showDeathScreen;
            foreach (GunEntity gun in hero.guns)
            {
                onAttack -= gun.Shoot;
                gun.CreateBullets();
            }
        }
    }
}
