using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Virtualization
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            /// List<TodoItem> items = new List<TodoItem>();
            Dogs = new ObservableCollection<Dog>();
            Random rnd = new Random();
          
            for (int i = 0; i < 1000; i++)
            {
                int image_number = rnd.Next(1, 10);
                string image_name = String.Format("/Images/{0}.jpg", image_number);
                Dogs.Add(new Dog() { Title = i.ToString(), Data = image_name });
            }

            DogsList.ItemsSource = Dogs;
        }

        public ObservableCollection<Dog> Dogs;
        public class Dog
        {
            public string Title { get; set; }
            public string Data { get; set; }
        }
        private void DeleteDog_Clicked(object sender, RoutedEventArgs e)
        {

            Button cmd = (Button)sender;
            if (cmd.DataContext is Dog)
            {

                Dog deleteme = (Dog)cmd.DataContext;
                Dogs.Remove(deleteme);
            }
        }
    }
}
