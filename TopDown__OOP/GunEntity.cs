using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace TopDown__OOP
{
    [Serializable]
    public abstract class GunEntity
    {
        public int damage;
        public int rangeOfAttack;
        public int speed;
        public int clip;
        public int bulletsLoaded;

        public virtual int ammo { get; set; }
        public int currAmmo;
        public int reloadTime;
        public int bulletSpeed;
        [NonSerialized] public List<Bullet> bullets;
        [NonSerialized] protected Timer testFlight;
        [NonSerialized] protected Graphics Map;
        private int time;
        public GunEntity(int damage, int rangeOfAttack, int ammo, int reloadTime, Graphics G_Bitmap, int speed)
        {
            bullets = new List<Bullet>();
            testFlight = new Timer();
            testFlight.Tick += new EventHandler(DrawBullets);
            testFlight.Interval = 40;
            this.damage = damage;
            this.rangeOfAttack = rangeOfAttack;
            this.ammo = ammo;
            this.Map = G_Bitmap;
            this.currAmmo = ammo;
            this.reloadTime = reloadTime;
            this.bulletSpeed = speed;
            
        }
        
        public int Reload()
        {
            time += 1;
            if (time == 5)
            {
                if (ammo > 0)
                {
                    bulletsLoaded += 1;
                    currAmmo += 1;
                    time = 0;
                }
                
            }
            return bulletsLoaded;
            
        }
        public abstract void Shoot(double x, double y, int dx, int dy); // передавать координаты персонажа для выстрела

        public void CreateBullets()
        {
            this.bullets = new List<Bullet>();
        }

        public abstract void CreateBullet();
        
        public void DrawBullets(object sender, EventArgs eArgs)
        {
            
            foreach (Bullet bullet in bullets.ToArray())
            {
                if (bullet.lifeTime < 200)
                {
                    bullet.Draw();
                    bullet.Move();
                    bullet.lifeTime += 10;
                }
                else
                {
                    bullets.Remove(bullet);
                    Console.WriteLine("уничтожил");
                }

            }

        }
        public void CreateTimer()
        {
            testFlight = new Timer();
            testFlight.Tick += new EventHandler(DrawBullets);
            testFlight.Interval = 30;
        }
    }
}
