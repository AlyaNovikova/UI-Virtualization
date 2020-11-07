using System.Collections.ObjectModel;

namespace Virtualization
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

        /// Returns a list of length cnt available elements from the start element
        /// </summary>
        ///
        ObservableCollection<T> ListOfAvailable(int start, int cnt);
    }
}
