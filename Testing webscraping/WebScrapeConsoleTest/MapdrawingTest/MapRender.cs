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

namespace MapdrawingTest
{
    public class MapRender
    {
                 WriteableBitmap writeableBitmap;
         Window w;
         System.Windows.Controls.Image i;
        private static double north = 69.06;
        private static double south = 55.336944;
        private static double east = 24.150556;
        private static double west = 11.113056;

        public void CalcCoord(double x, double y)
        {
            x = x > east ? east : (x < west ? west : x);
            y = y > north ? north : (y < south ? south : y);
            DrawLinex((int)((x - west) * 300 / (east - west)), (int)(703 - (y - south) * 703 / (north - south)));
        }

        public MapRender(System.Windows.Controls.Image image)
        {
            i = image;
            stamethod();
            BitmapImage bitmap = new BitmapImage(new Uri("C:\\Users\\Magnus\\Dropbox\\Kurser\\Programmering med C# III\\C-Sharp-III_Project\\Testing webscraping\\WebScrapeConsoleTest\\WebScrapeConsoleTest\\sweden-map.bmp", UriKind.Absolute));
            writeableBitmap = new WriteableBitmap(bitmap);
            i.Source = writeableBitmap;
            i.Stretch = Stretch.None;
            i.HorizontalAlignment = HorizontalAlignment.Left;
            i.VerticalAlignment = VerticalAlignment.Top;
        }

        [STAThread]
        private void stamethod()
        {
            //BitmapImage bitmap = new BitmapImage(new Uri("C:\\Users\\Magnus\\Dropbox\\Kurser\\Programmering med C# III\\C-Sharp-III_Project\\Testing webscraping\\WebScrapeConsoleTest\\WebScrapeConsoleTest\\sweden-map.bmp", UriKind.Absolute));
            //writeableBitmap = new WriteableBitmap(bitmap);
            //i.Source = writeableBitmap;
            //i.Stretch = Stretch.None;
            //i.HorizontalAlignment = HorizontalAlignment.Left;
            //i.VerticalAlignment = VerticalAlignment.Top;
        }
   
        // The DrawPixel method updates the WriteableBitmap by using
        // unsafe code to write a pixel into the back buffer.
        public void DrawPixel(MouseEventArgs e)
        {
            int column = (int)e.GetPosition(i).X;
            int row = (int)e.GetPosition(i).Y;

            // Reserve the back buffer for updates.
            writeableBitmap.Lock();

            unsafe
            {
                // Get a pointer to the back buffer.
                int pBackBuffer = (int)writeableBitmap.BackBuffer;

                // Find the address of the pixel to draw.
                pBackBuffer += row * writeableBitmap.BackBufferStride;
                pBackBuffer += column * 4;

                // Compute the pixel's color.
                int color_data = 255 << 16; // R
                color_data |= 255 << 8;   // G
                color_data |= 255 << 0;   // B

                // Assign the color data to the pixel.
                *((int*)pBackBuffer) = color_data;
            }

            // Specify the area of the bitmap that changed.
            writeableBitmap.AddDirtyRect(new Int32Rect(column, row, 1, 1));

            writeableBitmap.Unlock();
            DrawLine(column, row);
        }

        public Random r = new Random();

        public void DrawLine(int x, int y)
        {
            writeableBitmap.Lock();
            var bmp = new System.Drawing.Bitmap(300, 704,
                                     writeableBitmap.BackBufferStride,
                                     System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
                                     writeableBitmap.BackBuffer);

             System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);

            // Release the back buffer and make it available for display. 
            // g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Green, 1), new System.Drawing.Point(0, 0), new System.Drawing.Point(x, y));
            // g.FillRectangle(new SolidBrush(System.Drawing.Color.Green), 10, 10, 100, 100);

            g.Dispose();
            bmp.Dispose();
            writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, 300, 704));
            writeableBitmap.Unlock();
            DrawLinex(x, y);
        }
        
        public void DrawLinex(int x, int y)
        {
            writeableBitmap.Lock();
            var bmp = new System.Drawing.Bitmap(300, 704,
                                     writeableBitmap.BackBufferStride,
                                     System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
                                     writeableBitmap.BackBuffer);

            bmp.SetPixel(x, y, System.Drawing.Color.Red);
            bmp.Dispose();
            writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, 300, 704));
            writeableBitmap.Unlock();
            //Debug.WriteLine(x + ", " + y);
        }

        public void ErasePixel(MouseEventArgs e)
        {
            byte[] ColorData = { 0, 0, 0, 0 }; // B G R

            Int32Rect rect = new Int32Rect(
                    (int)(e.GetPosition(i).X),
                    (int)(e.GetPosition(i).Y),
                    1,
                    1);

            writeableBitmap.WritePixels(rect, ColorData, 4, 0);
        }

        public void i_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ErasePixel(e);
        }

        public void i_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DrawPixel(e);
        }

        public void i_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DrawPixel(e);
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                ErasePixel(e);
            }
        }

        public void w_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            System.Windows.Media.Matrix m = i.RenderTransform.Value;

            if (e.Delta > 0)
            {
                m.ScaleAt(
                    1.5,
                    1.5,
                    e.GetPosition(w).X,
                    e.GetPosition(w).Y);
            }
            else
            {
                m.ScaleAt(
                    1.0 / 1.5,
                    1.0 / 1.5,
                    e.GetPosition(w).X,
                    e.GetPosition(w).Y);
            }

            i.RenderTransform = new MatrixTransform(m);
        }
    }
}
