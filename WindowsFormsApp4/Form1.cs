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
        private bool isLeftKeyPressed = false;
        private bool isRightKeyPressed = false;
        private bool isDownKeyPressed = false;

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
            fallingSquares = new List<Square> { fallsquare };
            fallingNow = new List<Square> { };
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MoveSquaresDown();
            Invalidate();
            this.KeyDown += new KeyEventHandler(Form_KeyDown);
            this.KeyUp += new KeyEventHandler(Form_KeyUp);
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

                    square.Position = new Point(square.Position.X, square.Position.Y + 5);
                    square.color = Color.Green;

                    if (canSpawnNewSquare)
                    {
                        Square newSquare = new Square(50, 60, new Point(0, 0));
                        fallingNow.Add(square);
                        fallingSquares.Remove(square);
                        fallingSquares.Add(newSquare);
                        fallNow = newSquare;
                        CheckAndRemoveFilledLines();
                        break;
                    }
                    else
                    {
                        timer.Stop();
                       textBox1.Text = "Игра окончена ваш счет " + total;
                    }
                }
            }
        }
        void CheckAndRemoveFilledLines()
        {
            int gameWidth = this.ClientSize.Width - 200;
            int squareSize = 50;

            HashSet<int> filledLines = new HashSet<int>();

            foreach (var square in fallingNow)
            {
                int lineY = square.Position.Y / squareSize; 
                if (!filledLines.Contains(lineY))
                {
                    int occupiedX = square.Position.X / squareSize; 
                                                                   
                    if (IsLineFilled(lineY, occupiedX))
                    {
                        filledLines.Add(lineY);
                    }
                }
            }

            // Удаляем заполненные линии
            foreach (var line in filledLines)
            {
                RemoveLine(line);
            }

        }
        bool IsLineFilled(int lineY, int occupiedX)
        {
            int requiredSquares = (this.ClientSize.Width - 100) / 50; 
            int count = 0;

            foreach (var square in fallingNow)
            {
                if (square.Position.Y / 50 == lineY)
                {
                    count++;
                }
            }

            return count >= requiredSquares; 
        }
        void RemoveLine(int lineY)
        {
            fallingNow.RemoveAll(square => square.Position.Y / 50 == lineY);
            total += 50;
        }


        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode;
            int raz = 50;

            if (key == Keys.Left && fallNow.Position.X > 0 && !isLeftKeyPressed)
            {
                fallNow.Position = new Point(fallNow.Position.X - raz, fallNow.Position.Y);
                isLeftKeyPressed = true; 
            }
            else if (key == Keys.Right && fallNow.Position.X < this.ClientSize.Width - 200 && !isRightKeyPressed)
            {
                fallNow.Position = new Point(fallNow.Position.X + raz, fallNow.Position.Y);
                isRightKeyPressed = true; 
            }
            else if (key == Keys.Down && fallNow.Position.Y < this.ClientSize.Height && !isDownKeyPressed)
            {
                fallNow.Position = new Point(fallNow.Position.X, fallNow.Position.Y + 10);
                isDownKeyPressed = true; 
            }
        }
        private void Form_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                isLeftKeyPressed = false;
            }
            else if (e.KeyCode == Keys.Right)
            {
                isRightKeyPressed = false;
            }
            else if (e.KeyCode == Keys.Down)
            {
                isDownKeyPressed = false;
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
