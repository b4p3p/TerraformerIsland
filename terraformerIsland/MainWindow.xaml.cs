﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using terraformerIsland.DiamondAlghorithm;
using terraformerIsland.MessageController;
using terraformerIsland.PaintController;

namespace terraformerIsland
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate void DrawMatrixCallback(Grid grid, DiamondMatrix matrix);

        private double startWidth = 0;
        private double startHeight = 0;

        public static DiamondMatrix diamondMatrix;
        public static DiamondSeeder diamondSeeder;

        public int Size 
        {
            get {
                try
                {
                    return int.Parse(txtSize.Text);
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public float WaterPercentage
        {
            get
            {
                try
                {
                    return float.Parse(txtWater.Text);
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public int TileSize 
        { 
            get
            {
                try
                {
                    return int.Parse(txtTileSize.Text);
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        Point? lastCenterPositionOnTarget;
        Point? lastMousePositionOnTarget;
        Point? lastDragPoint;

        public MainWindow()
        {
            InitializeComponent();

            scrollViewer.MouseMove += OnMouseMove;
            scrollViewer.MouseLeftButtonUp += OnMouseLeftButtonUp;
            scrollViewer.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            scrollViewer.ScrollChanged += OnScrollViewerScrollChanged;

            zoomSlider.ValueChanged += OnSliderValueChanged;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmdInit_Click(null, null);
        }

        private void cmdInit_Click(object sender, RoutedEventArgs e)
        {
            if (ControlForm()) return; 

            diamondSeeder = new DiamondSeeder(TileSize, WaterPercentage);
            diamondMatrix = new DiamondMatrix(Size + 1, diamondSeeder);

            PainterMatrix.DrawMatrix(grid, diamondMatrix);

        }

        public bool ControlForm()
        {
            if (this.Size == -1)
            {
                Message.ShowMessage("Size is not a number");
                return true;
            }

            if (this.WaterPercentage == -1)
            {
                Message.ShowMessage("Water percentage is not a number");
                return true;
            }

            if (this.TileSize == -1)
            {
                Message.ShowMessage("TileSize is not a number");
                return true;
            }

            return false;
        }

        #region Eventi Zoom

        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (lastDragPoint.HasValue)
            {
                Point posNow = e.GetPosition(scrollViewer);

                double dX = posNow.X - lastDragPoint.Value.X;
                double dY = posNow.Y - lastDragPoint.Value.Y;

                lastDragPoint = posNow;

                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - dX);
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - dY);
            }
        }

        void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePos = e.GetPosition(scrollViewer);
            if (mousePos.X <= scrollViewer.ViewportWidth && mousePos.Y <
                scrollViewer.ViewportHeight) //make sure we still can use the scrollbars
            {
                scrollViewer.Cursor = Cursors.SizeAll;
                lastDragPoint = mousePos;
                Mouse.Capture(scrollViewer);
            }
        }

        private void Border_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            lastMousePositionOnTarget = Mouse.GetPosition(grid);

            if (e.Delta > 0)
            {
                zoomSlider.Value += 0.1;
            }
            else
            {
                zoomSlider.Value -= 0.1;
            }

            e.Handled = true;
            
        }

        void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            scrollViewer.Cursor = Cursors.Arrow;
            scrollViewer.ReleaseMouseCapture();
            lastDragPoint = null;
        }

        private void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange != 0 || e.ExtentWidthChange != 0)
            {
                Point? targetBefore = null;
                Point? targetNow = null;

                if (!lastMousePositionOnTarget.HasValue)
                {
                    if (lastCenterPositionOnTarget.HasValue)
                    {
                        var centerOfViewport = new Point(scrollViewer.ViewportWidth / 2,
                                                         scrollViewer.ViewportHeight / 2);
                        Point centerOfTargetNow =
                              scrollViewer.TranslatePoint(centerOfViewport, grid);

                        targetBefore = lastCenterPositionOnTarget;
                        targetNow = centerOfTargetNow;
                    }
                }
                else
                {
                    targetBefore = lastMousePositionOnTarget;
                    targetNow = Mouse.GetPosition(grid);

                    lastMousePositionOnTarget = null;
                }

                if (targetBefore.HasValue)
                {
                    double dXInTargetPixels = targetNow.Value.X - targetBefore.Value.X;
                    double dYInTargetPixels = targetNow.Value.Y - targetBefore.Value.Y;

                    double multiplicatorX = e.ExtentWidth / grid.Width;
                    double multiplicatorY = e.ExtentHeight / grid.Height;

                    double newOffsetX = scrollViewer.HorizontalOffset -
                                        dXInTargetPixels * multiplicatorX;
                    double newOffsetY = scrollViewer.VerticalOffset -
                                        dYInTargetPixels * multiplicatorY;

                    if (double.IsNaN(newOffsetX) || double.IsNaN(newOffsetY))
                    {
                        return;
                    }

                    scrollViewer.ScrollToHorizontalOffset(newOffsetX);
                    scrollViewer.ScrollToVerticalOffset(newOffsetY);
                }
            }
        }

        void OnSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            if (grid == null) return;
            if (double.IsNaN(grid.Width))
            {
                grid.Width = grid.ActualWidth;
                grid.Height = grid.ActualHeight;
                startWidth = grid.ActualWidth;
                startHeight = grid.ActualHeight;
            }

            grid.Width = startWidth * e.NewValue;
            grid.Height = startHeight * e.NewValue;
        }

        #endregion Eventi Zoom

        private void cmdNext_Click(object sender, RoutedEventArgs e)
        {
            diamondMatrix.Next();
            PainterMatrix.DrawMatrix(grid, diamondMatrix);
        }

        private void cmdGO_Click(object sender, RoutedEventArgs e)
        {
            diamondMatrix.Elaborate();
            //PainterMatrix.DrawMatrix(grid, diamondMatrix);
        }

        private void drawPerlin_Click(object sender, RoutedEventArgs e)
        {
            PainterNoise.Draw( uniformGrid, diamondMatrix);
        }
    }
}
