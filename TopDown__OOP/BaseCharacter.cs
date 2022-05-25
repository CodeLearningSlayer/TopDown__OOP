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
    [Serializable]
    public abstract class BaseCharacter
    {
        [NonSerialized] protected Graphics G_Bitmap;
        public int reloadTime;
        public double x { get; set; }
        public double y { get; set; }
        public int currAnimation;
        public RectangleF hitbox_base;
        public Rectangle hitbox;
        public bool isAttack { get; set; }
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
            this.isAttack = false;
        }

        public void GetHit(int damage)
        {
            health -= damage;
        }
        public abstract void Move();

        public virtual void Attack(bool isAtt)
        {
            this.isAttack = isAtt;
        }

        public abstract void RunAnim();

        public abstract void Draw();

        public abstract void Die();
        
        public virtual void CreateGraphics(Graphics G_Bitmap)
        {
            this.G_Bitmap = G_Bitmap;
        }
    }
}
