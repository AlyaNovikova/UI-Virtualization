using DogData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace DataTest
{
    [TestClass]
    public class DataProviderTest
    {
        [TestMethod]
        public void TestNumberOfDogs()
        {
            int cnt = DogDataProvider.NumberOfDogs();
  
            Assert.IsTrue(cnt > 0);
        }

        [TestMethod]
        public void TestDataSegment()
        {
            var dogs = DogDataProvider.DataSegment(0, 10);

            Assert.IsTrue(dogs != null && dogs.Count == 10);
        }

        [TestMethod]
        public void TestAllDogs()
        {
            var dogs = DogDataProvider.AllDogs();
            Assert.IsTrue(dogs != null && dogs.Count > 0);
        }

        [TestMethod]
        public void TestDogById()
        {
            var dogs = DogDataProvider.AllDogs();
            Assert.IsTrue(dogs != null && dogs.Count > 0);

            foreach (var dog in dogs)
            {
                int dogId = dog.DogId;
                var dogById = DogDataProvider.DogById(dogId);

                Assert.IsTrue(dogById != null);
                Assert.IsTrue(dogById.DogId == dogId);

                Assert.IsTrue(dog.Breed != null);
                Assert.IsTrue(dog.Image_data != null);

            }
        }
    }
}

