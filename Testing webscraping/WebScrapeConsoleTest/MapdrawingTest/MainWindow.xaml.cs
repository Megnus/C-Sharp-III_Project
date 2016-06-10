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
using System.Collections.Concurrent;

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
        //private List<StaticMapData> searchList;
        private ConcurrentQueue<StaticMapData> searchConcurrentQueue;
        private InformationContext datab;
        private Queue<StaticMapData> searchLQueue = new Queue<StaticMapData>();

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

            Person person = new InformationContext()
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

            using (var context = new InformationContext())
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
                //SearchForName("Anna");
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
            using (var context = new InformationContext())
            {
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
                    postal.Addresses.Add(staticMapData.GetAddress(postal));
                    context.SaveChanges();
                    return;
                }

                Person person = context.Persons.Where(x =>
                    x.DataId.ToString() == staticMapData.Id).FirstOrDefault();

                if (person == null)
                {
                    address.Persons.Add(staticMapData.GetPerson(address));
                    context.SaveChanges();
                }
            }
        }

        private void PrintListBox(StaticMapData staticMapData)
        {
            Dispatcher.Invoke(() =>
            {
                staticMapDataQueue.Enqueue(staticMapData);

                //searchListbox.InvalidateArrange();

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
        }

        /// <summary>
        /// http://blogs.msdn.com/b/adonet/archive/2011/01/31/using-dbcontext-in-ef-feature-ctp5-part-6-loading-related-entities.aspx
        /// </summary>
        private void SearchForName()
        {
            //string name = "";
            //bool flagger = false;
            //flagger = name == searchTextBox.Text.ToLower();
            string name = searchTextBox.Text.ToLower();
            
            Task.Run(() =>
            {
                searchButton.IsEnabled = false;
                //while (true)
                //{

                //    Dispatcher.Invoke(() =>
                //    {

                //    });

                //if (flagger)
                //{
                //    Thread.Sleep(100);
                //    continue;
                //}
                
                List<Person> persons;

                //while (searchConcurrentQueue.Count > 0)
                //{
                //    Thread.Sleep(100);
                //}

                using (var context = new InformationContext())
                {
                    persons = context.Persons.Where(p => p.Name.ToLower().Contains(name))
                        .Take(1000)
                        .DefaultIfEmpty()
                        .Include(p => p.Address)
                        .Include(p => p.Address.Postal)
                        .ToList();
                }
                //searchList.Clear();
                if (persons != null)
                {
                    Dispatcher.Invoke(() =>
                        {
                            searchLQueue.Clear();
                            searchListbox.Items.Refresh();
                        });

                    persons.ForEach(p => {
                        Thread.Sleep(100);
                        
                        Dispatcher.Invoke(() =>
                        {
                            searchLQueue.Enqueue(new StaticMapData(p));
                            searchListbox.Items.Refresh();
                        });
                    });

                   // persons.ForEach(p => searchConcurrentQueue.Enqueue(new StaticMapData(p)));
                    // flag = false;
                }
                
                foreach (StaticMapData s in searchConcurrentQueue)
                {
                    Debug.WriteLine(s.Name);
                }
                Debug.WriteLine("--------------------------");
                //StaticMapData pr = new StaticMapData();

                //bool v = searchConcurrentQueue.TryPeek(out pr);
                //if (persons != null)
                //    searchList.AddRange(persons.Select(p => new StaticMapData(p)).Take(15).ToList());
                //Debug.WriteLine(pr.Name);

                //}
                searchButton.IsEnabled = true;

            });
            //            List overPaidEmployees = Employee.Find(e => e.Salary >= 100000)
            //.OrderBy(e => e.Name)
            //.Take(3)
            //.ToList();

            // searchList.ForEach(x => Debug.WriteLine(x.Name));

            //searchListbox.InvalidateArrange();
            //searchListbox.Items.Refresh();

            searchButton.IsEnabled = true;
            //The ObjectContext instance has been disposed and can no longer be used for operations that require a connection System.InvalidOperationException {System.ObjectDisposedException}
            //http://blogs.msdn.com/b/adonet/archive/2011/01/31/using-dbcontext-in-ef-feature-ctp5-part-6-loading-related-entities.aspx
            // Debug.WriteLine("addresses.FirstOrDefault().Street");
        }

        private void searchTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //SearchForName(searchTextBox.Text);
            //searchListbox.InvalidateArrange();
            //searchListbox.Items.Refresh();
        }

        bool flag = true;

        private void transport()
        {
            StaticMapData sm;

            Task.Run(() =>
            {
                while (true)
                {
                    if (flag)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    flag = true;
                    searchLQueue.Clear();
                    while (!searchConcurrentQueue.IsEmpty && flag)
                    {
                        searchConcurrentQueue.TryDequeue(out sm);
                        Thread.Sleep(10);
                        Dispatcher.Invoke(() =>
                        {
                            searchLQueue.Enqueue(sm);
                            searchListbox.Items.Refresh();
                        });


                    }

                    //while (searchLQueue.Count > 0)
                    //{
                    //    Thread.Sleep(1);
                    //    searchLQueue.Dequeue();
                    //}

                    //Dispatcher.Invoke(() =>
                    //{
                    //    searchListbox.Items.Refresh();
                    //});
                }
            });
        }

        private void mainWindow_Initialized(object sender, EventArgs e)
        {
            staticMapDataQueue = new Queue<StaticMapData>();
            //staticMapDataQueue.Enqueue(new StaticMapData());

            staticMapListbox.ItemsSource = staticMapDataQueue;
            //staticMapListbox.ItemsSource = staticMapDataQueue;
            // staticMapListbox.ItemsSource = new List<StaticMapData>();

            searchLQueue = new Queue<StaticMapData>();
            searchConcurrentQueue = new ConcurrentQueue<StaticMapData>();
            //searchConcurrentQueue.Enqueue(new StaticMapData() { Name = "magnus" });

            searchListbox.ItemsSource = searchLQueue;
            searchListbox.InvalidateArrange();
            searchListbox.Items.Refresh();

    //        transport();
            //SearchForName();
            //Task.Run(() => { 
            //    foreach() {

            //    }
            //});
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            SearchForName();
        }
    }
}
