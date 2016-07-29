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

/*
 * Author:          Magnus Sundström
 * Creation Date:   2016-07-22
 * File:            MapRenderingHandler.cs
 */

namespace AddressCollectorApplication
{
    /// <summary>
    /// Class for handling the rendering of the graphical map.
    /// </summary>
    public class MapRenderingHandler
    {
        private static double north = 69.06;
        private static double south = 55.336944;
        private static double east = 24.150556;
        private static double west = 11.113056;
        private System.Windows.Controls.Image img;
        private System.Drawing.Color pixelColor;
        private WriteableBitmap writeableBitmap;
        private ConcurrentQueue<System.Windows.Point> cq;
        private CrossHair crossHair;
        private bool enableRendering;

        /// <summary>
        /// Overloading constructor.
        /// </summary>
        /// <param name="image">The image to be handled.</param>
        /// <param name="canvas">The canvas to be handled.</param>
        /// <param name="pixelColor">The color of the pixel.</param>
        /// <param name="bitmap">The bitmap to be set.</param>
        /// <param name="width">The width of the bitmap.</param>
        /// <param name="height">The height of the bitmap.</param>
        public MapRenderingHandler(System.Windows.Controls.Image image, Canvas canvas, System.Drawing.Color pixelColor, int width, int height) :
            this(image, canvas, pixelColor, new Bitmap(width, height)) { }

        /// <summary>
        /// The constructor of the class which initiates all the field variables.
        /// </summary>
        /// <param name="image">The image to be handled.</param>
        /// <param name="canvas">The canvas to be handled.</param>
        /// <param name="pixelColor">The color of the pixel.</param>
        /// <param name="bitmap">The bitmap to be set.</param>
        public MapRenderingHandler(System.Windows.Controls.Image image, Canvas canvas, System.Drawing.Color pixelColor, Bitmap bitmap)
        {
            LoadBitmap(bitmap);
            img = image;
            img.Source = writeableBitmap;
            img.Stretch = Stretch.None;
            img.HorizontalAlignment = HorizontalAlignment.Left;
            img.VerticalAlignment = VerticalAlignment.Top;
            cq = new ConcurrentQueue<System.Windows.Point>();
            this.crossHair = new CrossHair(canvas);
            this.pixelColor = pixelColor;
        }

        /// <summary>
        /// Adds a coordinate to the que.
        /// </summary>
        /// <param name="p">The point to be added.</param>
        public void AddCoordinate(System.Windows.Point p)
        {
            cq.Enqueue(p);
        }

        /// <summary>
        /// Stops the rendering of the graphicla map.
        /// </summary>
        public void StopRendering()
        {
            enableRendering = false;
        }

        /// <summary>
        /// Method to return the number of points in the que.
        /// </summary>
        /// <returns>The number of points in the que.</returns>
        public int QueueCount()
        {
            return cq.Count;
        }

        /// <summary>
        /// Starts the rendering of the graphical map.
        /// </summary>
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

        /// <summary>
        /// Transposes the point to a pixel coordinate in the map image.
        /// </summary>
        /// <param name="point">The point to transpose.</param>
        /// <returns>The new point that represent a pixel on the image.</returns>
        private System.Drawing.Point TransForm(System.Windows.Point point)
        {
            return new System.Drawing.Point(
                    (int)(point.X > east ? 300 : (point.X < west ? 0 : (point.X - west) * 300 / (east - west))),
                    (int)(point.Y > north ? 0 : (point.Y < south ? 600 : 703 - (point.Y - south) * 703 / (north - south)))
                );
        }

        /// <summary>
        /// Method to set a pixel on the image.
        /// </summary>
        /// <param name="x">The x-coordinate for the pixel.</param>
        /// <param name="y">The y-coordinate for the pixel.</param>
        public void Render(double x, double y)
        {
            System.Drawing.Point point = TransForm(new System.Windows.Point(x, y));
            Application.Current.Dispatcher.Invoke(() => SetPixel(point.X, point.Y, writeableBitmap, crossHair, pixelColor));
        }

        /// <summary>
        /// Method for createing a new bitmap.
        /// </summary>
        /// <param name="bitmapResource">The image in the resource.</param>
        public void LoadBitmap(Bitmap bitmapResource)
        {
            BitmapImage bitmapImage = null;
            Bitmap bitmap = null;
            bitmap = new Bitmap(bitmapResource);
            bitmapImage = ToBitmapImage(bitmap);
            writeableBitmap = new WriteableBitmap(bitmapImage);
        }

        /// <summary>
        /// Method to converte a bitmap to a bitmapimage.
        /// </summary>
        /// <param name="bitmap">The bitmap to convert.</param>
        /// <returns>The bitmap converted to a bitmapimage.</returns>
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

        /// <summary>
        /// Method to clear the bitmap.
        /// </summary>
        public void ClearAll()
        {
            int w = writeableBitmap.PixelWidth;
            int h = writeableBitmap.PixelHeight;
            int[] pixelData = new int[w * h];
            int widthInBytes = 4 * w;
            writeableBitmap.CopyPixels(pixelData, widthInBytes, 0);
            Array.Clear(pixelData, 0, w * h);
            writeableBitmap.WritePixels(new Int32Rect(0, 0, w, h), pixelData, widthInBytes, 0);
        }

        /// <summary>
        /// Action for setting a pixel on the bitmap.
        /// </summary>
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

        /// <summary>
        /// Method for setting the crosshair position.
        /// </summary>
        /// <param name="x">The x-coordinate for the pixel.</param>
        /// <param name="y">The y-coordiante for the pixel.</param>
        public void SetCrossHairPosition(double x, double y)
        {
            System.Drawing.Point point = TransForm(new System.Windows.Point(x, y));
            this.crossHair.SetPosition(new System.Windows.Point(point.X, point.Y));
        }

        /// <summary>
        /// Method for setting the visibility of the crosshair.
        /// </summary>
        /// <param name="visible"></param>
        public void SetCrossHairVisibility(bool visible)
        {
            this.crossHair.SetVisibility(visible);
        }
    }

    /// <summary>
    /// Class for handling the graphical rendering of the crosshair.
    /// </summary>
    public class CrossHair
    {
        private Line firstLine;
        private Line secondLine;
        private Ellipse ellipse;
        private Canvas canvas;
        private bool isVisible;

        /// <summary>
        /// The constructor of the class which initiates all the field variables.
        /// </summary>
        /// <param name="canvas">The canvas to be handled.</param>
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

        /// <summary>
        /// Method for setting the position of the crosshair.
        /// </summary>
        /// <param name="point">The coordinate for the crosshair.</param>
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

        /// <summary>
        /// Sets the visibility of the crosshair.
        /// </summary>
        /// <param name="visible"></param>
        public void SetVisibility(bool visible)
        {
            isVisible = visible;
            firstLine.Visibility = visible ? Visibility.Visible : Visibility.Hidden;
            secondLine.Visibility = visible ? Visibility.Visible : Visibility.Hidden;
        }

        /// <summary>
        /// Read-only property for the visibilty of the crosshair.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
        }
    }
}