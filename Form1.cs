using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LifeProject
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private int resolution;
        private GameEngine engine;

        private string Text;

        public Form1()
        {
            InitializeComponent();
        }

        private void StartGame()
        {
            if (!timer1.Enabled) 
            {
                nudResolution.Enabled = false;
                nudDensity.Enabled = false;
                Virus.Enabled = false;
                resolution = (int)nudResolution.Value;

                engine = new GameEngine
                    (
                        rows: pictureBox1.Height / resolution,
                        cols: pictureBox1.Width / resolution,
                        density: (int)nudDensity.Minimum + (int)nudDensity.Maximum - (int)nudDensity.Value,
                        spawnVirus: Virus.Checked
                    );


                Text = $"Generation: {engine.CurrentGeneration}";
                pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                graphics = Graphics.FromImage(pictureBox1.Image);
                timer1.Start();
            }
        }
        private void DrawNextGeneration()
        {
            graphics.Clear(Color.Gray);

            var field = engine.Field;
            
            for(int x = 0; x < field.GetLength(0); x++)
            {
                for(int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x,y] == true)
                    {
                        graphics.FillRectangle(Brushes.DarkGray, x * resolution, y * resolution, resolution, resolution);
                    }
                    else if (field[x,y] == null)
                    {
                        graphics.FillRectangle(Brushes.Black, x * resolution, y * resolution, resolution, resolution);
                    }
                }
            }

            Text = $"Generation: {engine.CurrentGeneration}";
            engine.NextGeneration();
            pictureBox1.Refresh();
        }

        private void StopGame()
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
                nudDensity.Enabled = true;
                nudResolution.Enabled = true;
                Virus.Enabled = true;

            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawNextGeneration();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            StartGame();

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled) return;

            if (e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                engine.AddCell(x, y);
            }
            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                engine.InfectCell(x, y);
            }
        }
    }
}
