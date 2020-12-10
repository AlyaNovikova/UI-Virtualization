using DogData;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace Virtualization
{
    /// <summary>
    /// Asynchronous version of virtualization.
    /// Count pages are loaded asynchronously in the direction of the user's current scrolling.
    /// </summary>
    /// 
    public class DataVirtualizationAsync<T> : DataVirtualization<T>, INotifyCollectionChanged
    {
        public DataVirtualizationAsync(IData<T> data, int size, int lifetime, int pagesForLoading)
            : base(data, size, lifetime)
        {
            this.pagesForLoading = pagesForLoading;
            synchronizationContext = SynchronizationContext.Current;
        }
        private int pagesForLoading { get; } = 2;

        private SynchronizationContext synchronizationContext { get; }

        public override event NotifyCollectionChangedEventHandler CollectionChanged;

        protected override event PropertyChangedEventHandler PropertyChanged;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        public override int Count
        {
            get
            {
                if (count == -1)
                {
                    Count = 0;
                    try
                    {
                        ThreadPool.QueueUserWorkItem(GetCount);
                    }
                    catch (Exception ex)
                    {
                        Debug.Print(ex.ToString());
                    }
                }
                return count;
            }
            set
            {
                count = value;
            }
        }

        private void UpdateUIWithCount(object state)
        {
            Count = (int)state;

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void GetCount(object state)
        {
            try
            {
                synchronizationContext.Send(UpdateUIWithCount, data.Available());
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
            }
        }

        public override void Clean()
        {
            ObservableCollection<int> lastPages = new ObservableCollection<int>(lastUse.Keys);
            foreach (int page in lastPages)
            {
                if (page != 0)
                {
                    double passed = (DateTime.Now - lastUse[page]).TotalMilliseconds;

                    if (passed > lifetime)
                    {
                        pages.Remove(page);
                        lastUse.Remove(page);
                    }
                }
            }
        }

        private class Page
        {
            public int index;
            public ObservableCollection<T> page;

            public Page(int index, ObservableCollection<T> page)
            {
                this.index = index;
                this.page = page;
            }
        }

        private void GetPage(object state)
        {

            int page = (int)state;

            ObservableCollection<T> newPageList = data.ListOfAvailable(page * size, size);

            Page newPage = new Page(page, newPageList);

            synchronizationContext.Send(UpdateUIWithPage, newPage);
        }

        private void UpdateUIWithPage(object state)
        {
            Page page = (Page)state;

            int index = page.index;
            ObservableCollection<T> pageList = page.page;

            if (pages.ContainsKey(index))
            {
                pages[index] = pageList;
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override void RequestPage(int page)
        {
            if (!pages.ContainsKey(page))
            {
                pages.Add(page, null);
                lastUse.Add(page, DateTime.MinValue);

                ThreadPool.QueueUserWorkItem(GetPage, page);
            }
            lastUse[page] = DateTime.Now;
        }

        private void TryToRequestNextPage(int page)
        {
            if (page < Count / size)
            {
                RequestPage(page + 1);
            }
        }

        private void TryToRequestPreviousPage(int page)
        {
            if (page > 0)
            {
                RequestPage(page - 1);
            }
        }

        public override T this[int i]
        {
            get
            {
                int page = i / size;
                int offset = i % size;

                RequestPage(page);

                if (offset > size / 2)
                {
                    for (int bias = 0; bias < pagesForLoading; bias++)
                    {
                        TryToRequestNextPage(page + bias);
                    }
                }
                else
                {
                    for (int bias = 0; bias < pagesForLoading; bias++)
                    {
                        TryToRequestPreviousPage(page - bias);
                    }
                }

                Clean();

                if (pages[page] is null)
                {
                    return default;
                }

                return pages[page][offset];
            }
        }
    }
}
