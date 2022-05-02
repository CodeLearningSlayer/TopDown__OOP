using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace TopDown__OOP
{
    public abstract class GunEntity
    {
        public int damage;
        public int rangeOfAttack;
        public int ammo;
        public int currAmmo;
        public int reloadTime;
        public int bulletSpeed;
        public List<Bullet> bullets;
        public Timer testFlight;
        Timer reload;
        public GunEntity(int damage, int rangeOfAttack, int ammo, int reloadTime, int speed)
        {
            testFlight = new Timer();
            bullets = new List<Bullet>();
            testFlight.Tick += new EventHandler(DrawBullets);
            testFlight.Interval = 40;
            this.damage = damage;
            this.rangeOfAttack = rangeOfAttack;
            this.ammo = ammo;
            this.currAmmo = ammo;
            this.reloadTime = reloadTime;
            this.bulletSpeed = speed;
            reload = new Timer();
            reload.Tick += new EventHandler(Reloading);
            reload.Interval = reloadTime;
        }
        
        public void Reload()
        {
            if (currAmmo < ammo)
            {
                reload.Start();
            }
        }
        public abstract void Shoot(double x, double y, int dx, int dy);
        

        public void Reloading(object sender, EventArgs e)
        {
            this.currAmmo = 30;
            reload.Stop();
        }

        public  void DrawBullets(object sender, EventArgs eArgs)
        {
            //Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

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
                }

            }

        }
    }
}
