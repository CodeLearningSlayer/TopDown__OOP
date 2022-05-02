using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows;


namespace TopDown__OOP
{
    public class Player : BaseCharacter
    {
        public Image PlayerImg;
        System.Windows.Forms.Timer t;
        public static bool goLeft;
        public static bool goRight;
        public bool isGunChosen;
        public static bool goUp;
        public static bool goDown;
        public int health;
        private List<GunEntity> guns;
        public GunEntity currGun;
        GraphicsUnit units = GraphicsUnit.Point;
        int speed = 5;
        public int Condition { get; set; }
        public Player(double x, double y, int dx, int dy, Graphics G_Bitmap) : base(x, y, dx, dy, G_Bitmap)
        {
            guns = new List<GunEntity>();
            this.PlayerImg = Properties.Resources.walk_1;
            t = new System.Windows.Forms.Timer();
            currGun = null;
            this.hitbox_base = this.PlayerImg.GetBounds(ref units);
            this.hitbox = Rectangle.Round(this.hitbox_base);
            isGunChosen = false;
            t.Tick += new EventHandler(t_tick);
            t.Interval = 200;
            health = 100;
            this.x = x;
            this.y = y;
            this.dx = 0;
            this.dy = 0;
            dirVector = new Vector(dx, dy);
            currGun = new Pistol(10, 15, 9, 5, G_Bitmap, 30);
            guns.Add(currGun);
            guns.Add(new Rifle(100, 15, 30, 6, G_Bitmap,40));

            t.Start();
            Condition = 0;
        }
        protected void t_tick(object sender, EventArgs eArgs)
        {
            this.RunAnim();
            // слежение за курсором
        }
        public override void Attack()
        {

        }

        public override void Draw()
        {
            using (Bitmap bmpToRotate = new Bitmap(this.PlayerImg.Width, this.PlayerImg.Height))
            {
                double angle = Math.Atan2(this.dy, this.dx) * 180 / Math.PI + (-90) * Math.PI / 180;
                //Console.WriteLine(dy);
                //Console.WriteLine(dx);
                //Console.WriteLine(angle);
                Graphics gfx = Graphics.FromImage(bmpToRotate);
                gfx.TranslateTransform((float)bmpToRotate.Width / 2, (float)bmpToRotate.Height / 2);
                gfx.RotateTransform((float)angle);
                gfx.TranslateTransform(-(float)bmpToRotate.Width / 2, -(float)bmpToRotate.Height / 2);
                gfx.DrawImage(PlayerImg, new System.Drawing.Point(0, 0));
                gfx.Dispose();
                this.hitbox.X = (int)this.x;
                this.hitbox.Y = (int)this.y;
                G_Bitmap.DrawImage(bmpToRotate, new System.Drawing.Point((int)this.x, (int)this.y));
                G_Bitmap.DrawRectangle(Pens.Blue, this.hitbox);
            }
            //G_Bitmap.DrawImage(PlayerImg, new Point(this.x, this.y));
        }

        public override void RunAnim()
        {
            if ((Player.goUp || Player.goDown || Player.goLeft || Player.goRight) && isGunChosen == false) {
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
            if (Player.goUp || Player.goDown) {

                if (Player.goUp == true)
                {
                    this.dirVector.Y = -1;
                }
                if (Player.goDown == true)
                {
                    this.dirVector.Y = 1;
                }
            }
            else
            {
                this.dirVector.Y = 0;
            }
            if (Player.goLeft || Player.goRight) {

                if (Player.goLeft == true)
                {
                    this.dirVector.X = -1;
                }
                if (Player.goRight == true)
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
        public override void Die()
        {

        }

        public void GetHit(int damage)
        {
            health -= damage;
        }

        public void ChangeGun(string gun)
        {
            if (gun == "pistol")
            {
                this.PlayerImg = Properties.Resources.player_9mmhandgun;
                this.speed = 4;
                currGun = guns[0];
                 
                isGunChosen = true;

            }
            else if (gun == "rifle")
            {
                if (guns.Count >= 2)
                {
                    this.PlayerImg = Properties.Resources.player_chaingun;
                    this.speed = 3;
                    currGun = guns[1];
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
                //this.PlayerImg = Properties.Resources.player_pumpgun_stand;
                isGunChosen = false;
                this.speed = 5;
                this.PlayerImg = Properties.Resources.walk_1;
            }
            Console.WriteLine(currGun);
        }

        public void ReloadGun()
        {

        }

        public void GetWeapon(GunEntity gun)
        {
            guns.Add(gun);
            
        }

    }
}
