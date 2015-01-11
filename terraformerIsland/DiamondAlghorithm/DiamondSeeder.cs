using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using terraformerIsland.DiamondAlghorithm.Utils;

namespace terraformerIsland.DiamondAlghorithm
{
    public class DiamondSeeder
    {
        private Random rnd = new Random();
        private int tileSize;
        private float waterPercentage;

        public DiamondSeeder(int tileSize, float waterPercentage) 
        {
            this.tileSize = tileSize;
            this.waterPercentage = waterPercentage;
        }

        public void Seed( DiamondMatrix diamondMatrix )
        {
            List<DiamondCell> listKeyCell = GetListKeyCell(diamondMatrix);
            listKeyCell = OrderByLevel(listKeyCell);
            foreach (DiamondCell item in listKeyCell)
            {
                item.SetValue( NewValue (item.Level) );
                item.IsEmpty = false;
            }
        }

        public List<DiamondCell> GetListKeyCell(DiamondMatrix diamondMatrix)
        {
            int firstRow = DiamondConverter.RoundNear( tileSize, (diamondMatrix.Size / 2) * waterPercentage );
            int firstCol = firstRow;

            int lastRow = DiamondConverter.RoundNear(tileSize, diamondMatrix.Size - firstRow);
            int lastCol = lastRow;

            List<DiamondCell> list = new List<DiamondCell>();

            for (int r = 0; r < diamondMatrix.Size; r++)
            {
                for (int c = 0; c < diamondMatrix.Size; c++)
                {
                    DiamondCell item = diamondMatrix.GetCell(r, c);
                    if (item.Row < firstRow || item.Row > lastRow) continue;
                    if (item.Column < firstCol || item.Column > lastCol) continue;
                    if (item.Row % tileSize != 0) continue;
                    if (item.Column % tileSize != 0) continue;
                    list.Add(item);
                }
            }

            return list;
        }

        public List<DiamondCell> OrderByLevel(List<DiamondCell> list)
        {
            return (from DiamondCell d in list
                    orderby d.Level
                    select d).ToList<DiamondCell>();
        }

        public float NewValue( int level )
        {
            return (float)( level * rnd.NextDouble() );
        }

    }
}
