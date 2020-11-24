using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace Virtualization
{
    /// <summary>
    /// Communication logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        /// <summary>
        /// Creating sample data with test constants
        /// </summary>
        /// 
        public MainWindow()
        {
            InitializeComponent();


            // DispatcherTimer setup
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

            DogData dogData = new DogData(elements, delay);

            DataContext = new DataVirtualizationAsync<Dog>(dogData, size, lifetime, pagesForLoading);

            ///-- Version without virtualization

            //ObservableCollection<Dog> Dogs = new ObservableCollection<Dog>();

            //Random rnd = new Random();

            //for (int i = 0; i < 1000; i++)
            //{
            //    int image_number = rnd.Next(1, 11);
            //    string image_name = String.Format("/Dogs/{0}.jpg", image_number);
            //    Dogs.Add(new Dog() { Title = i.ToString(), Image = image_name });
            //}

            //DataContext = Dogs;
            //DogsList.ItemsSource = Dogs;
        }

        /// <summary>
        /// System.Windows.Threading.DispatcherTimer.Tick handler
        /// </summary>
        /// 
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            double memory = GC.GetTotalMemory(true) / (double)(1024 * 1024);
            lblMemory.Text = string.Format("{0} MB", memory.ToString("0.####"));
        }

        /// <summary>
        /// Data constants
        /// </summary>
        /// 

        // Total number of items in the list
        static readonly int elements = 500000;

        // An artificial data delay. 
        // It's invoked on every data request, 
        // to simulate real conditions of interaction with large collections.
        static readonly int delay = 1000;

        // Size of one page
        static readonly int size = 100;

        // Time that new page is stored in the page dictionary
        static readonly int lifetime = 10000;

        // Constant for the asynchronous version.
        // The number of pages that will be loaded asynchronously in the scrolling direction
        static readonly int pagesForLoading = 2;
    }
}
