using System;
using DogData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Virtualization;

namespace VirtualizationTest
{
    [TestClass]
    public class DataVirtualizationTest
    {
        [TestMethod]
        public void CountTest()
        {
            DogIDataMock dogData = new DogIDataMock(10, 0);
            var controller = new DataVirtualization<DogMock>(dogData, 1, 10000, 10);

            Assert.IsTrue(controller.Count == 10);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            DogIDataMock dogData = new DogIDataMock(10, 0);
            var controller = new DataVirtualization<DogMock>(dogData, 1, 10000, 10);

            Assert.IsTrue(controller.Count == 10);

            for (int i = 0; i < controller.Count; i++)
            {
                Assert.IsTrue(controller[i] != null);
            }
        }
    }
}
