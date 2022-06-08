using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Text;

namespace TopDown__OOP
{
    public partial class Form2 : Form
    {
        private bool isButtonPressed = false;
        Graphics G_Form;
        Bitmap Images;
        PrivateFontCollection private_fonts;
        Font draw;
        Font helpFont;
        string control = "             Управление \n WASD - управление персонажем \n ЛКМ - выстрел \n R - перезарядка \n 1 - выбрать пистолет \n 2 - выбрать винтовку \n 4 - бег без оружия \n 5 - сохранение игры \n 6 - загрузка игры";
             
        string gameplay = "            Геймплей  \n Zомби нападают волнами, \n Zадача игрока - отбиться от них, \n C каждой волной их \n Kоличество растёт \n Сохранение возможно при \n смене волн \n Загрузка возможна при \n желании игрока \n       good luck have fun" ;
        Rectangle screen;
        Font myFont;
        Timer DrawTimer;
        Graphics G_Bitmap;
        public Form2()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(this.Form_Closing);
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isButtonPressed = true;
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.Focus();
            button3.Hide();
            Console.WriteLine(this.Focus());
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (isButtonPressed == false)
            Application.Exit();
        }

       
        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine(this.Events);
            screen = new Rectangle(0, 0, 1024, 768);
            draw = new Font("Arial", 40);
            Console.WriteLine("vvod");
            Images = new Bitmap(1024, 768);
            G_Form = this.CreateGraphics();
            G_Bitmap = Graphics.FromImage(Images);
            button1.Hide();
            button2.Hide();
            Console.WriteLine(this.CanFocus);
            DrawTimer = new Timer();
            DrawTimer.Interval = 40;
            DrawTimer.Start();
            DrawTimer.Tick += new EventHandler(DrawHelp);
            button3.Visible = true;
            private_fonts = new PrivateFontCollection();
            using (MemoryStream fontStream = new MemoryStream(Properties.Resources.Overdrive_Sunset))
            {
                // create an unsafe memory block for the font data
                System.IntPtr data = Marshal.AllocCoTaskMem((int)fontStream.Length);
                // create a buffer to read in to
                byte[] fontdata = new byte[fontStream.Length];
                // read the font data from the resource
                fontStream.Read(fontdata, 0, (int)fontStream.Length);
                // copy the bytes to the unsafe memory block
                Marshal.Copy(fontdata, 0, data, (int)fontStream.Length);
                // pass the font to the font collection
                private_fonts.AddMemoryFont(data, (int)fontStream.Length);
                // close the resource stream
                fontStream.Close();
                // free the unsafe memory
                Marshal.FreeCoTaskMem(data);
                myFont = new Font(private_fonts.Families[0], 18);
                helpFont = new Font(private_fonts.Families[0], 40);
                
            }
        }
        private void DrawHelp(object sender, EventArgs e)
        {
            G_Form.DrawImage(Images, new Point(0, 0));
            G_Bitmap.DrawRectangle(new Pen(Brushes.DarkRed), screen);
            G_Bitmap.FillRectangle(Brushes.Black, screen);

            G_Bitmap.DrawString("Help menu", helpFont, new SolidBrush(Color.Red), new Point(275, 20));
            //G_Bitmap.DrawString(description, myFont, new SolidBrush(Color.White), new Point(20, 90));
            G_Bitmap.DrawString(control, myFont, new SolidBrush(Color.White), new Point(20, 100));
            G_Bitmap.DrawString(gameplay, myFont, new SolidBrush(Color.White), new Point(400, 100));

        }

        private void button3_Click(object sender, EventArgs e)
        {
            DrawTimer.Stop();
            button1.Visible = true;
            button2.Visible = true;
            Console.WriteLine("click");
            button3.Visible = false;
            this.BackgroundImage = Properties.Resources.menu_background;
        }
    }
}
