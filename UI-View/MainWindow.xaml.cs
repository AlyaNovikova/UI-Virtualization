using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using Virtualization;

namespace ClientApp
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
        /// Version without virtualization. Just a list of items
        /// </summary>
        /// 
        private void Button_List(object sender, RoutedEventArgs e)
        {
            //Test data without database
            //var data = DataControllerTest.Data_List();

            var data = DataController.Data_List();

            DataContext = data;
        }

        /// <summary>
        /// Version with synchronous virtualization
        /// </summary>
        /// 
        private void Button_Sync(object sender, RoutedEventArgs e)
        {
            //Test data without database
            //var data = DataControllerTest.Data_Sync();

            var data = DataController.Data_Sync();

            DataContext = data;
        }

        /// <summary>
        /// Version with asynchronous virtualization
        /// </summary>
        /// 
        private void Button_Async(object sender, RoutedEventArgs e)
        {
            //Test data without database
            //var data = DataControllerTest.Data_Async();

            var data = DataController.Data_Async();

            DataContext = data;
        }
    }
}
