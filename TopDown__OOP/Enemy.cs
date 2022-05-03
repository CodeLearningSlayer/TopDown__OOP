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
    class Enemy : BaseCharacter
    {
        Image EnemyImg;
        System.Windows.Forms.Timer t;
        GraphicsUnit units = GraphicsUnit.Point;
        //public Vector dirVector;
        public bool isAttack { get; set; }
        int speed = 2;
        public int Condition { get; set; }
        public Enemy(double x, double y, int dx, int dy, Graphics G_Bitmap) : base(x, y, dx, dy, G_Bitmap)
        {
            this.EnemyImg = Properties.Resources.zombie_1;
            t = new System.Windows.Forms.Timer();
            
            this.hitbox_base = this.EnemyImg.GetBounds(ref units);
            this.hitbox = Rectangle.Round(this.hitbox_base);
            this.x = x;
            this.y = y;
            health = 50;
            this.dx = dx;
            this.dy = dy;
            this.dirVector = new Vector(dx, dy);
            dirVector.Normalize();
            t.Tick += new EventHandler(t_tick);
            t.Interval = 40;
            t.Start();
            Condition = 0;
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
        public override void Attack()
        {
            isAttack = true;
            //Console.WriteLine("атака");

        }

        public void SetDefault()
        {
            //Console.WriteLine("не атака");

            isAttack = false;
            //Condition = 0;
        }
        

        public override void Draw()
        {
            using (Bitmap Z = new Bitmap(65, 65))
            {
                double zombieDir = Math.Atan2(this.dy, this.dx) * 180 / Math.PI + (89.5) * 180 / Math.PI;
                Graphics Z_Gfx = Graphics.FromImage(Z);
                Z_Gfx.TranslateTransform((float)Z.Width / 2, (float)Z.Height / 2);
                Z_Gfx.RotateTransform((float)zombieDir);
                Z_Gfx.TranslateTransform(-(float)Z.Width / 2, -(float)Z.Height / 2);
                Z_Gfx.DrawImage(this.EnemyImg, new System.Drawing.Point(0, 0));
                Z_Gfx.Dispose();
                G_Bitmap.DrawImage(Z, new System.Drawing.Point((int)this.x, (int)this.y));

                this.hitbox.X = (int)this.x;
                this.hitbox.Y = (int)this.y;
                G_Bitmap.DrawRectangle(Pens.Red, this.hitbox);

            }
        }

        public override void RunAnim()
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

        public override void Die()
        {

        }
        public void GetHit(int damage)
        {
                health -= damage;
        }
        public override void Move()
        {
            this.x += Math.Round(this.dirVector.X * speed);
            this.y += Math.Round(this.dirVector.Y * speed);
        }
    }
}
