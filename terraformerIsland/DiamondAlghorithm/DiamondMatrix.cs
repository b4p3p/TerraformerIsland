using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace terraformerIsland.DiamondAlghorithm
{
    public class DiamondMatrix
    {
        internal Selector Selector { get; set; }
        
        DiamondCell[,] matrix;
        public List<DiamondMatrix> ListOpen = new List<DiamondMatrix>();
        
        public int Size { get; set; }

        public int Center { 
            get
            {
                return (Size - 1) / 2;
            }
        }

        public DiamondMatrix(int size, DiamondSeeder seeder)
        {
            Selector = new Selector(this);
            Size = size;
            matrix = new DiamondCell[Size, Size];
            InitDiamondCell();
            seeder.Seed(this);
            ListOpen.Add(this);
        }

        /// <summary>
        /// Construct for submatrix
        /// </summary>
        /// <param name="diamondMatrix"></param>
        /// <param name="rowStart"></param>
        /// <param name="rowEnd"></param>
        /// <param name="colStart"></param>
        /// <param name="colEnd"></param>
        private  DiamondMatrix(DiamondMatrix diamondMatrix, int rowStart, int rowEnd, int colStart, int colEnd)
        {
            Selector = new Selector(this);
            Size = rowEnd - rowStart;
            matrix = new DiamondCell[Size, Size];
            InitDiamondCell(diamondMatrix, rowStart, rowEnd, colStart, colEnd);
            
        }

        private void InitDiamondCell()
        {
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    matrix[r,c] = new DiamondCell(this,r,c,0);
                }
            }
        }

        /// <summary>
        /// InitDiamondCell for SubMatrix
        /// </summary>
        /// <param name="diamondMatrix"></param>
        /// <param name="rowStart"></param>
        /// <param name="rowEnd"></param>
        /// <param name="colStart"></param>
        /// <param name="colEnd"></param>
        private void InitDiamondCell(DiamondMatrix diamondMatrix, int rowStart, int rowEnd, int colStart, int colEnd)
        {
            int row = rowStart;
            for (int r = 0; r < Size; r++)
            {
                int col = colStart;
                for (int c = 0; c < Size; c++)
                {
                    matrix[r, c] = diamondMatrix.GetCell(row, col);
                    col++;
                }
                row++;
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

        internal DiamondCell GetCell(int r, int c)
        {
            return matrix[r, c];
        }

        internal void Elaborate()
        {
            while ( ListOpen.Count > 0)
            {
                Next();
            }
        }

        internal void Next()
        {
   

            
            //get first
            DiamondMatrix matrix = ListOpen.FirstOrDefault<DiamondMatrix>();
            if (matrix == null) return;
            Elaborate(matrix);
            ListOpen.RemoveAt(0);

            if ( !(matrix.Selector.TopLeft.Column + 1 == matrix.Selector.MiddleTop.Column &&
                   matrix.Selector.MiddleTop.Column + 1 == matrix.Selector.TopRight.Column) )
            {
                DiamondMatrix m1 = GetSubMatrix(
                    matrix.Selector.TopLeft.Row,
                    matrix.Selector.Center.Row + 1,
                    matrix.Selector.TopLeft.Column,
                    matrix.Selector.Center.Column + 1);

                DiamondMatrix m2 = GetSubMatrix(
                    matrix.Selector.TopLeft.Row,
                    matrix.Selector.Center.Row + 1,
                    matrix.Selector.Center.Column,
                    matrix.Selector.TopRight.Column + 1);

                DiamondMatrix m3 = GetSubMatrix(
                    matrix.Selector.MiddleLeft.Row,
                    matrix.Selector.MiddleBottom.Row + 1,
                    matrix.Selector.MiddleLeft.Column,
                    matrix.Selector.MiddleBottom.Column + 1);

                DiamondMatrix m4 = GetSubMatrix(
                    matrix.Selector.Center.Row,
                    matrix.Selector.BottomRight.Row + 1,
                    matrix.Selector.Center.Column,
                    matrix.Selector.BottomRight.Column + 1);

                ListOpen.Add(m1);
                ListOpen.Add(m2);
                ListOpen.Add(m3);
                ListOpen.Add(m4);
            }
        }

        private void Elaborate(DiamondMatrix matrix)
        {
            //Control
            if ( matrix.Selector.TopLeft.IsEmpty )
            {
                matrix.Selector.TopLeft.Value = 0;
                matrix.Selector.TopLeft.IsEmpty = false;
            }
            if (matrix.Selector.TopRight.IsEmpty)
            {
                matrix.Selector.TopRight.Value = 0;
                matrix.Selector.TopRight.IsEmpty = false;
            }
            if (matrix.Selector.BottomLeft.IsEmpty)
            {
                matrix.Selector.BottomLeft.Value = 0;
                matrix.Selector.BottomLeft.IsEmpty = false;
            }
            if (matrix.Selector.BottomRight.IsEmpty)
            {
                matrix.Selector.BottomRight.Value = 0;
                matrix.Selector.BottomRight.IsEmpty = false;
            }
            
            //calculate
            if (matrix.Selector.MiddleTop.IsEmpty)
            {
                matrix.Selector.MiddleTop.Value = (matrix.Selector.TopLeft.Value + matrix.Selector.TopRight.Value ) / 2;
                matrix.Selector.MiddleTop.IsEmpty = false;
            }
            if (matrix.Selector.MiddleRight.IsEmpty)
            {
                matrix.Selector.MiddleRight.Value = (matrix.Selector.TopRight.Value + matrix.Selector.BottomRight.Value) / 2;
                matrix.Selector.MiddleRight.IsEmpty = false;
            }
            if (matrix.Selector.MiddleBottom.IsEmpty)
            {
                matrix.Selector.MiddleBottom.Value = (matrix.Selector.BottomLeft.Value + matrix.Selector.BottomRight.Value) / 2;
                matrix.Selector.MiddleBottom.IsEmpty = false;
            }
            if (matrix.Selector.MiddleLeft.IsEmpty)
            {
                matrix.Selector.MiddleLeft.Value = (matrix.Selector.TopLeft.Value + matrix.Selector.BottomLeft.Value) / 2;
                matrix.Selector.MiddleLeft.IsEmpty = false;
            }
            if (matrix.Selector.Center.IsEmpty)
            {
                matrix.Selector.Center.Value = (
                    matrix.Selector.MiddleBottom.Value + 
                    matrix.Selector.MiddleTop.Value + 
                    matrix.Selector.MiddleLeft.Value + 
                    matrix.Selector.MiddleRight.Value) / 4;
                matrix.Selector.Center.IsEmpty = false;
            }


        }

        private DiamondMatrix GetSubMatrix(int rowStart, int rowEnd, int colStart, int colEnd)
        {
            return new DiamondMatrix(this, rowStart, rowEnd, colStart, colEnd);
        }

        
    }
}
