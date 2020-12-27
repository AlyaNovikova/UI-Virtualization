using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Virtualization;

namespace ClientApp
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
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
            //var data = DataControllerMock.Data_List();

            var data = DataController.Data_List();

            DataContext = data;
            treeView.ItemsSource = data;
        }

        /// <summary>
        /// Version with synchronous virtualization
        /// </summary>
        /// 
        private void Button_Sync(object sender, RoutedEventArgs e)
        {
            //Test data without database
            //var data = DataControllerMock.Data_Sync();

            var data = DataController.Data_Sync();

            DataContext = data;
            treeView.ItemsSource = data;
        }

        /// <summary>
        /// Version with asynchronous virtualization
        /// </summary>
        /// 
        private void Button_Async(object sender, RoutedEventArgs e)
        {
            //Test data without database
            //var data = DataControllerMock.Data_Async();

            var data = DataController.Data_Async();

            DataContext = data;
            treeView.ItemsSource = data;
        }
    }
}
