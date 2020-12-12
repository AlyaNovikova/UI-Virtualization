using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;

namespace DogData
{
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
    public class DogIDataMock : IData<DogMock>
    {
        private readonly int elements = 500000;
        private readonly int delay = 1000;

        public DogIDataMock(int elements, int delay)
        {
            this.elements = elements;
            this.delay = delay;
        }

        public int Available()
        {
            Thread.Sleep(delay);
            return elements;
        }

        public ObservableCollection<DogMock> ListOfAvailable(int start, int cnt)
        {
            Thread.Sleep(delay);

            ObservableCollection<DogMock> Dogs = new ObservableCollection<DogMock>();
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

                DogMock dog = new DogMock { Title = (i + 1).ToString(), Breed = dogs_breed[j], Image = image_path };
                Dogs.Add(dog);
            }
            return Dogs;
        }
    }
}