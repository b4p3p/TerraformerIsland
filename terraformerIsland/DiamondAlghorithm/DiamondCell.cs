using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace terraformerIsland.DiamondAlghorithm
{
    class DiamondCell
    {
        private DiamondMatrix diamondMatrix;

        public int Level {
            get {
                if (_level > -1) return _level;
                int center = diamondMatrix.Center;
                int min = Math.Min(Row, Column);
                int diff = Math.Abs(center - min);
                _level = center - diff;
                return _level; 
            }
            set { _level = value; }
        }
        private int _level = -1;

        public float Value { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public DiamondCell(DiamondMatrix diamondMatrix, int r, int c, int v)
        {
            this.diamondMatrix = diamondMatrix;
            Value = v;
            Row = r;
            Column = c;
        }

        public void SetValue( float v )
        {
            Value = v;
        }
    }
}
