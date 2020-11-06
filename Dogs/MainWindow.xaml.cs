using System.Windows;

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

            int elements = 100000;
            int delay = 1000;
            int pageSize = 100;

            DogData dogData = new DogData(elements, delay);

            DataContext = new DataVirtualization<Dog>(dogData, pageSize);


            ///-- Версия без виртуализации

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
    }
}
