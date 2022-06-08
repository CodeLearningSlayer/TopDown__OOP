using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Windows;


namespace TopDown__OOP
{
    [Serializable]
    public class Player : BaseCharacter
    {
        [NonSerialized] public Image PlayerImg;
        [NonSerialized] System.Windows.Forms.Timer t;
        public bool goLeft;
        public bool goRight;
        public bool goUp;
        public bool goDown;
        private bool isReloaded { get; set; }
        public bool isGunChosen;
        private Game game;
        public bool gameInit = false;
        public event Action onDie;
        [NonSerialized] public Rectangle reloadBar;
        [NonSerialized] private Timer reload;
        public List<GunEntity> guns;
        public GunEntity currGun;
        [NonSerialized] private GraphicsUnit units = GraphicsUnit.Point;
        private int Condition { get; set; }
        public Player(double x, double y, int dx, int dy, Graphics G_Bitmap) : base(x, y, dx, dy, G_Bitmap)
        {
            guns = new List<GunEntity>();
            this.PlayerImg = Properties.Resources.walk_1;
            currGun = null;
            this.hitbox_base = this.PlayerImg.GetBounds(ref units);
            this.hitbox = Rectangle.Round(this.hitbox_base);
            isGunChosen = false;
            t = new System.Windows.Forms.Timer();
            t.Tick += new EventHandler(t_tick);
            t.Interval = 200;
            health = 100;
            this.speed = 5;
            isReloaded = true;
            this.x = x;
            this.dy = 0;
            reloadBar = new Rectangle((int)this.x, (int)this.y + 10, 50, 15);
            dirVector = new Vector(dx, dy);
            currGun = new Pistol(10, 15, 9, 5, G_Bitmap, 30);
            guns.Add(currGun);
            guns.Add(new Rifle(100, 15, 90, 6, G_Bitmap, 40));
            t.Start();
            Condition = 0;
            reload = new Timer();
            reload.Tick += new EventHandler(ReloadGun);
            reload.Interval = 40;

        }
        protected void t_tick(object sender, EventArgs eArgs)
        {
            this.RunAnim();
        }

        public void AddAmmo()
        {
            foreach (GunEntity gun in guns)
            {
                gun.ammo += 30;
            }
        }


        public override void Draw()
        {
            Bitmap bmpToRotate = new Bitmap(this.PlayerImg.Width + 10, this.PlayerImg.Height + 10);
            using (bmpToRotate)
            {
                double angle = Math.Atan2(this.dy, this.dx) * 180 / Math.PI + (-90) * Math.PI / 180;
                Graphics gfx = Graphics.FromImage(bmpToRotate);
                gfx.TranslateTransform((float)bmpToRotate.Width / 2, (float)bmpToRotate.Height / 2);
                gfx.RotateTransform((float)angle);
                gfx.TranslateTransform(-(float)bmpToRotate.Width / 2, -(float)bmpToRotate.Height / 2);
                gfx.DrawImage(PlayerImg, new System.Drawing.Point(6, 6));
                gfx.Dispose();
                this.hitbox.X = (int)this.x;
                this.hitbox.Y = (int)this.y;
                G_Bitmap.DrawImage(bmpToRotate, new System.Drawing.Point((int)this.x, (int)this.y));
                //G_Bitmap.DrawRectangle(Pens.Blue, this.hitbox);
            }
        }

        public void DrawAmmo(Font myFont)
        {
            if (currGun is Pistol)
                G_Bitmap.DrawString("Ammo: " + currGun.currAmmo + "/" + "♾", myFont, new SolidBrush(Color.Red), new System.Drawing.Point(610, 50));
            else
                G_Bitmap.DrawString("Ammo: " + currGun.currAmmo + "/" + currGun.ammo, myFont, new SolidBrush(Color.Red), new System.Drawing.Point(610, 50));

        }

        protected override void RunAnim()
        {
            if ((goUp || goDown || goLeft || goRight) && isGunChosen == false) {
                if (Condition == 0)
                {
                    PlayerImg = Properties.Resources.walk_1;
                    Condition = 1;
                }
                else if (Condition == 1)
                {
                    PlayerImg = Properties.Resources.walk_2;
                    Condition = 2;
                }
                else if (Condition == 2)
                {
                    PlayerImg = Properties.Resources.walk_3;
                    Condition = 3;
                }
                else if (Condition == 3)
                {
                    PlayerImg = Properties.Resources.walk_4;
                    Condition = 4;
                }
                else if (Condition == 4)
                {
                    PlayerImg = Properties.Resources.walk_5;
                    Condition = 5;
                }
                else if (Condition == 5)
                {
                    PlayerImg = Properties.Resources.walk_6;
                    Condition = 0;
                }
            }
            else if( isGunChosen == false)
            {
                Condition = 0;
                PlayerImg = Properties.Resources.walk_1;
            }

        }

