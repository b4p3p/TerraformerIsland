using System;
using System.Collections.Generic;
using System.Globalization;
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
        private static DrawingBrush mainDrawingBrush;
        private static DrawingGroup mainDrawingGroup;
                
        private static double SizeGrid = 0;
        private static double SizeMatrix = 0;
        private static double sizePen;

        public static void CreateDrawingBrush(Grid grid, double _sizePen)
        {
            mainDrawingBrush = new DrawingBrush();
            mainDrawingGroup = new DrawingGroup();

            mainDrawingBrush.Drawing = mainDrawingGroup;

            sizePen = _sizePen;
            grid.Background = mainDrawingBrush;
        }

        public static void DrawGrid(Grid grid, int sizeMatrix)
        {
            GeometryDrawing geometryDrawing = new GeometryDrawing();
            GeometryGroup geometryGroup = new GeometryGroup();
            geometryDrawing.Geometry = geometryGroup;
            SetLayout(geometryDrawing , Brushes.LightBlue, Brushes.Black, sizePen);
            
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
                DrawLine(geometryGroup, grid, x, 0, x, SizeGrid);
            }
            //horizzontal
            for (double y = 0; y <= SizeGrid + 1; y += SizeGrid / sizeMatrix)
            {
                DrawLine(geometryGroup, grid, 0, y, SizeGrid, y);
            }

            mainDrawingGroup.Children.Add(geometryDrawing);
        }
        internal static void DrawMatrix(Grid grid, DiamondMatrix diamondMatrix)
        {
            Painter.CreateDrawingBrush(grid, 0.3);
            
            Painter.DrawGrid(grid, diamondMatrix.Size - 1 );

            GeometryDrawing geometryDrawing = new GeometryDrawing();
            GeometryGroup geometryGroup = new GeometryGroup();
            geometryDrawing.Geometry = geometryGroup;
            SetLayout(geometryDrawing, Brushes.White, Brushes.Black, sizePen);

            foreach (DiamondCell item in diamondMatrix.GetMatrix())
            {
                if (!item.IsEmpty)
                    DrawBubble(geometryGroup, item);
            }
            mainDrawingGroup.Children.Add(geometryDrawing);

            DrawingMatrixText(diamondMatrix);
        }

        private static void DrawingMatrixText(DiamondMatrix diamondMatrix)
        {
            GeometryDrawing geometryDrawing = new GeometryDrawing();
            GeometryGroup geometryGroup = new GeometryGroup();
            geometryDrawing.Geometry = geometryGroup;
            SetLayout(geometryDrawing, Brushes.Black, Brushes.Black, 0.1);

            foreach (DiamondCell item in diamondMatrix.GetMatrix())
            {
                if (!item.IsEmpty)
                    DrawText(geometryGroup, item);
            }
            mainDrawingGroup.Children.Add(geometryDrawing);
        }

        private static void DrawText(GeometryGroup geometryGroup, DiamondCell item)
        {
            double y = item.Row * SizeGrid / SizeMatrix;
            double x = (item.Column * SizeGrid / SizeMatrix) - 8;

            string strValue = item.Value.ToString("F2");

            FormattedText text = new FormattedText(strValue + "\nLv: " + item.Level + "\n" + item.Debug,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Tahoma"),
                16,
                Brushes.Black);
            text.TextAlignment = TextAlignment.Center;
            Geometry textGeometry = text.BuildGeometry(new Point(x, y));

            geometryGroup.Children.Add(textGeometry);
        }

        private static void SetLayout(GeometryDrawing geometry , SolidColorBrush colorBackground, SolidColorBrush colorPen, double sizePen)
        {
            geometry.Brush = colorBackground;
            geometry.Pen = new Pen(colorPen, sizePen);         
        }

        public static void DrawLine(GeometryGroup geometryGroup,
                                    Grid grid, double x1, double y1 , double x2, double y2)
        {
            //geometryDrawing.Geometry
            geometryGroup.Children.Add(new LineGeometry(new Point(x1, y1), new Point(x2, y2)));
        }

        private static void DrawBubble(GeometryGroup geometryGroup, DiamondCell item)
        {
            double y = item.Row * SizeGrid / SizeMatrix;
            double x = item.Column * SizeGrid / SizeMatrix;
            
            EllipseGeometry ell = new EllipseGeometry(new Point(x, y), 10, 10);           

            geometryGroup.Children.Add(ell);
        }
        
    }
}
