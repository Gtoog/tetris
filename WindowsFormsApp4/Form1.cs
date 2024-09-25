using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        private SquareGlav square;
        private SquareGlav square1;
        private Square fallsquare;
        private Square fallNow;
        private Timer timer;
        private List<Square> fallingSquares, fallingNow;
        private int total;

        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            square = new SquareGlav(250, 350, new Point(0, 0));
            square1 = new SquareGlav(120, 350, new Point(250, 0));
            fallsquare = new Square(50, 60, new Point(0, 0));
            timer = new Timer();
            timer.Interval = 500;
            timer.Tick += Timer_Tick;
            timer.Start();

            // Начальная фигура
            fallingSquares = new List<Square> { fallsquare };
            fallingNow = new List<Square> { };
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MoveSquaresDown();
            Invalidate();
            this.KeyDown += new KeyEventHandler(Form_KeyDown);
        }

        private void MoveSquaresDown()
        {
            bool canSpawnNewSquare = true;

            foreach (var square in fallingSquares)
            {
                fallNow = square;
                bool isTouching = fallingNow.Any(otherSquare =>
                    square.Position.X < otherSquare.Position.X + otherSquare.Size.Width &&
                    square.Position.X + square.Size.Width > otherSquare.Position.X &&
                    square.Position.Y + square.Size.Height > otherSquare.Position.Y &&
                    square.Position.Y < otherSquare.Position.Y + otherSquare.Size.Height);
                
                if (square.Position.Y + square.Size.Height + 10 < this.ClientSize.Height && !isTouching)
                {
                    square.Position = new Point(square.Position.X, square.Position.Y + 10);
                }
                else
                {
                    if (square.Position.Y <= 0)
                    {
                        canSpawnNewSquare = false;
                    }

                    square.Position = new Point(square.Position.X, square.Position.Y + 10);
                    square.color = Color.Green;

                    if (canSpawnNewSquare)
                    {
                        Square newSquare = new Square(50, 60, new Point(0, 0));
                        fallingNow.Add(square);
                        fallingSquares.Remove(square);
                        fallingSquares.Add(newSquare);
                        break;
                    }
                    else
                    {
                        timer.Stop();
                       // textBox1.Text = "Игра окончена ваш счет " + total;
                    }
                }
            }
        }
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode;

            if (key == Keys.Left && fallNow.Position.X > 0)
            {
                fallNow.Position = new Point(fallNow.Position.X - 10, fallNow.Position.Y);
            }
            else if (key == Keys.Right && fallNow.Position.X < this.ClientSize.Width)
            {
                fallNow.Position = new Point(fallNow.Position.X + 10, fallNow.Position.Y);
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            square.Draw(e.Graphics);
            square1.Draw1(e.Graphics);
            foreach (var square in fallingSquares)
            {
                square.Fall(e.Graphics);
            }
            foreach (var square in fallingNow)
            {
                square.Draw(e.Graphics);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //textBox1.Text = "Ваш счет " + total;
        }
    }
}
