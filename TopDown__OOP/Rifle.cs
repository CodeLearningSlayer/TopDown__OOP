using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TopDown__OOP
{
    public class Rifle:GunEntity
    {
        int reloadTime;
        Image bulletImg;
        Graphics Map;
        int speed;
        public Rifle(int damage, int rangeOfAttack, int ammo, int reloadTime, Graphics G_Bitmap, int speed): base(damage, rangeOfAttack, ammo, reloadTime, speed)
        {
            Map = G_Bitmap;
            this.damage = damage;
            this.rangeOfAttack = rangeOfAttack;
            this.ammo = ammo;
            this.reloadTime = reloadTime;
            speed = 40;
            bulletImg = Properties.Resources.gun_bullet;
        }

        public override void Shoot(double x, double y, int dx, int dy)
        {
            Console.WriteLine("Rifle shot");
            
            bullets.Add(new Bullet(x, y, dx, dy, speed, Map, bulletImg));
            testFlight.Start();
        }

        
    }
}
