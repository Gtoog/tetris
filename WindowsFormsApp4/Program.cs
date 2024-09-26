using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WindowsFormsApp4.Form1;

namespace WindowsFormsApp4
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Form1());
        }
    }

    public abstract class Shape
    {
        public Point Position { get; set; }
        public Size Size { get; set; }
        public Color color { get; set; }

        public abstract void Draw(Graphics g);
        public void Fall(Graphics g)
        {
            using (Brush brush = new SolidBrush(color))
            {
                g.FillRectangle(brush, Position.X, Position.Y, Size.Width, Size.Height);
            }
        }
    }

    public class Square : Shape
    {

        public Square(int width, int height, Point position)
        {
            Size = new Size(width, height);
            Position = position;
            color = Color.Blue;
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.Green, Position.X, Position.Y, Size.Width, Size.Height);
        }

    }
    public class Line : Shape
    {
        Pen pen = new Pen(Color.Blue, 3);

        public Line(int width, int height, Point position)
        {
            Size = new Size(width, height);
            Position = position;
            color = Color.Blue;
        }

        public override void Draw(Graphics g)
        {
            g.DrawLine(pen, Position.X, Position.Y, Size.Width, Size.Height);
        }

    }

    public class SquareGlav
    {
        public int Size { get; private set; }
        public int Size1 { get; private set; }
        public Point Location { get; private set; }

        public SquareGlav(int size, int size1, Point location)
        {
            Size = size;
            Size1 = size1;
            Location = location;
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.Black, Location.X, Location.Y, Size, Size1);
        }
        public void Draw1(Graphics g)
        {
            g.FillRectangle(Brushes.Red, Location.X, Location.Y, Size, Size1);
        }
    }
}
