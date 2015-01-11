using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using terraformerIsland.DiamondAlghorithm;


namespace terraformerIsland.PaintController
{

    class PainterNoise
    {

        internal static void Draw(UniformGrid grid, DiamondMatrix diamondMatrix)
        {
            CreateImage(grid, diamondMatrix);
            //grid.Background = new ImageBrush(image);
        }

        private static void CreateImage(UniformGrid wrap, DiamondMatrix diamondMatrix)
        {
            double width = wrap.ActualWidth / diamondMatrix.Size; // for example
            double height = wrap.ActualHeight / diamondMatrix.Size; // for example
            
            int size = diamondMatrix.Size + 1 ;

            wrap.Children.Clear();
            wrap.Columns = diamondMatrix.Size;

            byte[] buffer = diamondMatrix.ToArray();
            int cont = 0;

            for (int r = 0; r < diamondMatrix.Size; r++)
            {
                for (int c = 0; c < diamondMatrix.Size; c++)
                {
                    wrap.Children.Add(new Rectangle() 
                    {
                        Width = width, 
                        Height = height, 
                        Fill = new SolidColorBrush( Color.FromRgb(buffer[cont], buffer[cont], buffer[cont]))
                    });
                    cont++;
                }
            }

        }
    }
}
