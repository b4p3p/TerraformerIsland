using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace terraformerIsland.DiamondAlghorithm
{
    class DiamondMatrix
    {
        DiamondCell[,] matrix;
        
        public int Size { get; set; }

        public int Center { 
            get
            {
                return (Size - 1) / 2;
            }
        }

        public DiamondMatrix(int size, DiamondSeeder seeder)
        {
            Size = size + 1;
            matrix = new DiamondCell[Size, Size];
            InitDiamondCell();
            seeder.Seed(this);
        }

        private void InitDiamondCell()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    matrix[i,j] = new DiamondCell(this,i,j,0);
                }
            }
        }

        public bool SetValue(int r , int c, int v)
        {
            try
            {
                matrix[r, c].SetValue(v);
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }

        public DiamondCell[,] GetMatrix()
        {
            return matrix;
        }

        
    }
}
