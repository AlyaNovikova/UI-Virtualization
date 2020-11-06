using System.Collections.ObjectModel;

namespace Virtualization
{
    public interface IData<T>
    {
        // Возвращает количество доступных в данный момент элементов
        int Available();
        // Возвращает список доступных элементов
        ObservableCollection<T> ListOfAvailable(int start, int cnt);
    }
}
