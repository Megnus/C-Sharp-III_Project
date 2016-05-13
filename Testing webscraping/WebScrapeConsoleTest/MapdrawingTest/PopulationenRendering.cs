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
                    cq.TryDequeue(out p);
                    CalcCoord(p.X, p.Y);
                    Thread.Sleep(500);
                }
            }).Start();
            Debug.WriteLine("Rendering stoped");
        }

        public void CalcCoord(double x, double y)
        {
            int xx = (int)(x > east ? 300 : (x < west ? 0 : (x - west) * 300 / (east - west)));
            int yy = (int)(y > north ? 0 : (y < south ? 600 : 703 - (y - south) * 703 / (north - south)));
            Application.Current.Dispatcher.Invoke(() => SetPixel(xx, yy, writeableBitmap, viewBox));
        }

        public PopulationenRendering(System.Windows.Controls.Image image, Viewbox viewBox, Canvas canvas)
        {
            i = image;
            //BitmapImage bitmap = new BitmapImage(new Uri("C:\\Users\\Magnus\\Dropbox\\Kurser\\Programmering med C# III\\C-Sharp-III_Project\\Testing webscraping\\WebScrapeConsoleTest\\WebScrapeConsoleTest\\sweden-map.bmp", UriKind.Absolute));
            BitmapImage bitmap = new BitmapImage(new Uri("C:\\Users\\msundstr\\Pictures\\sweden-map.bmp", UriKind.Absolute));
            writeableBitmap = new WriteableBitmap(bitmap);
            i.Source = writeableBitmap;
            i.Stretch = Stretch.None;
            i.HorizontalAlignment = HorizontalAlignment.Left;
            i.VerticalAlignment = VerticalAlignment.Top;
            cq = new ConcurrentQueue<System.Windows.Point>();
            this.viewBox = viewBox;

            Application.Current.Dispatcher.Invoke(() => {}
            // Add a Line Element
            Line firstLine = new Line();
            firstLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            firstLine.X1 = 1;
            firstLine.X2 = 50;
            firstLine.Y1 = 1;
            firstLine.Y2 = 50;
            firstLine.HorizontalAlignment = HorizontalAlignment.Left;
            firstLine.VerticalAlignment = VerticalAlignment.Center;
            firstLine.StrokeThickness = 1;

            canvas.Children.Add(firstLine); });
        }

        Action<int, int, WriteableBitmap, Viewbox> SetPixel = (x, y, wbm, vb) =>
        {
            wbm.Lock();
            var bmp = new System.Drawing.Bitmap(300, 704,
                                     wbm.BackBufferStride,
                                     System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
                                     wbm.BackBuffer);

            bmp.SetPixel(x, y, System.Drawing.Color.Red);
            bmp.Dispose();
            //writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, 300, 704));
            wbm.AddDirtyRect(new Int32Rect(0, 0, 300, 680));
            wbm.Unlock();
            vb.Margin = new Thickness(x - 5, y - 5, 291 - x + 5, 700 - y + 5);

        };
    }
}