using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Threading;
using System.Windows.Shapes;

namespace MapdrawingTest
{
    public class PopulationenRendering
    {
        WriteableBitmap writeableBitmap;
        System.Windows.Controls.Image i;
        private static double north = 69.06;
        private static double south = 55.336944;
        private static double east = 24.150556;
        private static double west = 11.113056;
        private ConcurrentQueue<System.Windows.Point> cq;
        private bool enableRendering;
        private Viewbox viewBox;

        private CrossHair crossHair;

        public void AddCoordinate(System.Windows.Point p)
        {
            cq.Enqueue(p);
        }

        public void StopRendering()
        {
            enableRendering = false;
        }

        public void StartRendering()
        {
            new Thread(() =>
            {
                Debug.WriteLine("Rendering started");
                enableRendering = true;
                System.Windows.Point p;
                while (enableRendering)
                {
                    if (cq.Count > 0)
                    {
                        cq.TryDequeue(out p);
                        Render(p.X, p.Y);
                        Thread.Sleep(0);
                    }
                    else
                    {
                        Thread.Sleep(250);
                    }
                    
                    
                }
            }).Start();
            
        }

        private System.Drawing.Point TransForm(System.Windows.Point point)
        {
            return new System.Drawing.Point(
                    (int)(point.X > east ? 300 : (point.X < west ? 0 : (point.X - west) * 300 / (east - west))),
                    (int)(point.Y > north ? 0 : (point.Y < south ? 600 : 703 - (point.Y - south) * 703 / (north - south)))
                );
        }

        public void Render(double x, double y)
        {
            System.Drawing.Point point = TransForm(new System.Windows.Point(x, y));
            Application.Current.Dispatcher.Invoke(() => SetPixel(point.X, point.Y, writeableBitmap, crossHair));
        }

        public PopulationenRendering(System.Windows.Controls.Image image, Viewbox viewBox, Canvas canvas)
        {
            i = image;
            BitmapImage bitmap = null;
            try
            {
                bitmap = new BitmapImage(new Uri("C:\\Users\\Magnus\\Dropbox\\Kurser\\Programmering med C# III\\C-Sharp-III_Project\\Testing webscraping\\WebScrapeConsoleTest\\WebScrapeConsoleTest\\sweden-map.bmp", UriKind.Absolute));
            }
            catch
            {
                bitmap = new BitmapImage(new Uri("C:\\Users\\msundstr\\Pictures\\sweden-map.bmp", UriKind.Absolute));
            }

            writeableBitmap = new WriteableBitmap(bitmap);
            i.Source = writeableBitmap;
            i.Stretch = Stretch.None;
            i.HorizontalAlignment = HorizontalAlignment.Left;
            i.VerticalAlignment = VerticalAlignment.Top;
            cq = new ConcurrentQueue<System.Windows.Point>();
            this.viewBox = viewBox;
            this.crossHair = new CrossHair(canvas); ;
        }

        Action<int, int, WriteableBitmap, CrossHair> SetPixel = (x, y, wbm, ch) =>
        {
            wbm.Lock();
            var bmp = new System.Drawing.Bitmap(300, 704,
                                     wbm.BackBufferStride,
                                     System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
                                     wbm.BackBuffer);

            bmp.SetPixel(x, y, System.Drawing.Color.Red);
            bmp.Dispose();
            wbm.AddDirtyRect(new Int32Rect(0, 0, 300, 680));
            wbm.Unlock();
            ch.SetPosition(new System.Windows.Point(x, y));

        };
    }

    public class CrossHair
    {
        private Line firstLine;
        private Line secondLine;
        private Ellipse ellipse;
        private Canvas canvas;

        public CrossHair(Canvas canvas)
        {
            this.canvas = canvas;

            firstLine = new Line();
            secondLine = new Line();
            firstLine.Stroke = System.Windows.Media.Brushes.Black;
            secondLine.Stroke = System.Windows.Media.Brushes.Black;

           

            firstLine.HorizontalAlignment = HorizontalAlignment.Left;
            firstLine.VerticalAlignment = VerticalAlignment.Center;
            firstLine.StrokeThickness = 1;
            firstLine.SnapsToDevicePixels = true;

            secondLine.HorizontalAlignment = HorizontalAlignment.Left;
            secondLine.VerticalAlignment = VerticalAlignment.Center;
            secondLine.StrokeThickness = 1;
            secondLine.SnapsToDevicePixels = true;

            secondLine.VerticalAlignment = VerticalAlignment.Center;
            secondLine.StrokeThickness = 1;

           

            canvas.Children.Add(firstLine);
            canvas.Children.Add(secondLine);
   


            //this.canvas = canvas;

            //firstLine = new Line();
            //secondLine = new Line();
            //firstLine.Stroke = System.Windows.Media.Brushes.Black;
            //secondLine.Stroke = System.Windows.Media.Brushes.Black;

            //firstLine.X1 = -5 + 200;
            //firstLine.X2 = 5 + 200;
            //firstLine.Y1 = -5 + 200;
            //firstLine.Y2 = 5 + 200;

            //secondLine.X1 = 5 + 200;
            //secondLine.X2 = -5 + 200;
            //secondLine.Y1 = -5 + 200;
            //secondLine.Y2 = 5 + 200;

            //firstLine.HorizontalAlignment = HorizontalAlignment.Left;
            //firstLine.VerticalAlignment = VerticalAlignment.Center;
            //firstLine.StrokeThickness = 1;
            //firstLine.SnapsToDevicePixels = true;

            //secondLine.HorizontalAlignment = HorizontalAlignment.Left;
            //secondLine.VerticalAlignment = VerticalAlignment.Center;
            //secondLine.StrokeThickness = 1;
            //secondLine.SnapsToDevicePixels = true;

            //secondLine.VerticalAlignment = VerticalAlignment.Center;
            //secondLine.StrokeThickness = 1;

            //ellipse = new Ellipse { Width = 16, Height = 16 };
            //double left = 200 - (16 / 2);
            //double top = 200 - (16 / 2);
            //ellipse.Margin = new Thickness(left, top, 0, 0);
            //ellipse.Stroke = System.Windows.Media.Brushes.Black;
            //ellipse.SnapsToDevicePixels = true;

            //canvas.Children.Add(firstLine);
            //canvas.Children.Add(secondLine);
            //canvas.Children.Add(ellipse);
        }

        public void SetPosition(System.Windows.Point point)
        {
            firstLine.X1 = -10 + point.X;
            firstLine.X2 = 10 + point.X;
            firstLine.Y1 = point.Y;
            firstLine.Y2 = point.Y;

            secondLine.X1 = point.X;
            secondLine.X2 = point.X;
            secondLine.Y1 = -10 + point.Y;
            secondLine.Y2 = 10 + point.Y;


            //firstLine.X1 = -5 + point.X;
            //firstLine.X2 = 5 + point.X;
            //firstLine.Y1 = -5 + point.Y;
            //firstLine.Y2 = 5 + point.Y;

            //secondLine.X1 = 5 + point.X;
            //secondLine.X2 = -5 + point.X;
            //secondLine.Y1 = -5 + point.Y;
            //secondLine.Y2 = 5 + point.Y;

            //double left = point.X - (16 / 2);
            //double top = point.Y - (16 / 2);
            //ellipse.Margin = new Thickness(left, top, 0, 0);
        }
    }
}