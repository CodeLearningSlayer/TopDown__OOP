using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TopDown__OOP
{
    [Serializable]

    public class Rifle:GunEntity
    {
        
        [NonSerialized]Image bulletImg;
        
        public Rifle(int damage, int rangeOfAttack, int ammo, int reloadTime, Graphics G_Bitmap, int speed): base(damage, rangeOfAttack, ammo, reloadTime, G_Bitmap, speed)
        {
            Map = G_Bitmap;
            this.damage = damage;
            this.rangeOfAttack = rangeOfAttack;
            this.ammo = ammo;
            this.currAmmo = 30;
            this.clip = 30;
            this.reloadTime = reloadTime;
            speed = 40;
            bulletImg = Properties.Resources.gun_bullet;
        }
        
        public override void Shoot(double x, double y, int dx, int dy)
        {
            Console.WriteLine("Rifle shot");

            bullets.Add(new Bullet(x, y, dx, dy, speed, Map, bulletImg));
            testFlight.Start();
            currAmmo -= 1;
        }

        public override void CreateBullet()
        {
            bulletImg = Properties.Resources.gun_bullet;
        }

    }
}