        public override void Move()
        {
            
            if (goUp || goDown) {

                if (goUp == true)
                {
                    this.dirVector.Y = -1;
                    
                }
                if (goDown == true)
                {
                    this.dirVector.Y = 1;
                    
                }
            }
            else
            {
                this.dirVector.Y = 0;
            }
            if (goLeft || goRight) {

                if (goLeft == true)
                {
                    this.dirVector.X = -1;
                }
                if (goRight == true)
                {
                    this.dirVector.X = 1;
                }
            }
            else
            {
                this.dirVector.X = 0;
            }


            if (this.x <= 0 && this.dirVector.X == -1)
            {
                this.dirVector.X = 0;
            }
            else if (this.y <= 0 && this.dirVector.Y == -1)
            {
                this.dirVector.Y = 0;
            }
            else if (this.y == 405 && this.dirVector.Y == 1)
            {
                this.dirVector.Y = 0;
            }
            else if (this.x == 740 && this.dirVector.X == 1)
            {
                this.dirVector.X = 0;
            }
            this.x += speed * this.dirVector.X;
            this.y += speed * this.dirVector.Y;

        }
        //public override void Die()
        //{

        //}

        public override void GetHit(int damage)
        {
            base.GetHit(damage);
            if (health < 0)
            {
                Die();
            }
        }

        private void Die()
        {
            onDie.Invoke();
        }

        public void ChangeGun(string gun)
        {
            if (gun == "pistol" && isReloaded)
            {
                this.PlayerImg = Properties.Resources.player_9mmhandgun;
                this.speed = 4;
                game.onAttack -= currGun.Shoot;
                currGun = guns[0];
                game.onAttack += currGun.Shoot;
                isGunChosen = true;

            }
            else if (gun == "rifle" && isReloaded)
            {
                if (guns.Count >= 2)
                {
                    this.PlayerImg = Properties.Resources.player_chaingun;
                    this.speed = 3;
                    game.onAttack -= currGun.Shoot;
                    currGun = guns[1];
                    game.onAttack += currGun.Shoot;
                    isGunChosen = true;

                }


            }
            else if (gun == "shotgun")
            {
                if (guns.Count>=3)
                {
                    this.PlayerImg = Properties.Resources.player_pumpgun_stand;
                    this.speed = 3;
                    currGun = guns[2];
                    isGunChosen = true;

                }

            }
            else if (gun == "no gun")
            {
                
                isGunChosen = false;
                this.speed = 5;
                this.PlayerImg = Properties.Resources.walk_1;
            }
            Console.WriteLine(currGun);
        }

        private void ReloadGun(object sender, EventArgs e)
        {
            G_Bitmap.DrawRectangle(new Pen(Brushes.DarkRed), reloadBar);
            reloadBar.X = (int)this.x - 5;
            reloadBar.Y = (int)this.y - 5;
            G_Bitmap.FillRectangle(Brushes.Red, new Rectangle(reloadBar.X, reloadBar.Y, reloadBar.Width, reloadBar.Height));
            G_Bitmap.FillRectangle(Brushes.Green, new Rectangle(reloadBar.X, reloadBar.Y, (int)Math.Round((double)currGun.currAmmo / currGun.clip * reloadBar.Width), reloadBar.Height));
            if (currGun.currAmmo < currGun.clip && currGun.currAmmo < currGun.ammo && currGun.ammo>0)
            {
                currGun.Reload();
            }
            else
            {
                reload.Stop();
                stopReload();
                isReloaded = true;
                currGun.ammo -= currGun.Reload();
                currGun.bulletsLoaded = 0;
            }
        }

        public void checkAmmo(Game sender, bool reloadPressed)
        {
            if (currGun.currAmmo <= 0 && currGun.ammo > 0 || reloadPressed)
            {
                reload.Start();
                isReloaded = false;
                sender.onAttack -= currGun.Shoot;
            }
            if (currGun.currAmmo <= 0 && currGun.ammo == 0)
            {
                sender.onAttack -= currGun.Shoot;
            }
            
        }

        private void stopReload()
        {
            if (gameInit)
                Console.WriteLine("можно стрелять");
                game.onAttack += currGun.Shoot;
        }

        public void GetWeapon(GunEntity gun)
        {
            guns.Add(gun);
        }

        public void initGame(Game game)
        {
            this.game = game;
        }
        public void CreateTimer()
        {
            onDie = null;
            t = new System.Windows.Forms.Timer();
            t.Tick += new EventHandler(t_tick);
            t.Interval = 200;
            reload = new Timer();
            reload.Tick += new EventHandler(ReloadGun);
            reload.Interval = 40;
        }
        public override void CreateGraphics(Graphics G_Bitmap)
        {
            base.CreateGraphics(G_Bitmap);
            //this.PlayerImg = Properties.Resources.walk_1;
            Console.WriteLine(reloadBar);
            Bitmap bmpToRotate = new Bitmap(this.PlayerImg.Width, this.PlayerImg.Height);
            reloadBar = new Rectangle((int)this.x, (int)this.y + 10, 50, 15);

        }

        public void CreateBullets()
        {
            foreach (GunEntity entity in guns.ToArray())
            {
                entity.CreateBullets();
                entity.CreateBullet();
                entity.CreateTimer();
            }
        }

    }
}
