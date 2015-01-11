using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using terraformerIsland.DiamondAlghorithm;

namespace terraformerIsland.PaintController
{
    class Painter
    {
        private static DrawingBrush drawingBrush;
        private static GeometryDrawing myGeometryDrawing;
        private static GeometryGroup geometryGroup;
        private static double SizeGrid = 0;
        private static double SizeMatrix = 0;

        public static void CreateDrawingBrush(double sizePen)
        {
            drawingBrush = new DrawingBrush();

            myGeometryDrawing = new GeometryDrawing();
            myGeometryDrawing.Brush = Brushes.LightBlue;
            myGeometryDrawing.Pen = new Pen(Brushes.Black, sizePen);

            geometryGroup = new GeometryGroup();
        }

        public static void DrawGrid(Grid grid, int sizeMatrix)
        {
            SizeMatrix = sizeMatrix;

            if (grid.ActualHeight < grid.ActualWidth)
            {
                SizeGrid = grid.ActualHeight;
            }
            else
            {
                SizeGrid = grid.ActualWidth;
            }

            //vertical
            for (double x = 0; x <= SizeGrid + 1; x += SizeGrid / sizeMatrix)
            {
                DrawLine(grid, x, 0, x, SizeGrid);
            }
            //horizzontal
            for (double y = 0; y <= SizeGrid + 1; y += SizeGrid / sizeMatrix)
            {
                DrawLine(grid, 0, y, SizeGrid, y );
            }
        }

        public static void DrawLine(Grid grid, double x1, double y1 , double x2, double y2)
        {
            geometryGroup.Children.Add(new LineGeometry(new Point(x1, y1), new Point(x2, y2)));
            //geometryGroup.Children.Add(new LineGeometry(new Point(x1, 0), new Point(10, 100)));
            
            myGeometryDrawing.Geometry = geometryGroup;
            drawingBrush.Drawing = myGeometryDrawing;

            grid.Background = drawingBrush;
        }

        internal static void DrawMatrix(DiamondAlghorithm.DiamondMatrix diamondMatrix)
        {
            Brush tmpBr = myGeometryDrawing.Brush;
            Pen tmpPen = myGeometryDrawing.Pen;

            myGeometryDrawing.Brush = Brushes.White;
            myGeometryDrawing.Pen = new Pen(Brushes.Black, 0.2);

            foreach (DiamondCell item in diamondMatrix.GetMatrix())
            {
                if ( item.Value != 0 )
                    DrawBubble(item);
            }

            myGeometryDrawing.Brush = tmpBr;
            myGeometryDrawing.Pen = tmpPen;

        }

        private static void DrawBubble(DiamondCell item)
        {
            EllipseGeometry ell = new EllipseGeometry(new Point(item.Row * SizeGrid / SizeMatrix, item.Column * SizeGrid / SizeMatrix), 10, 10);
            geometryGroup.Children.Add(ell);
        }
        
    }
}
