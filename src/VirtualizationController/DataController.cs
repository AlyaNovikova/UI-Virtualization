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
    public class DataController
    {
        /// <summary>
        /// Data constants
        /// </summary>
        /// 

        private static int elements { get; } = getParameter("elements");
        private static int delay { get; } = getParameter("delay");
        private static int size { get; } = getParameter("size");
        private static int lifetime { get; } = getParameter("lifetime");
        private static int pagesForLoading { get; } = getParameter("pagesForLoading");
        private static int maxPages { get; } = getParameter("maxPages");

        private static int getParameter(string parameterName)
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings[parameterName]);
        }

        /// <summary>
        /// Version without virtualization. Just a list of items
        /// </summary>
        /// 
        public static ObservableCollection<Dog> Data_List()
        {
            DogData.DogIData dogData = new DogData.DogIData(delay);

            ObservableCollection<Dog> data = dogData.AllDogs();

            return data;
        }

        /// <summary>
        /// Version with synchronous virtualization
        /// </summary>
        /// 
        public static DataVirtualization<Dog> Data_Sync()
        {
            DogIData dogData = new DogIData(delay);

            return new DataVirtualization<Dog>(dogData, size, lifetime, maxPages);
        }

        /// <summary>
        /// Version with asynchronous virtualization
        /// </summary>
        /// 
        public static DataVirtualizationAsync<Dog> Data_Async()
        {
            DogIData dogData = new DogIData(delay);

            return new DataVirtualizationAsync<Dog>(dogData, size, lifetime, maxPages, pagesForLoading);
        }
    }
}
