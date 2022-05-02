using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;


namespace TopDown__OOP
{
    public abstract class BaseCharacter
    {
        protected Graphics G_Bitmap;
        public int reloadTime;
        public double x { get; set; }
        public double y { get; set; }
        public int currAnimation;
        public RectangleF hitbox_base;
        public Rectangle hitbox;
        public Vector dirVector;
        public int health;
        public int dx, dy;
        public delegate void Death(int Number);
        public Death OnDeath;
        public  BaseCharacter(double x, double y, int dx, int dy, Graphics G_Bitmap)
        {
            this.x = x;
            this.y = y;
            this.dx = dx;
            this.dy = dy;
            this.G_Bitmap = G_Bitmap;
        }
        public abstract void Move();

        public abstract void Attack();

        public abstract void RunAnim();

        public abstract void Draw();

        public abstract void Die();
        
    }
}
