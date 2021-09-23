using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameLife
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private int resolution;
        private bool[,] vs;
        private int rows;
        private int cols;
        private int currentGeneration = 0;

        private void Start()
        {
            if (timer1.Enabled)
                return;

            currentGeneration = 0;

            nudResolution.Enabled = false;
            nudDensity.Enabled = false;


            resolution = (int)nudResolution.Value;
            rows = pictureBox1.Height / resolution;
            cols = pictureBox1.Width / resolution;

            Random random = new Random();

            vs = new bool[cols, rows];

            for (int i = 0; i < vs.GetLength(0); i++)
            {
                for (int j = 0; j < vs.GetLength(1); j++)
                {
                    vs[i, j] = random.Next((int)nudDensity.Value) == 0;
                }
            }



            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();



        }
        public Form1()
        {
            InitializeComponent();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Resolution_Click(object sender, EventArgs e)
        {

        }



        

        private void NextGeneration()
        {
            graphics.Clear(Color.Black);

            var newVS = new bool[cols,rows];

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    var countNeightboors = CountNeighboor(i,j);
                    var hasLife = vs[i, j];

                    if (hasLife)
                    {
                        graphics.FillRectangle(Brushes.Crimson, i * resolution, j * resolution, resolution - 1, resolution - 1);
                    }

                    if (!hasLife && countNeightboors == 3)
                    {
                        newVS[i, j] = true;
                    }
                    else if (hasLife && (countNeightboors < 2 || countNeightboors > 3))
                    {
                        newVS[i, j] = false;
                    }
                    else
                    {
                        newVS[i, j] = vs[i, j];
                    }
                }
            }


            vs = newVS;
            Text = $"Generation {++currentGeneration}";
            pictureBox1.Refresh();
        }




        private int CountNeighboor(int i, int j)
        {
            int count = 0;
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    var col = (x + i + cols) % cols;
                    var row = (j + y + rows) % rows;
                    var isSelfchecking = x == col && y == row;
                    var hasLife = vs[col, row];

                    if (hasLife && !isSelfchecking)
                        count++;
                }
            }


            return count;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void Stop()
        {
            if (!timer1.Enabled)
                return;

            timer1.Stop();

            nudResolution.Enabled = true;
            nudDensity.Enabled = true;
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int x = e.Location.X / resolution;
                int y = e.Location.Y / resolution;
                vs[x, y] = true;
            }

            if (e.Button == MouseButtons.Right)
            {
                int x = e.Location.X / resolution;
                int y = e.Location.Y / resolution;
                vs[x, y] = false;
            }
        }
    }
}
