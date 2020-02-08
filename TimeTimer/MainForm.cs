using System;
using System.Drawing;
using System.Windows.Forms;

namespace TimeTimer
{
    public partial class MainForm : Form
    {
        private Graphics graphics;
        private Timer timer;
        private int min = 0;
        private bool isStarted = false;
        private bool canDraw = true;

        public MainForm()
        {
            InitializeComponent();

            graphics = this.CreateGraphics();
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (timer == null)
            {
                timer = new Timer();
            }

            timer.Tick += OnPaintUpdate;
            timer.Interval = 60000;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;
                return handleParam;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!canDraw)
            {
                return;
            }

            DrawCircle();
            DrawPie();
            DrawDot();
            DrawMinNumber();
            DrawCheck();

            canDraw = false;
        }

        private void DrawCircle()
        {
            graphics.FillEllipse(Brushes.Red, 80, 80, 200, 200);
        }

        private void DrawDot()
        {
            graphics.FillEllipse(Brushes.Black, 178, 178, 5, 5);
        }

        private void DrawPie()
        {
            graphics.FillPie(Brushes.White, 79, 79, 203, 203, -90, (float)min * 6);
        }

        private void DrawMinNumber()
        {
            int minStr = 60;
            String str;

            for (int i = 1; i <= 12; i++)
            {
                double radius = (double)((double)((double)i / 12.0 * 360.0 - 90) / 180) * Math.PI;
                double x = 172 + (180 / 2 - 5) * Math.Cos(radius);
                double y = 172 + (180 / 2 - 5) * Math.Sin(radius);

                minStr -= 5;

                if (minStr == 0)
                {
                    str = minStr.ToString();
                    minStr = 60;
                }
                else
                {
                    str = minStr.ToString();
                }

                graphics.DrawString(str, new Font("Arial", 11f), Brushes.Black, (float)x, (float)y);
            }
        }

        private void DrawCheck()
        {
            if (min < 60)
            {
                return;
            }

            min = 0;

            canDraw = false;

            if (timer != null)
            {
                timer.Stop();
            }

            if (CMessageBox.Show(this, "Time is over") == DialogResult.OK)
            {
                isStarted = false;
                StartBtn.Text = "START";

                canDraw = true;

                Invalidate();

                ((UpDownBase)MinBtn).Text = "0";

                MinBtn.Enabled = true;
                ResetBtn.Enabled = true;

                OkBtn.Enabled = true;
                OkBtn.Visible = true;
            }
        }

        private void OnPaintUpdate(object sender, EventArgs e)
        {
            canDraw = true;

            min++;

            Invalidate();
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            if (isStarted)
            {
                min = 0;

                canDraw = false;

                isStarted = false;
                StartBtn.Enabled = false;

                ResetBtn.Enabled = true;

                if (timer != null)
                {
                    timer.Stop();
                }
            }
            else
            {
                isStarted = true;
                StartBtn.Text = "STOP";
                StartBtn.Enabled = true;

                ResetBtn.Enabled = false;

                if (timer != null)
                {
                    timer.Start();
                }
            }
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            min = 0;

            canDraw = true;

            Invalidate();

            ((UpDownBase)MinBtn).Text = "0";

            MinBtn.Enabled = true;

            StartBtn.Text = "START";

            StartBtn.Enabled = false;
            StartBtn.Visible = false;

            OkBtn.Enabled = true;
            OkBtn.Visible = true;
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }

            this.Dispose();

            Application.Exit();
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            int val = Convert.ToInt32(((UpDownBase)MinBtn).Text);
            if (val < 1 || val > 60)
            {
                CMessageBox.Show(this, "Please, Input 1 ~ 60 Minutes");
                return;
            }

            min = 60 - val;

            canDraw = true;

            Invalidate();

            MinBtn.Enabled = false;

            OkBtn.Enabled = false;
            OkBtn.Visible = false;

            StartBtn.Enabled = true;
            StartBtn.Visible = true;
        }
    }
}
