using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;


namespace TopDown__OOP
{
    [Serializable]
    class Enemy : BaseCharacter
    {
        [NonSerialized]private Image EnemyImg;
        [NonSerialized]private System.Windows.Forms.Timer t;
        [NonSerialized]private GraphicsUnit units = GraphicsUnit.Point;
        //public Vector dirVector;
        //public bool isAttack { get; set; }
        private System.Drawing.Point drawPoint;

        private int Condition { get; set; }
        public Enemy(double x, double y, int dx, int dy, Graphics G_Bitmap) : base(x, y, dx, dy, G_Bitmap)
        {
            this.EnemyImg = Properties.Resources.zombie_1;
            t = new System.Windows.Forms.Timer();
            
            this.hitbox_base = this.EnemyImg.GetBounds(ref units);
            this.hitbox = Rectangle.Round(this.hitbox_base);
            this.x = x;
            this.y = y;
            this.speed = 2;
            health = 50;
            this.dx = dx;
            this.dy = dy;
            this.dirVector = new Vector(dx, dy);
            dirVector.Normalize();
            t.Tick += new EventHandler(t_tick);
            t.Interval = 100;
            t.Start();
            Condition = 0;
            isAttack = false;
        }
        protected void t_tick(object sender, EventArgs eArgs)
        {
            if (isAttack == true)
            {
                RunAnim();
            }
            else
            {
                Condition = 0;
                EnemyImg = Properties.Resources.zombie_1;
                //Console.WriteLine("не бьёт");
            }
        }
       
        

        public override void Draw()
        {
            drawPoint.X = (int)this.x;
            drawPoint.Y = (int)this.y;
            using (Bitmap Z = new Bitmap(65, 65))
            {
                double zombieDir = Math.Atan2(this.dy, this.dx) * 180 / Math.PI + (89.5) * 180 / Math.PI;
                Graphics Z_Gfx = Graphics.FromImage(Z);
                Z_Gfx.TranslateTransform((float)Z.Width / 2, (float)Z.Height / 2);
                Z_Gfx.RotateTransform((float)zombieDir);
                Z_Gfx.TranslateTransform(-(float)Z.Width / 2, -(float)Z.Height / 2);
                Z_Gfx.DrawImage(this.EnemyImg, new System.Drawing.Point(0, 0));
                Z_Gfx.Dispose();

                G_Bitmap.DrawImage(Z, drawPoint);
                this.hitbox.X = (int)this.x;
                this.hitbox.Y = (int)this.y;
                
                

            }
        }

        protected override void RunAnim()
        {
            if (Condition == 0)
            {
                EnemyImg = Properties.Resources.zombie_1;
                Condition = 1;
            }
            else if (Condition == 1)
            {
                EnemyImg = Properties.Resources.zombie_2;
                Condition = 2;
            }
            else if (Condition == 2)
            {
                EnemyImg = Properties.Resources.zombie_3;
                Condition = 3;
            }
            else if (Condition == 3)
            {
                EnemyImg = Properties.Resources.zombie_4;
                Condition = 4;
            }
            else if (Condition == 4)
            {
                EnemyImg = Properties.Resources.zombie_5;
                Condition = 5;
            }
            else if (Condition == 5)
            {
                EnemyImg = Properties.Resources.zombie_6;
                Condition = 0;
            }
        }

        public override void GetHit(int damage)
        {
            base.GetHit(damage);
            this.speed -= 0.1;
        }

        public override void CreateGraphics(Graphics G_Bitmap)
        {
            base.CreateGraphics(G_Bitmap);
            this.EnemyImg = Properties.Resources.zombie_1;
        }

        public override void Move()
        {
            if ( double.IsNaN(this.dirVector.X) == false && double.IsNaN(this.dirVector.Y) == false)
            {
                this.x += Math.Round(this.dirVector.X * speed);
                this.y += Math.Round(this.dirVector.Y * speed);
            }
            else
            {
                this.x += 0;
                this.y += 0;
            }
        }
    }
}
