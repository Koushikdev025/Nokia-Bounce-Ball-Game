using System;
using System.Drawing;
using System.Windows.Forms;

namespace BounceBallGame
{
    public partial class Form1 : Form
    {
        private int ballX = 5;
        private int ballY = 5;
        private int ballSize = 21;
        private int paddleWidth = 100;
        private int paddleHeight = 10;
        private int paddleX;
        private int score = 0;

        private int ballDX = 5;
        private int ballDY = 5;

        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Width = 502;
            this.Height = 500;
            this.Text = "Bounce Ball - Nokia Style";

            paddleX = (this.ClientSize.Width - paddleWidth) / 2;

            timer.Interval = 20; // ~50 FPS
            timer.Tick += Timer_Tick;
            timer.Start();

            this.KeyDown += Form1_KeyDown;
            this.Paint += Form1_Paint;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Move the ball
            ballX += ballDX;
            ballY += ballDY;

            // Bounce off walls
            if (ballX <= 0 || ballX + ballSize >= this.ClientSize.Width)
                ballDX = -ballDX;

            if (ballY <= 0)
                ballDY = -ballDY;

            // Bounce off paddle
            Rectangle ballRect = new Rectangle(ballX, ballY, ballSize, ballSize);
            Rectangle paddleRect = new Rectangle(paddleX, this.ClientSize.Height - 50, paddleWidth, paddleHeight);
            if (ballRect.IntersectsWith(paddleRect))
            {
                ballDY = -ballDY;
                score++;
            }

            // Ball falls below paddle
            if (ballY + ballSize >= this.ClientSize.Height)
            {
                timer.Stop();
                MessageBox.Show($"Game Over! Your score: {score}");
                this.Close();
            }

            this.Invalidate(); // Redraw
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillEllipse(Brushes.Red, ballX, ballY, ballSize, ballSize);
            g.FillRectangle(Brushes.Blue, paddleX, this.ClientSize.Height - 50, paddleWidth, paddleHeight);

            g.DrawString($"Score: {score}", new Font("Arial", 12), Brushes.Black, 10, 10);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            int moveSpeed = 25;
            if (e.KeyCode == Keys.Left && paddleX > 0)
                paddleX -= moveSpeed;
            else if (e.KeyCode == Keys.Right && paddleX + paddleWidth < this.ClientSize.Width)
                paddleX += moveSpeed;
        }
    }
}
