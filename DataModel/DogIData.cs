using DogDatabase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DogData
{
    public class DogIData : IData<Dog>
    {
        private readonly int delay = 10;

        public DogIData(int delay)
        {
            this.delay = delay;
        }

        public int Available()
        {
            Thread.Sleep(delay);
            return DogDataProvider.NumberOfDogs();
        }

        public ObservableCollection<Dog> ListOfAvailable(int start, int cnt)
        {
            Thread.Sleep(delay);

            ObservableCollection<Dog> Dogs = new ObservableCollection<Dog>();

            foreach (var dogDataBase in DogDataProvider.DataSegment(start, cnt))
            {
                Dog dog = new Dog
                {
                    Title = dogDataBase.DogId.ToString(),
                    Breed = dogDataBase.Breed,
                    Image = dogDataBase.Image_data
                };

                Dogs.Add(dog);
            }

            return Dogs;
        }

        public ObservableCollection<Dog> AllDogs()
        {
            ObservableCollection<Dog> Dogs = new ObservableCollection<Dog>();

            foreach (var dogDataBase in DogDataProvider.AllDogs())
            {
                Dog dog = new Dog
                {
                    Title = dogDataBase.DogId.ToString(),
                    Breed = dogDataBase.Breed,
                    Image = dogDataBase.Image_data
                };

                Dogs.Add(dog);
            }

            return Dogs;
        }
    }
}
