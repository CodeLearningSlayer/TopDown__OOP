using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace TopDown__OOP
{

    public class Game
    {
        private List<BaseCharacter> entities;
        Player hero;
        Timer upd;
        Timer spawnTimer;
        int score;
        Form form1;
        int ammo;
        Bitmap Images = new Bitmap(1024, 768);
        Graphics G_Bitmap;
        Graphics G_Form;
        public event Action onCollision;
        public event Action offCollision;
        public delegate void MakeShot(double x, double y, int dx, int dy);
        public event MakeShot onAttack;
        public delegate void onHit(int damage);
        public event onHit hit;
        protected Graphics G_Gamemap;
        Image Background;
        public Game(Form form)
        {
            this.form1 = form;
            this.score = 0;
            this.ammo = 0;
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
            this.form1.KeyDown += new KeyEventHandler(this.ChangeGun);
            this.hero = null;
            this.Background = Properties.Resources.background;

        }

        public void DrawBg()
        {
            if (form1.InvokeRequired)
            {
                Console.WriteLine("InvokeRequired");
            }


            G_Bitmap.DrawImage(Background, new Point(0, 0));
        }


        public void spawnEnemy(object sender, EventArgs eArgs)
        {
            var rand = new Random();
            Enemy enemy = new Enemy(rand.Next(1025), rand.Next(750,768), 0,0, G_Bitmap);
            onCollision += enemy.Attack;
            offCollision += enemy.SetDefault;
            hit += enemy.GetHit;
            this.entities.Add(enemy);

        }
        public void createObjects()
        {
            this.hero = new Player(370, 170, 0, 0, G_Bitmap);
            onAttack += hero.currGun.Shoot;
            this.entities.Add(this.hero);
            spawnTimer.Start();
        }

        public void collisionCheck()
        {
            foreach(BaseCharacter entity in entities.ToArray())
            {
                if (this.hero.hitbox.IntersectsWith(entity.hitbox) && entity is Enemy && entity != null)
                {
                    Console.WriteLine(entity);
                    hero.GetHit(hero.currGun.damage);
                    onCollision();

                }
                else if (entity!=null && entity is Enemy)
                {
                    offCollision();
                }
                for (int i=0; i<hero.currGun.bullets.Count; i++)
                if (entity is Enemy && entity.hitbox.IntersectsWith(hero.currGun.bullets[i].hitbox))
                {
                        if (entity.health > 0)
                            hit(hero.currGun.damage);
                        else entities.Remove(entity);
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
            onAttack += hero.currGun.Shoot;
        }
        protected void updateTimer(object sender, EventArgs e)
        {
            G_Form.DrawImage(Images, new Point(0, 0));
            this.DrawBg();
            foreach (BaseCharacter entity in entities)
            {
                if (entity is Enemy) 
                {
                    entity.dx = (int)(this.hero.x - entity.x);
                    entity.dy = (int)(this.hero.y - entity.y);
                    entity.dirVector.X = (this.hero.x - entity.x);
                    entity.dirVector.Y = (this.hero.y - entity.y);
                    entity.dirVector.Normalize();
                    entity.Draw();
                    entity.Move();
                }
                else
                {
                    entity.Draw();
                    entity.Move();
                }
                
            }
            collisionCheck();
        }

        
        private void GetAttack(object sender, MouseEventArgs e)
        {
           if (e.Button == MouseButtons.Left )
            {
                
                onAttack(hero.x + hero.PlayerImg.Width/2, hero.y + hero.PlayerImg.Height/2, hero.dx, hero.dy);
                if (hero.isGunChosen == false)
                this.hero.ChangeGun("pistol");
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
    }
}
