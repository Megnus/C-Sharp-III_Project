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
//using System.Drawing;

namespace MapdrawingTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string USER_LIST_REGEX = "(?<=data-id=\")\\d+?(?=\">\n)";
        private const string STATIC_MAP_REGEX = "(?<=var\\sstaticMapData\\s=\\s\\[).+?(?=\\];)";
        private const string BIRTHDAY_REGEX = "(?<=födelsedag\\n<\\/h2>\n<p>\n).+?(?=<br\\/>)";
        private const string TELEPHONE_REGEX = "(?<=\"telephone\">).+?(?=</li>)";
        private const string LINEFEED_REGEX = "\n";
        //https://msdn.microsoft.com/en-us/library/system.windows.media.imaging.writeablebitmap(VS.85).aspx
        PopulationenRendering populationenRendering;
        PopulationenRendering populationenRendering_Copy;
        private Queue<StaticMapData> staticMapDataQueue;
        private ConcurrentQueue<StaticMapData> searchConcurrentQueue;
        private InformationContext datab;
        private Queue<StaticMapData> searchLQueue = new Queue<StaticMapData>();
        private PersonListInformationHandler personListInformationHandler;
        private StaticMapDataHandler siteInformationHandler;
        private bool runWebScraping = true;
        private TabSelected tabSelected;

        enum TabSelected
        {
            Webscraping,
            Search
        }

        public MainWindow()
        {
            InitializeComponent();
            System.Drawing.Bitmap bitmap = MapdrawingTest.Properties.Resources.sweden_map;
            populationenRendering = new PopulationenRendering(mapImage, canvas, System.Drawing.Color.Red, bitmap);
            populationenRendering_Copy = new PopulationenRendering(mapImage_Copy, canvas_Copy, System.Drawing.Color.Blue, bitmap.Width, bitmap.Height);

            this.personListInformationHandler = new PersonListInformationHandler();
            this.siteInformationHandler = new StaticMapDataHandler();
            this.tabSelected = TabSelected.Webscraping;
        }

        public void WebScraping()
        {
            runWebScraping = true;

            Person person = new InformationContext()
                .Persons.OrderByDescending(u => u.UrlIndex)
                .ThenByDescending(u => u.PageNumber)
                .FirstOrDefault();

            if (person == null)
            {
                person = new Person() { UrlIndex = 0, PageNumber = 1 };
            }

            personListInformationHandler.PostalNumber = person.UrlIndex;
            personListInformationHandler.PageNumber = person.PageNumber;

            using (var context = new InformationContext())
            {
                context.Addresses.ToList().ForEach(a => populationenRendering.AddCoordinate(new Point(a.XCoord, a.YCoord)));
            }

            while (runWebScraping && Dispatcher.Thread.IsAlive)
            {
                List<string> personList = personListInformationHandler.GetNextList(USER_LIST_REGEX);
                personList.ForEach(id => HandleUserInformation(
                    siteInformationHandler,
                    personListInformationHandler.PostalNumber,
                    personListInformationHandler.PageNumber,
                    int.Parse(id)));
            }
        }

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

                if (staticMapDataQueue.Count > 30)
                {
                    staticMapDataQueue.Dequeue();
                }

                staticMapListbox.Items.Refresh();
            });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(() => WebScraping());
            thread.IsBackground = true;
            thread.Start();
            populationenRendering.StartRendering();
            populationenRendering.SetCrossHairVisibility(true);
        }

        /// <summary>
        /// http://blogs.msdn.com/b/adonet/archive/2011/01/31/using-dbcontext-in-ef-feature-ctp5-part-6-loading-related-entities.aspx
        /// </summary>
        private void SearchForName()
        {
            populationenRendering_Copy.StartRendering();

            Action<bool> SearchEnable = enable =>
            {
                Dispatcher.Invoke(() =>
                {
                    searchButton.IsEnabled = enable;
                });
            };

            Action<Action> dispatcherAction = action =>
            {
                Dispatcher.Invoke(() =>
                {
                    action();
                    searchListbox.Items.Refresh();
                });
            };

            StaticMapData staticMapData = new StaticMapData()
            {
                Name = searchTextBox.Text.ToLower(),
                Phone = "",
                Addr1 = "",
                PostalCode = "",
                City = ""
            };

            Task.Run(() =>
            {
                List<Person> persons;
                SearchEnable(false);

                using (var context = new InformationContext())
                {
                    persons = context.Persons.Where(p =>
                            p.Name.ToLower().Contains(staticMapData.Name)
                            && p.Phone.Contains(staticMapData.Phone)
                            && p.Address.Street.Contains(staticMapData.Addr1)
                            && p.Address.Postal.PostalCode.Contains(staticMapData.PostalCode)
                            && p.Address.Postal.City.Contains(staticMapData.City))
                        .Take(100)
                        .DefaultIfEmpty()
                        .Include(p => p.Address)
                        .Include(p => p.Address.Postal)
                        .ToList();
                }
                //PopulationenRendering
                //persons.ForEach(p => populationenRendering_Copy.AddCoordinate(new Point(p.Address.XCoord, p.Address.YCoord)));
                dispatcherAction(delegate() { persons.ForEach(p => populationenRendering_Copy.AddCoordinate(new Point(p.Address.XCoord, p.Address.YCoord))); });

                if (persons != null)
                {
                    dispatcherAction(delegate() { searchLQueue.Clear(); });

                    persons.ForEach(p =>
                        {
                            Thread.Sleep(100);
                            dispatcherAction(delegate() { searchLQueue.Enqueue(new StaticMapData(p)); });
                            populationenRendering.AddCoordinate(new Point(p.Address.XCoord, p.Address.YCoord));
                        });
                }

                SearchEnable(true);
            });
            //The ObjectContext instance has been disposed and can no longer be used for operations that require a connection System.InvalidOperationException {System.ObjectDisposedException}
            //http://blogs.msdn.com/b/adonet/archive/2011/01/31/using-dbcontext-in-ef-feature-ctp5-part-6-loading-related-entities.aspx
            // Debug.WriteLine("addresses.FirstOrDefault().Street");
        }

        bool flag = true;

        private void transport()
        {
            StaticMapData sm;

            Task.Run(() =>
            {
                while (Dispatcher.Thread.IsAlive)
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
                }
            });
        }

        private void mainWindow_Initialized(object sender, EventArgs e)
        {
            staticMapDataQueue = new Queue<StaticMapData>();
            staticMapListbox.ItemsSource = staticMapDataQueue;
            searchLQueue = new Queue<StaticMapData>();
            searchConcurrentQueue = new ConcurrentQueue<StaticMapData>();
            searchListbox.ItemsSource = searchLQueue;
            searchListbox.InvalidateArrange();
            searchListbox.Items.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            populationenRendering_Copy.ClearAll();
            SearchForName();
        }

        private void TabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            //System.Windows.MessageBox.Show(sender.ToString());
            if (sender.Equals(searchTab))
            {
                tabSelected = TabSelected.Search;
                populationenRendering.SetCrossHairVisibility(false);
                populationenRendering_Copy.SetCrossHairVisibility(false);
            }
            else if (sender.Equals(webscrapingTab))
            {
                tabSelected = TabSelected.Webscraping;
                populationenRendering.SetCrossHairVisibility(true);
                populationenRendering_Copy.SetCrossHairVisibility(false);
            }
        }

        private void searchListbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (searchListbox.Items.Count > 0)
            {
                populationenRendering_Copy.SetCrossHairVisibility(true);
                StaticMapData s = (StaticMapData)searchListbox.SelectedItem;
                if (s != null)
                {
                    double x = double.Parse(s.CoordX);
                    double y = double.Parse(s.CoordY);
                    populationenRendering_Copy.SetCrossHairPosition(x, y);
                }
            }
        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            populationenRendering.StopRendering();
            populationenRendering_Copy.StopRendering();
            Environment.Exit(0);
        }

        private void MenuItem_Checked(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(() => WebScraping());
            thread.IsBackground = true;
            thread.Start();
            populationenRendering.SetCrossHairVisibility(tabSelected.Equals(TabSelected.Webscraping));
            populationenRendering.StartRendering();
        }

        private void MenuItem_Unchecked(object sender, RoutedEventArgs e)
        {
            runWebScraping = false;
            populationenRendering.SetCrossHairVisibility(true);
            populationenRendering.StopRendering();
        }
    }
}