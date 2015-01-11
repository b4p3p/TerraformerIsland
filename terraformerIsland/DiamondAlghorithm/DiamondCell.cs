using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace terraformerIsland.DiamondAlghorithm
{
    public class DiamondCell
    {
        private DiamondMatrix diamondMatrix;

        public int Level {
            get {
                if (_level > -1) return _level;
                int center = diamondMatrix.Center;
                int X = Math.Abs(center - Column);
                int Y = Math.Abs(center - Row);
                int max = Math.Max(X, Y);
                _level = center - max;
                
                return _level; 
            }
            set { _level = value; }
        }
        private int _level = -1;

        public string Debug 
        { 
            get
            {
                //return Math.Min(Row, Column);
                return "";
            }
        }

        public float Value { get; set; }

        public int Row { get; set; }

        public int Column { get; set; }

        public bool IsEmpty { get; set; }

        public DiamondCell(DiamondMatrix diamondMatrix, int r, int c, int v)
        {
            this.diamondMatrix = diamondMatrix;
            Value = v;
            Row = r;
            Column = c;
            IsEmpty = true;
        }

        public void SetValue( float v )
        {
            Value = v;
        }

        
    }
}
