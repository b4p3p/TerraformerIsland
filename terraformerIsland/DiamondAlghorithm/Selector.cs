using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace terraformerIsland.DiamondAlghorithm
{
    class Selector
    {
        private DiamondMatrix diamondMatrix;

        public Selector(DiamondMatrix diamondMatrix)
        {
            // TODO: Complete member initialization
            this.diamondMatrix = diamondMatrix;
        }

        public DiamondCell TopLeft
        {
            get
            {
                return diamondMatrix.GetMatrix()[0, 0];
            }
        }

        public DiamondCell TopRight
        {
            get
            {
                return diamondMatrix.GetMatrix()[0, diamondMatrix.Size - 1];
            }
        }

        public DiamondCell BottomLeft
        {
            get
            {
                return diamondMatrix.GetMatrix()[diamondMatrix.Size - 1, 0];
            }
        }

        public DiamondCell BottomRight
        {
            get
            {
                return diamondMatrix.GetMatrix()[diamondMatrix.Size - 1, diamondMatrix.Size - 1];
            }
        }

        public DiamondCell MiddleTop 
        { 
            get
            {
                return diamondMatrix.GetMatrix()[0, (diamondMatrix.Size - 1)/2];
            }
        }

        public DiamondCell MiddleRight
        {
            get
            {
                return diamondMatrix.GetMatrix()[(diamondMatrix.Size - 1) / 2, (diamondMatrix.Size - 1)];
            }
        }

        public DiamondCell MiddleBottom
        {
            get
            {
                return diamondMatrix.GetMatrix()[diamondMatrix.Size - 1, (diamondMatrix.Size - 1) / 2];
            }
        }

        public DiamondCell MiddleLeft
        {
            get
            {
                return diamondMatrix.GetMatrix()[(diamondMatrix.Size - 1) / 2, 0];
            }
        }

        public DiamondCell Center
        {
            get
            {
                return diamondMatrix.GetMatrix()[(diamondMatrix.Size - 1) / 2, (diamondMatrix.Size - 1) / 2];
            }
        }



    }
}
