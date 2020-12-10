using DogData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtualization
{
    public class DataControllerTest
    {
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


        /// <summary>
        /// Version without virtualization. Just a list of items
        /// </summary>
        /// 
        public static ObservableCollection<Dog> Data_List()
        {
            DogDataMock dogData = new DogDataMock(elements, delay);

            int count = dogData.Available();
            ObservableCollection<Dog> data = dogData.ListOfAvailable(0, count);

            return data;
        }

        /// <summary>
        /// Version with synchronous virtualization
        /// </summary>
        /// 
        public static DataVirtualization<Dog> Data_Sync()
        {
            DogDataMock dogData = new DogDataMock(elements, delay);

            return new DataVirtualization<Dog>(dogData, size, lifetime);
        }

        /// <summary>
        /// Version with asynchronous virtualization
        /// </summary>
        /// 
        public static DataVirtualizationAsync<Dog> Data_Async()
        {
            DogDataMock dogData = new DogDataMock(elements, delay);

            return new DataVirtualizationAsync<Dog>(dogData, size, lifetime, pagesForLoading);
        }
    }
}
