using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;

namespace Virtualization
{
    /// <summary>
    /// Dog class - sample data class for virtualization testing.
    /// </summary>
    /// 
    public class Dog
    {
        public string Title { get; set; }
        public string Breed { get; set; }
        public string Image { get; set; }
    }

    /// <summary>
    /// Implementing the IData Interface for the Dog class.
    /// The data is in the 'Dogs/' folder.
    /// It contains several images and breeds of dogs, 
    /// which are generated in a random order with repetitions for each data request.
    /// 
    /// An artificial data delay is invoked on every data request,
    /// to simulate real conditions of interaction with large collections.
    /// </summary>
    /// 
    public class DogData : IData<Dog>
    {
        private readonly int elements = 500000;
        private readonly int delay = 1000;

        public DogData(int elements, int delay)
        {
            this.elements = elements;
            this.delay = delay;
        }

        public int Available()
        {
            Thread.Sleep(delay);
            return elements;
        }

        public ObservableCollection<Dog> ListOfAvailable(int start, int cnt)
        {
            Thread.Sleep(delay);

            ObservableCollection<Dog> Dogs = new ObservableCollection<Dog>();
            _ = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = System.IO.Path.GetFullPath("../../dogs_data/breeds.txt");
            List<string> dogs_breed = new List<string>();

            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    dogs_breed.Add(line);
                }
            }
            int bread_size = dogs_breed.Count;

            Random rnd = new Random();

            for (int i = start; i < start + cnt; i++)
            {
                int j = rnd.Next(0, bread_size);

                int image_number = j + 1;
                string image_name = String.Format("../../dogs_data/{0}.jpg", image_number);

                string image_path = System.IO.Path.GetFullPath(image_name);

                Dog dog = new Dog { Title = (i + 1).ToString(), Breed = dogs_breed[j], Image = image_path };
                Dogs.Add(dog);
            }
            return Dogs;
        }
    }
}
