using DogData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtualization
{
    public class DataControllerMock
    {
        /// <summary>
        /// Data constants
        /// </summary>
        /// 

        private static int elements { get; } = getParameter("elements");
        private static int delay { get; } = getParameter("delay");
        private static int size { get; } = getParameter("size");
        private static int lifetime { get; } = getParameter("lifetime");
        private static int maxPages { get; } = getParameter("maxPages");
        private static int pagesForLoading { get; } = getParameter("pagesForLoading");


        private static int getParameter(string parameterName)
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings[parameterName]);
        }

        /// <summary>
        /// Version without virtualization. Just a list of items
        /// </summary>
        /// 
        public static ObservableCollection<DogMock> Data_List()
        {
            DogIDataMock dogData = new DogIDataMock(elements, delay);

            int count = dogData.Available();
            ObservableCollection<DogMock> data = dogData.ListOfAvailable(0, count);

            return data;
        }

        /// <summary>
        /// Version with synchronous virtualization
        /// </summary>
        /// 
        public static DataVirtualization<DogMock> Data_Sync()
        {
            DogIDataMock dogData = new DogIDataMock(elements, delay);

            return new DataVirtualization<DogMock>(dogData, size, lifetime, maxPages);
        }

        /// <summary>
        /// Version with asynchronous virtualization
        /// </summary>
        /// 
        public static DataVirtualizationAsync<DogMock> Data_Async()
        {
            DogIDataMock dogData = new DogIDataMock(elements, delay);

            return new DataVirtualizationAsync<DogMock>(dogData, size, lifetime, maxPages, pagesForLoading);
        }
    }
}
