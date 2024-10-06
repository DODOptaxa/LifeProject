using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LifeProject
{
    internal class GameEngine
    {
        public uint CurrentGeneration {  get; private set; }
        private bool?[,] field ;
        private readonly int rows;
        private readonly int columns;

        private uint currentCellHasInfectedNeighbour;

        public bool?[,] Field {  get { return field; } }

        public GameEngine(int rows, int cols, int density)
        {
            this.rows = rows;
            this.columns = cols;
            field = new bool?[cols, rows];
            Random random = new Random();
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next(density) == 0;
                    field[x, y] = random.Next(3000) == 0 ? null : field[x, y]; 
                }
            }
        }

        private uint CountNeighbours(int x, int y)
        {
            uint count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int col = (x + i + columns) % columns;
                    int row = (y + j + rows) % rows;

                    bool isSelfCheking = col == x && row == y;
                    var hasLife = field[col, row];

                    if (hasLife == null && !isSelfCheking)
                    {
                        currentCellHasInfectedNeighbour++;
                        continue;
                    }
                    if ((hasLife == true ) && !isSelfCheking) count++;
                }
            }
            return count;
        }


        public void NextGeneration()
        {
            var newField = new bool?[columns, rows];

            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var hasLife = field[x, y];

                    uint neighboursCount = CountNeighbours(x, y);
                    uint allNeighboursCount = neighboursCount + currentCellHasInfectedNeighbour;
                    if (hasLife == true && (currentCellHasInfectedNeighbour > 0 && allNeighboursCount == 3)) newField[x, y] = null;
                    else if (hasLife == null)
                    {
                        if (neighboursCount > 3) newField[x, y] = false;
                        else if (allNeighboursCount > 5 || allNeighboursCount < 3) newField[x, y] = false;
                    }
                    else if (hasLife == false && (currentCellHasInfectedNeighbour > allNeighboursCount)) newField[x,y] = null;
                    else if (hasLife == false && currentCellHasInfectedNeighbour < 3 && allNeighboursCount == 3)
                    {
                        newField[x, y] = true;
                    }
                    else if (hasLife == true && (allNeighboursCount < 2 || allNeighboursCount > 3))
                    {
                        newField[x, y] = false;
                    }
                    else
                    {
                        newField[x, y] = field[x, y];
                    }

                    
                    currentCellHasInfectedNeighbour = 0;
                }
            }
            
            field = newField;
            CurrentGeneration++;
        }

        private bool ValidateCellPosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < columns && y < rows;
        }

        private void UpdateCell(int x, int y, bool? state)
        {
            if (ValidateCellPosition(x, y))
            { field[x, y] = state; }
        }

        public void InfectCell(int x, int y)
        {
            UpdateCell(x, y, null);
        }
        public void AddCell(int x, int y)
        {
            UpdateCell(x, y, true); 
        }
    }
}
