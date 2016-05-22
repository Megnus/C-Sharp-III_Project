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
        private List<Data.Person> persons;
        private List<StaticMapData> staticMapDataList;
        private PersonContext datab;

        public MainWindow()
        {
            InitializeComponent();

            //            Debug.WriteLine(StaticMapData.ConvertToDouble("12.9991601155327").ToString());
            //12.9991601155327  
            //57.7310176461699
            datab = new PersonContext();

            populationenRendering = new PopulationenRendering(mapImage, vbab, canvas);
            //Debug.WriteLine((mainWindow.Width - vbab.Width));
            //Debug.WriteLine((mainWindow.Height - vbab.Height));
            view = new ViewModel();
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
            persons = new List<Data.Person>();
            staticMapDataList = new List<StaticMapData>();
            //persons.Add(new Data.Person() { PersonId = 1, Id = 2, Name = "Magnus", Phone = "070-3945876", BirthDate = "18 maj", Address = new Data.Address() { Street = "FUCK", XCoord = 66.234, Postal = new Data.Postal { City = "York", PostalCode = "123456" } } });
            //persons.Add(persons.First());
            //persons.Add(persons.First());
            //persons.Add(persons.First());
            //persons.Add(persons.First());
            //persons.Add(persons.First());
            //persons.Add(persons.First());

            //PersonsList.ItemsSource = persons;
            PersonsList.ItemsSource = staticMapDataList;

            List<TodoItem> items = new List<TodoItem>();
            items.Add(new TodoItem() { Title = "Complete this WPF tutorial", Title2 = "tetstststst", Completion = 45 });
            items.Add(new TodoItem() { Title = "Learn C#", Completion = 80 });
            items.Add(new TodoItem() { Title = "Wash the car", Completion = 0 });

            //Task.Factory.StartNew(() => WebScraping());
            //Task.Run(() => WebScraping());

        }

        private void WebScraping()
        {
            SiteInformationHandler<StaticMapData> siteInformationHandler =
                new SiteInformationHandler<StaticMapData>("http://www.torget.se/personer/Stockholm/TA_{0}/", "staticMapData");

            siteInformationHandler.SetIndex(79779);
            int x = 0;
            while (x++ < 1000)
            {
                if (!siteInformationHandler.GetNextIndex())
                {
                    continue;
                }

                StaticMapData staticMapData =
                    siteInformationHandler.GetSerializedData("(?<=var\\sstaticMapData\\s=\\s\\[).+?(?=\\];)", RegexOptions.Singleline);

                staticMapData.Birthday =
                    siteInformationHandler.GetStringData("(?<=födelsedag\\n<\\/h2>\n<p>\n).+?(?=<br\\/>)", RegexOptions.Singleline);
                Thread.Sleep(1000);
                //Person person = staticMapData.GetPerson();
                //Postal postal = staticMapData.GetPostal();
                //Postal result;
                //using (var db = new PersonContext())
                //{
                //var result = datab.Postals.SingleOrDefault(p => p.PostalCode == postal.PostalCode);
                //}
                Postal postal = new Postal()
                {
                    City = "132",
                    PostalCode = "1212",
                    Addresses = new List<Address>() {
                        new Address()
                        {
                            AddressId = 1212,
                            Street = "asd",
                            XCoord = 1.1,
                            YCoord = 1.2,
                            Persons = new List<Person>() { 
                                    new Person() {
                                        PersonId = 12121,
                                                    Ixd = 121,
                                                    Name = "asdasd",
                                    Phone = "asdasdasd",
                                    BirthDate = "asdasd"
                                } }

                        }
                }
                };



                //if (result != null)
                //{
                //    Address address = new Address()
                //    {
                //        AddressId = 1212,
                //        Street = "asd",
                //        XCoord = 1.1,
                //        YCoord = 1.2,
                //        Persons = new List<Person>() { 
                //                new Person() {
                //                    PersonId = 12121,
                //                                Ixd = 121,
                //                                Name = "asdasd",
                //                Phone = "asdasdasd",
                //                BirthDate = "asdasd"
                //            } }
                //    };
                //    result.Addresses.Add(address);
                //}

                //using (var db = new PersonContext())
                {
                    // if (db.Postals.Any(p => p.PostalCode == person.Address.Postal.PostalCode))
                    {
                        // person.Address.Postal = db.Postals.Find(person.Address.Postal.PostalCode);
                        //person.Address.Postal = db.Postals.Where(p => p.PostalCode == person.Address.Postal.PostalCode).FirstOrDefault();
                        //    System.Windows.Forms.MessageBox.Show("aaaaaaaaaaaaaaaaggg");
                        //  db.Postals.Remove(db.Postals.Where(p => p.PostalCode == person.Address.Postal.PostalCode).FirstOrDefault());
                        //  person.Address.Postal.PostalCode = person.Address.Postal.PostalCode  + 1;

                        //    db.SaveChanges();
                    }

                    //foreach (Person p in db.Persons)
                    //{
                    //    Debug.WriteLine(p.Name);
                    //}
                    //db.Persons.Add(person);

                    //if (db.Postals.Any(p => p.PostalCode == person.Address.Postal.PostalCode))
                    //{
                    //    //  db.Postals.Remove(db.Postals.Where(p => p.PostalCode == person.Address.Postal.PostalCode).FirstOrDefault());
                    //}
                    //if (db.Postals.Any(p => p.PostalCode == person.Address.Postal.PostalCode))
                    //{
                    //    db.Postals.Find(person.Address.Postal.PostalCode);
                    //}

                    //if (db.Postals != null && db.Postals.Any(p => p.PostalCode == staticMapData.PostalCode))
                    //{
                    //    postal.Addresses.Add(staticMapData.GetAddress());
                    //    db.Entry(postal).State = System.Data.Entity.EntityState.Modified;
                    //    db.Entry(postal.Addresses).State = System.Data.Entity.EntityState.Modified;
                    //}
                    //else
                    //{
                    //    db.Entry(postal).State = System.Data.Entity.EntityState.Added;
                    //}
                    datab.Postals.Attach(postal);
                    datab.Entry(postal).State = System.Data.Entity.EntityState.Added;


                    //db.Postals.Attach(postal);

                    //var entry = db.Entry(postal);

                    //if (db.Persons.Any(p => p.PersonId == person.PersonId))
                    //{
                    //    entry.State = System.Data.Entity.EntityState.Modified;
                    //    Debug.WriteLine("--- MODIFIED ---");
                    //}
                    //else
                    //{
                    //    entry.State = System.Data.Entity.EntityState.Added;
                    //    Debug.WriteLine("--- ADDED ----");
                    //}





                    //foreach (Postal p in db.Postals)
                    //{
                    //    Debug.WriteLine("+++++++++++++++++++++------------- " + p.PostalCode);
                    //}
                    // other changed properties
                    datab.SaveChanges();

                    // db.SaveChanges();
                }
                //   this.Dispatcher.Invoke((Action)(() =>
                //{

                //}));
                Dispatcher.Invoke(() =>
                    {
                        //persons.Add(person);
                        staticMapDataList.Add(staticMapData);
                        PersonsList.InvalidateArrange();
                        PersonsList.UpdateLayout();
                        PersonsList.Items.Refresh();
                    });
                //PersonsList.UpdateLayout()
                //persons.Add(person);
            }
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
            new Thread(() => WebScraping()).Start();

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
    }

    public class TodoItem
    {
        public string Title { get; set; }
        public string Title2 { get; set; }
        public int Completion { get; set; }
    }

    public class ViewModel
    {
        public List<Personal> Items
        {
            get
            {
                return new List<Personal>
            {
                new Personal { Name = "P1", Age = 1 },
                new Personal { Name = "P2", Age = 2 }
            };
            }
        }
    }

    public class Personal
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

}
