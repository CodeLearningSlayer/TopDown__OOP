using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;



namespace TopDown__OOP
{
    public class Bullet
    {
        private static Graphics G_Bitmap;
        private double x { get; set; }
        private double y { get; set; }
        Vector bulletDir;
        public int speed;
        public RectangleF hitbox_base;
        public Rectangle hitbox;
        Image bulletImg; 
        GraphicsUnit units = GraphicsUnit.Point;
        private int dx, dy;
        public int lifeTime;

        public Bullet(double x, double y, int dx, int dy, int speed, Graphics G_Bitmap, Image bulletImg)
        {
            
            this.bulletImg = bulletImg;
            this.x = x;
            this.y = y;
            this.dx = dx;
            this.hitbox_base = bulletImg.GetBounds(ref units);
            this.hitbox = Rectangle.Round(this.hitbox_base);
            lifeTime = 0;
            bulletDir = new Vector(dx, dy);
            bulletDir.Normalize();
            this.dy = dy;
            //renderThread = new Thread(new ThreadStart(Draw));
            //renderThread.Start();
            this.speed = speed;
        }

        
       
        public void MakeBullet()
        {
            
        }
        public void Move()
        {
            //this.x += speed * dx;
            this.x += 30 * bulletDir.X;
            this.y += 30 * bulletDir.Y;

            //this.y += speed * dy;
        }
        public void Draw()
        {
            using (Bitmap bulletMap = new Bitmap(bulletImg.Width, bulletImg.Height))
            {
                //Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                //Console.WriteLine(x);
                //Console.WriteLine(y);
                double bulletVec = Math.Atan2(this.dy, this.dx) * 180 / Math.PI + (270) * 180 / Math.PI;
                Graphics B_gfx = Graphics.FromImage(bulletMap);
                B_gfx.DrawImage(bulletImg, new System.Drawing.Point(0, 0));
                B_gfx.TranslateTransform((float)bulletMap.Width / 2, (float)bulletMap.Height / 2);
                B_gfx.RotateTransform((float)bulletVec);
                B_gfx.TranslateTransform(-(float)bulletMap.Width / 2, -(float)bulletMap.Height / 2);
                B_gfx.DrawImage(bulletImg, new System.Drawing.Point(0, 0));
                B_gfx.Dispose();
                G_Bitmap.DrawImage(bulletMap, new System.Drawing.Point((int)x, (int)y));
                this.hitbox.X = (int)this.x;
                this.hitbox.Y = (int)this.y;
            }
        }
        public static void CreateGraphics(Graphics G_Bitmap)
        {
            Bullet.G_Bitmap = G_Bitmap;
        }
        
    }
}
