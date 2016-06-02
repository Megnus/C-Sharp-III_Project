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
using MapdrawingTest.Web;
using MapdrawingTest.Data;
using System.Text.RegularExpressions;
using System.Globalization;

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MapdrawingTest
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //https://msdn.microsoft.com/en-us/library/system.windows.media.imaging.writeablebitmap(VS.85).aspx
        PopulationenRendering populationenRendering;
        // private List<Data.Person> listboxData;
        //private List<StaticMapData> staticMapDataList;
        private Queue<StaticMapData> staticMapDataQueue;
        private PersonContext datab;

        public MainWindow()
        {
            InitializeComponent();

            //            Debug.WriteLine(StaticMapData.ConvertToDouble("12.9991601155327").ToString());
            //12.9991601155327  
            //57.7310176461699
            //datab = new PersonContext();

            populationenRendering = new PopulationenRendering(mapImage, vbab, canvas);
            //Debug.WriteLine((mainWindow.Width - vbab.Width));
            //Debug.WriteLine((mainWindow.Height - vbab.Height));

            /* return;

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
            //listboxData = new List<Data.Person>();
            //persons.Add(new Data.Person() { PersonId = 1, Id = 2, Name = "Magnus", Phone = "070-3945876", BirthDate = "18 maj", Address = new Data.Address() { Street = "FUCK", XCoord = 66.234, Postal = new Data.Postal { City = "York", PostalCode = "123456" } } });
            //persons.Add(persons.First());

            //PersonsList.ItemsSource = persons;
            //staticMapDataList = new List<StaticMapData>();
            staticMapDataQueue = new Queue<StaticMapData>();
            staticMapListbox.ItemsSource = staticMapDataQueue;

            //Task.Factory.StartNew(() => WebScraping());
            //Task.Run(() => WebScraping());
        }

        private void WebScraping()
        {
            PersonListInformationHandler personListInformationHandler = new PersonListInformationHandler();
            StaticMapDataHandler siteInformationHandler = new StaticMapDataHandler();
            //int maxval = new PersonContext()
            //    .UrlDataRecords.Max(u => u.UrlIndex);
            //int maxvali = new PersonContext()
            //    .UrlDataRecords.Max(u => u.UrlIndex);

            Person person = new PersonContext()
                .Persons.OrderByDescending(u => u.UrlIndex)
                .ThenByDescending(u => u.PageNumber)
                .FirstOrDefault();

            if (person == null)
            {
                person = new Person() { UrlIndex = 0, PageNumber = 1 };
            }

            personListInformationHandler.PostalNumber = person.UrlIndex;
            //new PersonContext()
            //.UrlDataRecords.Select(u => u.UrlIndex).DefaultIfEmpty(0).Max();
            personListInformationHandler.PageNumber = person.PageNumber;

            using (var context = new PersonContext())
            {
                foreach (Address a in context.Addresses)
                {
                    populationenRendering.AddCoordinate(new Point(a.XCoord, a.YCoord));
                }
            }

            while (Dispatcher.Thread.IsAlive)
            {
                List<string> personList = personListInformationHandler.GetNextList(USER_LIST_REGEX);
                personList.ForEach(id => HandleUserInformation(
                    siteInformationHandler,
                    personListInformationHandler.PostalNumber,
                    personListInformationHandler.PageNumber,
                    int.Parse(id)));
               // var dd = SearchForName();//.ForEach(x => Debug.WriteLine(x.Name));
                //Debug.WriteLine("------------------------------");
                SearchForName();
            }
        }

        private const string USER_LIST_REGEX = "(?<=data-id=\")\\d+?(?=\">\n)";
        private const string STATIC_MAP_REGEX = "(?<=var\\sstaticMapData\\s=\\s\\[).+?(?=\\];)";
        private const string BIRTHDAY_REGEX = "(?<=födelsedag\\n<\\/h2>\n<p>\n).+?(?=<br\\/>)";
        private const string TELEPHONE_REGEX = "(?<=\"telephone\">).+?(?=</li>)";
        private const string LINEFEED_REGEX = "\n";

        private void HandleUserInformation(StaticMapDataHandler infoHandler, int urlIndex, int pageNumber, int dataId)
        {
            double x, y;
            NumberStyles style = NumberStyles.AllowDecimalPoint;
            CultureInfo culture = CultureInfo.InvariantCulture;

            if (infoHandler.SetDataId(dataId))
            {
                StaticMapData staticMapData = infoHandler.GetSerializedData(STATIC_MAP_REGEX);
                staticMapData.Birthday = infoHandler.GetStringData(BIRTHDAY_REGEX);
                staticMapData.Phone = infoHandler.GetStringData(TELEPHONE_REGEX).Replace(LINEFEED_REGEX, String.Empty);
                staticMapData.CoordX = staticMapData.CoordX.Replace(',', '.');
                staticMapData.CoordY = staticMapData.CoordY.Replace(',', '.');

                if (Double.TryParse(staticMapData.CoordX, style, culture, out x)
                    && Double.TryParse(staticMapData.CoordY, style, culture, out y))
                {
                    populationenRendering.AddCoordinate(new Point(x, y));

                }

                staticMapData.UrlIndex = urlIndex;
                staticMapData.PageNumber = pageNumber;
                PrintListBox(staticMapData);
                UpdateDatabase(staticMapData);
            }
        }

        //private void UpdateDataIdentity(int urlIndex, int dataId)
        //{
        //    using (var context = new PersonContext())
        //    {
        //        DataIdentity dataIdentity = context.DataIdentities.Where(x => 
        //            x.UrlIndex == urlIndex).FirstOrDefault();

        //        if (dataIdentity == null)
        //        {
        //            dataIdentity = new DataIdentity() { 
        //                UrlIndex = urlIndex, DataId = new List<int>() { dataId } };
        //        }

        //        context.DataIdentities.Add(dataIdentity);
        //        context.SaveChanges();
        //    }
        //}

        private static void UpdateDatabase(StaticMapData staticMapData)
        {
            using (var context = new PersonContext())
            {
                //UrlData urlData = context.UrlDataRecords.Where(u => u.UrlIndex == staticMapData.UrlIndex 
                //    && u.PageNumber == staticMapData.PageNumber).FirstOrDefault();

                //staticMapData.SetUrlData(urlData);


                Postal postal = context.Postals.Where(x =>
                    x.PostalCode == staticMapData.PostalCode &&
                    x.City == staticMapData.City).FirstOrDefault();

                if (postal == null)
                {
                    context.Postals.Add(staticMapData.GetPostal());
                    context.SaveChanges();
                    return;
                }

                Address address = context.Addresses.Where(x =>
                        x.Street == staticMapData.Addr1
                        && x.PostalCode == staticMapData.PostalCode).FirstOrDefault();

                if (address == null)
                {
                    postal.Addresses.Add(staticMapData.GetAddress(postal.PostalId, postal));
                    context.SaveChanges();
                    return;
                }

                Person person = context.Persons.Where(x =>
                    x.DataId.ToString() == staticMapData.Id).FirstOrDefault();

                if (person == null)
                {
                    address.Persons.Add(staticMapData.GetPerson(address.AddressId, address));
                    context.SaveChanges();
                }


            }
        }

        private void PrintListBox(StaticMapData staticMapData)
        {
            Dispatcher.Invoke(() =>
            {
                staticMapDataQueue.Enqueue(staticMapData);
                staticMapListbox.InvalidateArrange();

                if (staticMapDataQueue.Count > 10)
                {
                    staticMapDataQueue.Dequeue();
                }

                staticMapListbox.Items.Refresh();
            });
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
            new Thread(() => WebScraping()).Start();
            populationenRendering.StartRendering();
            /* this.Dispatcher.Invoke((Action)(() =>
             {
                
             }));*/
            //    populationenRendering.StartRendering();
            //    //Task.Factory.StartNew
            //    new Thread(() =>
            //    {
            //        for (int i = 0; i < 100000; i++)
            //        {
            //            double x = r.Next(11113056, 24150556) / 1000000.0;
            //            double y = r.Next(55336944, 69060000) / 1000000.0;
            //            populationenRendering.AddCoordinate(new Point(x, y));
            //            Thread.Sleep(100);
            //        }
            //    }).Start();
        }


        private void SearchForName()
        {
            //List<StaticMapData>
            string value = "Anna";
            value = value.ToLower();
            Match match = Regex.Match(value, ".+" + value + ".+");
            Regex regex = new Regex(".+Anna.+");
            StaticMapData s;
            List<Person> persons;
            List<Address> addresses;
            using (var context = new PersonContext())
            {
                persons = context.Persons.Where(x => x.Name.Contains("Anna")).ToList<Person>();//regex.IsMatch(x.Name));
                //addresses = context.Addresses.Where(a => a.AddressId == Xxxx.First().AddressId).ToList();
            }
            List<int> myint = persons.Select(m => m.AddressId).ToList();
            using (var context = new PersonContext())
            {
                //var Xxxx = context.Persons.Where(x => x.Name.Contains("Anna")).ToList<Person>();//regex.IsMatch(x.Name));
                addresses = context.Addresses.Where(a => myint.Any(x => x == a.AddressId)).ToList<Address>();
            }
            //entity framework threw an exception of type 'System.ObjectDisposedException'
            //The ObjectContext instance has been disposed and can no longer be used for operations that require a connection System.InvalidOperationException {System.ObjectDisposedException}
            //http://blogs.msdn.com/b/adonet/archive/2011/01/31/using-dbcontext-in-ef-feature-ctp5-part-6-loading-related-entities.aspx
            Debug.WriteLine("addresses.FirstOrDefault().Street");

        }

        //private async Task<Person> SearchForNamex()
        //{
        //    //List<StaticMapData>
        //    string value = "Anna";
        //    value = value.ToLower();
        //    Match match = Regex.Match(value, ".+" + value + ".+");
        //    Regex regex = new Regex(".+Anna.+");
        //    StaticMapData s;
        //    List<Person> persons;
        //    List<Address> addresses;
        //    using (var context = new PersonContext())
        //    {
        //       var Xxxx = await (context.Persons.Where(x => x.Name.Contains("Anna")).ToListAsync<Person>());//regex.IsMatch(x.Name));
        //       //addresses = context.Addresses.Where(a => a.AddressId == Xxxx.First().AddressId).ToList();
        //    }

        //    Debug.WriteLine("addresses.FirstOrDefault().Street");

        //    return null;
        //}
    }
}
