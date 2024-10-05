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
        private bool[,] field;
        private int rows;
        private int columns;

        private int NeighbourCountMin = 2;
        private int NeighbourCountMax = 3;

        private int NeigbourRadius_X_Min = -1;
        private int NeigbourRadius_X_Max = 1;
        private int NeigbourRadius_Y_Min = -1;
        private int NeigbourRadius_Y_Max = 1;

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
                countMin.Enabled = false;
                countMax.Enabled = false;
                rad_X_Max.Enabled = false;
                rad_X_Min.Enabled = false;
                rad_Y_Max.Enabled = false;
                rad_Y_Min.Enabled = false;
                
                resolution = (int)nudResolution.Value;

                NeighbourCountMin = (int)countMin.Value;
                NeighbourCountMax = (int)countMax.Value;

                NeigbourRadius_X_Min = -1 * (int)rad_X_Min.Value;
                NeigbourRadius_X_Max = (int)rad_X_Max.Value;
                NeigbourRadius_Y_Min = -1 * (int)rad_Y_Min.Value;
                NeigbourRadius_Y_Max = (int)rad_Y_Max.Value;

                rows = pictureBox1.Height / resolution;
                columns = pictureBox1.Width / resolution;
                field = new bool[columns, rows];

                Random rnd = new Random();
                for (int x = 0; x < columns; x++) 
                {
                    for (int y = 0; y < rows; y++) 
                    {
                        field[x, y] = rnd.Next((int)nudDensity.Value) == 0;
                    }
                }
                pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                graphics = Graphics.FromImage(pictureBox1.Image);
                timer1.Start();
            }
        }
        private void StopGame()
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
                nudDensity.Enabled = true;
                nudResolution.Enabled = true;
                countMax.Enabled = true;
                countMin.Enabled = true;
                rad_X_Max.Enabled = true;
                rad_X_Min.Enabled = true;
                rad_Y_Max.Enabled = true;
                rad_Y_Min.Enabled = true;
            }
        }
        private void NextGeneration()
        {
            graphics.Clear(Color.Gray);
            var newField = new bool[columns, rows];

            for(int x = 0; x < columns; x++)
            {
                for(int y = 0; y < rows; y++)
                {
                    var neighboursCount = CountNeighbours(x, y);
                    var hasLife = field[x, y];

                    if (!hasLife && neighboursCount == NeighbourCountMax)
                    {
                        newField[x, y] = true;
                    }
                    else if (hasLife && (neighboursCount < NeighbourCountMin || neighboursCount > NeighbourCountMax))
                    {
                        newField[x, y] = false;
                    }
                    else
                    {
                        newField[x, y] = field[x, y];
                    }

                    if (hasLife)
                    {
                        graphics.FillRectangle(Brushes.DarkGray, x * resolution, y * resolution, resolution, resolution);
                    }
                }
            }
            field = newField;
            pictureBox1.Refresh();
        }

        private int CountNeighbours(int x, int y)
        {
            int count = 0;
            for (int i = NeigbourRadius_X_Min; i < NeigbourRadius_X_Max+1; i++)
            {
                for (int j = NeigbourRadius_Y_Min; j < NeigbourRadius_Y_Max+1; j++)
                {
                    int col = (x + i + columns) % columns;
                    int row = (y + j + rows) % rows;

                    bool isSelfCheking = col == x && row == y;
                    var hasLife = field[col, row];

                    if (hasLife && !isSelfCheking) count++;
                }
            }
            return count;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            StartGame();

        }


    }
}
