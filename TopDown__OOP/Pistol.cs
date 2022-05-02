using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
//using System.Windows.Forms;

namespace TopDown__OOP
{
    public class Pistol:GunEntity
    {
        Image bulletImg;
        Graphics Map;
        Bullet testBullet;
        int elapsedTime;
        int totalTime;
        int speed;

        public Pistol(int damage, int rangeOfAttack, int ammo, int reloadTime, Graphics G_Bitmap, int speed) : base(damage, rangeOfAttack, ammo, reloadTime, speed)
        {
            
            elapsedTime = 0;
            totalTime = 3000;
            //testFlight = new System.Windows.Forms.Timer();
            //testFlight.Tick += new EventHandler(DrawBullets);
            //testFlight.Interval = 40;

            //bulletFlight.
            this.damage = damage;
            this.rangeOfAttack = rangeOfAttack;
            speed = 30;
            this.ammo = ammo;
            Map = G_Bitmap;
            this.currAmmo = ammo;
            this.reloadTime = reloadTime;
            bulletImg = Properties.Resources.pistol_bullet;
        }
        public override void Shoot(double x, double y, int dx, int dy) // передавать координаты персонажа для выстрела
        {
            //Console.WriteLine(dx);
            //Console.WriteLine(dy);
            Console.WriteLine("Pistol shot");
            bullets.Add(new Bullet(x, y, dx, dy, speed, Map, bulletImg));
            testBullet = new Bullet(x, y, dx, dy, speed, Map, bulletImg);
            testFlight.Start();
            //bulletFlight.Enabled = true;
            //DrawBullets();            
        }

        

  
    }
}
