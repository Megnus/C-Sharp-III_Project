using System;
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
using System.Windows.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace MapdrawingTest
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //https://msdn.microsoft.com/en-us/library/system.windows.media.imaging.writeablebitmap(VS.85).aspx
        PopulationenRendering populationenRendering;
        private ViewModel view;

        public MainWindow()
        {
            InitializeComponent();
            populationenRendering = new PopulationenRendering(mapImage, vbab, canvas);
            Debug.WriteLine((mainWindow.Width - vbab.Width));
            Debug.WriteLine((mainWindow.Height - vbab.Height));
            view = new ViewModel();
            mylist.ItemsSource = view.Items;
            /* return;
             // Add a Line Element
             //Line myLine = new Line();
             //myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
             //myLine.X1 = 1;
             //myLine.X2 = 50;
             //myLine.Y1 = 1;
             //myLine.Y2 = 250;
             //myLine.HorizontalAlignment = HorizontalAlignment.Left;
             //myLine.VerticalAlignment = VerticalAlignment.Center;
             //myLine.StrokeThickness = 2;
             //myGrid.Children.Add(myLine);

             // Define a StackPanel to host Controls
             StackPanel myStackPanel = new StackPanel();
             myStackPanel.Orientation = Orientation.Vertical;
             myStackPanel.Height = 200;
             myStackPanel.VerticalAlignment = VerticalAlignment.Top;
             myStackPanel.HorizontalAlignment = HorizontalAlignment.Center;

             // Add the Image to the parent StackPanel
             myStackPanel.Children.Add(image);

             // Add the StackPanel as the Content of the Parent Window Object
             mainWindow.Content = myStackPanel;maprender.DrawLinex(100, 100);
             mainWindow.Show();

             // DispatcherTimer setup
             // The DispatcherTimer will be used to update _random every
             //    second with a new random set of colors.
             DispatcherTimer dispatcherTimer = new DispatcherTimer();
             dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
             dispatcherTimer.IsEnabled = true;
             dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
             dispatcherTimer.Start();*/

            List<TodoItem> items = new List<TodoItem>();
            items.Add(new TodoItem() { Title = "Complete this WPF tutorial", Title2 = "tetstststst", Completion = 45 });
            items.Add(new TodoItem() { Title = "Learn C#", Completion = 80 });
            items.Add(new TodoItem() { Title = "Wash the car", Completion = 0 });

            lbTodoList.ItemsSource = items;
            PersonsList.ItemsSource = items;
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
            writeableBitmap.WritePixels(_rect, _colorArray, _stride, 0);

            //Set the Image source.
            image.Source = writeableBitmap;

        }

        private Image image = new Image();
        // Create the writeable bitmap will be used to write and update.
        private static WriteableBitmap writeableBitmap =
            new WriteableBitmap(100, 100, 96, 96, PixelFormats.Bgra32, null);
        // Define the rectangle of the writeable image we will modify. 
        // The size is that of the writeable bitmap.
        private static Int32Rect _rect = new Int32Rect(0, 0, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight);
        // Calculate the number of bytes per pixel. 
        private static int _bytesPerPixel = (writeableBitmap.Format.BitsPerPixel + 7) / 8;
        // Stride is bytes per pixel times the number of pixels.
        // Stride is the byte width of a single rectangle row.
        private static int _stride = writeableBitmap.PixelWidth * _bytesPerPixel;

        // Create a byte array for a the entire size of bitmap.
        private static int _arraySize = _stride * writeableBitmap.PixelHeight;
        private static byte[] _colorArray = new byte[_arraySize];

        Random r = new Random();

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {


            /* this.Dispatcher.Invoke((Action)(() =>
             {
                
             }));*/
            populationenRendering.StartRendering();
            //Task.Factory.StartNew
            new Thread(() =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    double x = r.Next(11113056, 24150556) / 1000000.0;
                    double y = r.Next(55336944, 69060000) / 1000000.0;
                    populationenRendering.AddCoordinate(new Point(x, y));
                    Thread.Sleep(100);
                }
            }).Start();
        }
    }

    public class TodoItem
    {
        public string Title { get; set; }
        public string Title2 { get; set; }
        public int Completion { get; set; }
    }

    public class ViewModel
    {
        public List<Person> Items
        {
            get
            {
                return new List<Person>
            {
                new Person { Name = "P1", Age = 1 },
                new Person { Name = "P2", Age = 2 }
            };
            }
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

}
