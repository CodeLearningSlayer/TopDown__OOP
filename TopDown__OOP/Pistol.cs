using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
//using System.Windows.Forms;

namespace TopDown__OOP
{
    [Serializable]
    public class Pistol:GunEntity
    {
        [NonSerialized] Image bulletImg;
        [NonSerialized] Graphics Map;

        int speed;

        public Pistol(int damage, int rangeOfAttack, int ammo, int reloadTime, Graphics G_Bitmap, int speed) : base(damage, rangeOfAttack, ammo, reloadTime, speed)
        {
            
            
           
            this.damage = damage;
            this.rangeOfAttack = rangeOfAttack;
            speed = 30;
            Map = G_Bitmap;
            this.currAmmo = ammo;
            this.reloadTime = reloadTime;
            bulletImg = Properties.Resources.pistol_bullet;
        }
        public override void Shoot(double x, double y, int dx, int dy) // передавать координаты персонажа для выстрела
        {
            Console.WriteLine("Pistol shot");
            bullets.Add(new Bullet(x, y, dx, dy, speed, Map, bulletImg));
            testFlight.Start();
            currAmmo -= 1;
        }

        public override void CreateBullet()
        {
            bulletImg = Properties.Resources.pistol_bullet;
        }
  
        

    }
}
