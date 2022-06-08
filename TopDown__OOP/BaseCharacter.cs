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
        public double x { get; protected set; }
        public double y { get; protected set; }
        public int currAnimation;
        public RectangleF hitbox_base;
        public Rectangle hitbox;
        public bool isAttack { get; protected set; }
        public Vector dirVector;
        public int health;
        public int dx { get; protected set; }
        public int dy { get; protected set; }
        public double speed;
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

        public virtual void GetHit(int damage)
        {
            health -= damage;
        }
        public abstract void Move();

        public virtual void Attack(bool isAtt)
        {
            this.isAttack = isAtt;
        }
        public void SetDirection(int dirX, int dirY)
        {
            this.dx = dirX;
            this.dy = dirY;
        }

        public void SetVector(double vX = 0 , double vY = 0)
        {
            dirVector.X = vX;
            dirVector.Y = vY;
            dirVector.Normalize();
        }
        protected abstract void RunAnim();

        public abstract void Draw();

        //public abstract void Die();
        
        public virtual void CreateGraphics(Graphics G_Bitmap)
        {
            this.G_Bitmap = G_Bitmap;
        }
    }
}
