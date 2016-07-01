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
using System.Windows.Interop;
using System.IO;
using System.Drawing.Imaging;

namespace MapdrawingTest
{
    public class MapRendering
    {
        private WriteableBitmap writeableBitmap;
        System.Windows.Controls.Image img;
        private static double north = 69.06;
        private static double south = 55.336944;
        private static double east = 24.150556;
        private static double west = 11.113056;
        private ConcurrentQueue<System.Windows.Point> cq;
        private bool enableRendering;
        private Viewbox viewBox;
        private System.Drawing.Color pixelColor;

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
                enableRendering = true;
                System.Windows.Point p;
                while (enableRendering)
                {
                    if (cq.Count > 0)
                    {
                        cq.TryDequeue(out p);
                        Render(p.X, p.Y);
                        Thread.Sleep(100);
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
            Application.Current.Dispatcher.Invoke(() => SetPixel(point.X, point.Y, writeableBitmap, crossHair, pixelColor));
        }

        // MapdrawingTest.Properties.Resources.sweden_map
        public void LoadBitmap(Bitmap bitmapResource)
        {
            BitmapImage bitmapImage = null;
            Bitmap bitmap = null;
            // bitmap = new BitmapImage(new Uri("C:\\Users\\Magnus\\Dropbox\\Kurser\\Programmering med C# III\\C-Sharp-III_Project\\Testing webscraping\\WebScrapeConsoleTest\\WebScrapeConsoleTest\\sweden-map.bmp", UriKind.Absolute));
            // bitmap = new BitmapImage(new Uri("C:\\Users\\msundstr\\Pictures\\sweden-map.bmp", UriKind.Absolute));
            bitmap = new Bitmap(bitmapResource);
            bitmapImage = ToBitmapImage(bitmap);
            writeableBitmap = new WriteableBitmap(bitmapImage);
        }

        private BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        public MapRendering(System.Windows.Controls.Image image, Canvas canvas, System.Drawing.Color pixelColor, int width, int height) :
            this(image, canvas, pixelColor, new Bitmap(width, height)) { }

        public MapRendering(System.Windows.Controls.Image image, Canvas canvas, System.Drawing.Color pixelColor, Bitmap bitmap)
        {
            //Bitmap flag = new Bitmap(bitmap.Width, bitmap.Height);
            LoadBitmap(bitmap);
            img = image;
            img.Source = writeableBitmap;
            img.Stretch = Stretch.None;
            img.HorizontalAlignment = HorizontalAlignment.Left;
            img.VerticalAlignment = VerticalAlignment.Top;
            cq = new ConcurrentQueue<System.Windows.Point>();
            this.crossHair = new CrossHair(canvas);
            this.pixelColor = pixelColor;
            //Application.Current.Dispatcher.Invoke(() => { LoadBitmap(bitmap); });
        }

        public void ClearAll()
        {
            int w = writeableBitmap.PixelWidth;
            int h = writeableBitmap.PixelHeight;
            int[] pixelData = new int[w * h];
            int widthInBytes = 4 * w;
            writeableBitmap.CopyPixels(pixelData, widthInBytes, 0);
            Array.Clear(pixelData, 0, w * h);
            //for (int i = 0; i < pixelData.Length; ++i)
            //{
            //    pixelData[i] ^= 0x00ffffff;
            //}
            writeableBitmap.WritePixels(new Int32Rect(0, 0, w, h), pixelData, widthInBytes, 0);
        }

        Action<int, int, WriteableBitmap, CrossHair, System.Drawing.Color> SetPixel = (x, y, wbm, ch, color) =>
        {
            wbm.Lock();
            var bmp = new System.Drawing.Bitmap(300, 704,
                                     wbm.BackBufferStride,
                                     System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
                                     wbm.BackBuffer);
            bmp.SetPixel(x, y, color);
            bmp.Dispose();
            wbm.AddDirtyRect(new Int32Rect(0, 0, 300, 680));
            wbm.Unlock();
            if (ch.IsVisible)
            {
                ch.SetPosition(new System.Windows.Point(x, y));
            }
        };

        public void SetCrossHairPosition(double x, double y)
        {
            System.Drawing.Point point = TransForm(new System.Windows.Point(x, y));
            this.crossHair.SetPosition(new System.Windows.Point(point.X, point.Y));
        }

        public void SetCrossHairVisibility(bool visible)
        {
            this.crossHair.SetVisibility(visible);
        }

        //public void 
    }

    public class CrossHair
    {
        private Line firstLine;
        private Line secondLine;
        private Ellipse ellipse;
        private Canvas canvas;
        private bool isVisible;

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
        }

        public void SetVisibility(bool visible)
        {
            isVisible = visible;
            firstLine.Visibility = visible ? Visibility.Visible : Visibility.Hidden;
            secondLine.Visibility = visible ? Visibility.Visible : Visibility.Hidden;
        }

        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
        }
    }
}