using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Threading;

namespace MapdrawingTest
{
    public class app : Application
    {
        Window mainWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            CreateAndShowMainWindow();
        }
        private void CreateAndShowMainWindow()
        {
            // Create the application's main window
            mainWindow = new Window();
            mainWindow.Title = "Writeable Bitmap";
            mainWindow.Height = 200;
            mainWindow.Width = 200;

            // Define the Image element
            _random.Stretch = Stretch.None;
            _random.Margin = new Thickness(20);

            // Define a StackPanel to host Controls
            StackPanel myStackPanel = new StackPanel();
            myStackPanel.Orientation = Orientation.Vertical;
            myStackPanel.Height = 200;
            myStackPanel.VerticalAlignment = VerticalAlignment.Top;
            myStackPanel.HorizontalAlignment = HorizontalAlignment.Center;

            // Add the Image to the parent StackPanel
            myStackPanel.Children.Add(_random);

            // Add the StackPanel as the Content of the Parent Window Object
            mainWindow.Content = myStackPanel;
            mainWindow.Show();

            // DispatcherTimer setup
            // The DispatcherTimer will be used to update _random every
            //    second with a new random set of colors.
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.IsEnabled = true;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
        //  System.Windows.Threading.DispatcherTimer.Tick handler
        //
        //  Updates the Image element with new random colors
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //Update the color array with new random colors
            Random value = new Random();
            value.NextBytes(_colorArray);

            //Update writeable bitmap with the colorArray to the image.
            _wb.WritePixels(_rect, _colorArray, _stride, 0);

            //Set the Image source.
            _random.Source = _wb;

        }

        private Image _random = new Image();
        // Create the writeable bitmap will be used to write and update.
        private static WriteableBitmap _wb =
            new WriteableBitmap(100, 100, 96, 96, PixelFormats.Bgra32, null);
        // Define the rectangle of the writeable image we will modify. 
        // The size is that of the writeable bitmap.
        private static Int32Rect _rect = new Int32Rect(0, 0, _wb.PixelWidth, _wb.PixelHeight);
        // Calculate the number of bytes per pixel. 
        private static int _bytesPerPixel = (_wb.Format.BitsPerPixel + 7) / 8;
        // Stride is bytes per pixel times the number of pixels.
        // Stride is the byte width of a single rectangle row.
        private static int _stride = _wb.PixelWidth * _bytesPerPixel;

        // Create a byte array for a the entire size of bitmap.
        private static int _arraySize = _stride * _wb.PixelHeight;
        private static byte[] _colorArray = new byte[_arraySize];

    }

    // Define a static entry class
    //internal static class EntryClass
    //{
    //    [System.STAThread()]
    //    private static void Main()
    //    {
    //        app app = new app();
    //        app.Run();
    //    }
    //}
}
