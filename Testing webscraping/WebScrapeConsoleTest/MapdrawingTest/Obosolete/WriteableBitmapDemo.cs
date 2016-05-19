using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MapdrawingTest.Obosolete
{
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/system.windows.media.imaging.writeablebitmap(v=vs.90).aspx
    /// http://www.i-programmer.info/programming/wpf-workings/527-writeablebitmap.html?start=1
    /// https://msdn.microsoft.com/en-us/library/system.drawing.bitmap(v=vs.110).aspx
    /// http://stackoverflow.com/questions/88488/getting-a-drawingcontext-for-a-wpf-writeablebitmap
    /// https://msdn.microsoft.com/en-us/library/system.drawing.pen(v=vs.110).aspx
    /// 
    /// http://stackoverflow.com/questions/1302741/how-do-i-draw-simple-graphics-in-c
    /// http://www.c-sharpcorner.com/article/gdi-tutorial-for-beginners/
    /// </summary>
    public class WriteableBitmapDemo
    {
        static WriteableBitmap writeableBitmap;
        static Window w;
        static System.Windows.Controls.Image i;

        [STAThread]
        public static void doit()
        {
            i = new System.Windows.Controls.Image();
            //i = new Bitmap(@"C:\Users\Magnus\Dropbox\Kurser\Programmering med C# III\C-Sharp-III_Project\Testing webscraping\WebScrapeConsoleTest\WebScrapeConsoleTest\sweden-map.png", true);
            // ImageSource imageSource = new BitmapImage(new Uri("C:\\Users\\Magnus\\Dropbox\\Kurser\\Programmering med C# III\\C-Sharp-III_Project\\Testing webscraping\\WebScrapeConsoleTest\\WebScrapeConsoleTest\\sweden-map.png"));
            // i.Source = imageSource;

            //RenderOptions.SetBitmapScalingMode(i, BitmapScalingMode.NearestNeighbor);
            //RenderOptions.SetEdgeMode(i, EdgeMode.Aliased);

            w = new Window();
            w.Content = i;
            w.Show();

            /* writeableBitmap = new WriteableBitmap(
                 (int)w.ActualWidth,
                 (int)w.ActualHeight,
                 96,
                 96,
                 PixelFormats.Bgr32,
                 null);*/

            BitmapImage bitmap = new BitmapImage(new Uri("C:\\Users\\Magnus\\Dropbox\\Kurser\\Programmering med C# III\\C-Sharp-III_Project\\Testing webscraping\\WebScrapeConsoleTest\\WebScrapeConsoleTest\\sweden-map.bmp", UriKind.Absolute));
            writeableBitmap = new WriteableBitmap(bitmap);

            i.Source = writeableBitmap;
            //i.Stretch = Stretch.None;
            //i.HorizontalAlignment = HorizontalAlignment.Left;
            //i.VerticalAlignment = VerticalAlignment.Top;

            //i.MouseMove += new MouseEventHandler(i_MouseMove);
            //i.MouseLeftButtonDown +=
            //    new MouseButtonEventHandler(i_MouseLeftButtonDown);
            //i.MouseRightButtonDown +=
            //    new MouseButtonEventHandler(i_MouseRightButtonDown);

            //w.MouseWheel += new MouseWheelEventHandler(w_MouseWheel);

            Application app = new Application();
            app.Run();
        }

        // The DrawPixel method updates the WriteableBitmap by using
        // unsafe code to write a pixel into the back buffer.
        //static void DrawPixel(MouseEventArgs e)
        //{
        //    int column = (int)e.GetPosition(i).X;
        //    int row = (int)e.GetPosition(i).Y;

        //    // Reserve the back buffer for updates.
        //    writeableBitmap.Lock();

        //    unsafe
        //    {
        //        // Get a pointer to the back buffer.
        //        int pBackBuffer = (int)writeableBitmap.BackBuffer;

        //        // Find the address of the pixel to draw.
        //        pBackBuffer += row * writeableBitmap.BackBufferStride;
        //        pBackBuffer += column * 4;

        //        // Compute the pixel's color.
        //        int color_data = 255 << 16; // R
        //        color_data |= 255 << 8;   // G
        //        color_data |= 255 << 0;   // B

        //        // Assign the color data to the pixel.
        //        *((int*)pBackBuffer) = color_data;
        //    }

        //    // Specify the area of the bitmap that changed.
        //    writeableBitmap.AddDirtyRect(new Int32Rect(column, row, 1, 1));

        //    writeableBitmap.Unlock();
        //    DrawLine(column, row);
        //}

        //public static Random r = new Random();

        //public static void DrawLine(int x, int y)
        //{
        //    writeableBitmap.Lock();
        //    var bmp = new System.Drawing.Bitmap(300, 704,
        //                             writeableBitmap.BackBufferStride,
        //                             System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
        //                             writeableBitmap.BackBuffer);

        //    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);

        //    // Release the back buffer and make it available for display. 
        //    g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Green, 1), new System.Drawing.Point(0, 0), new System.Drawing.Point(x, y));
        //    g.FillRectangle(new SolidBrush(System.Drawing.Color.Green), 10, 10, 100, 100);

        //    g.Dispose();
        //    bmp.Dispose();
        //    writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, 300, 704));
        //    writeableBitmap.Unlock();
        //    DrawLinex(x, y);
        //}

        //public static void DrawLinex(int x, int y)
        //{
        //    var bmp = new System.Drawing.Bitmap(300, 704,
        //                             writeableBitmap.BackBufferStride,
        //                             System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
        //                             writeableBitmap.BackBuffer);

        //    bmp.SetPixel(x, y, System.Drawing.Color.Red);
        //    bmp.Dispose();
        //}

        //static void ErasePixel(MouseEventArgs e)
        //{
        //    byte[] ColorData = { 0, 0, 0, 0 }; // B G R

        //    Int32Rect rect = new Int32Rect(
        //            (int)(e.GetPosition(i).X),
        //            (int)(e.GetPosition(i).Y),
        //            1,
        //            1);

        //    writeableBitmap.WritePixels(rect, ColorData, 4, 0);
        //}

        //static void i_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    ErasePixel(e);
        //}

        //static void i_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    DrawPixel(e);
        //}

        //static void i_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        DrawPixel(e);
        //    }
        //    else if (e.RightButton == MouseButtonState.Pressed)
        //    {
        //        ErasePixel(e);
        //    }
        //}

        //static void w_MouseWheel(object sender, MouseWheelEventArgs e)
        //{
        //    System.Windows.Media.Matrix m = i.RenderTransform.Value;

        //    if (e.Delta > 0)
        //    {
        //        m.ScaleAt(
        //            1.5,
        //            1.5,
        //            e.GetPosition(w).X,
        //            e.GetPosition(w).Y);
        //    }
        //    else
        //    {
        //        m.ScaleAt(
        //            1.0 / 1.5,
        //            1.0 / 1.5,
        //            e.GetPosition(w).X,
        //            e.GetPosition(w).Y);
        //    }

        //    i.RenderTransform = new MatrixTransform(m);
        //}
    }
}
