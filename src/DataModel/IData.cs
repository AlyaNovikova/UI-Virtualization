using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogData
{
    /// <summary>
    /// Data interface required for virtualization
    /// </summary>
    /// 
    public interface IData<T>
    {
        /// <summary>
        /// Returns the number of currently available elements
        /// </summary>
        ///
        int Available();

        /// <summary>
        /// Returns a list of length cnt available elements from the start element
        /// </summary>
        ///
        ObservableCollection<T> ListOfAvailable(int start, int cnt);
    }
    
}
