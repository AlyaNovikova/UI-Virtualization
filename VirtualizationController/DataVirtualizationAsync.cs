using DogData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Virtualization
{
    /// <summary>
    /// Asynchronous version of virtualization.
    /// Count pages are loaded asynchronously in the direction of the user's current scrolling.
    /// </summary>
    /// 
    public class DataVirtualizationAsync<T> : DataVirtualization<T>, INotifyPropertyChanged, INotifyCollectionChanged
    {
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

        private int pagesForLoading { get; }
        public bool asyncLoading { get; set; }

        private SynchronizationContext synchronizationContext { get; }

        public new event NotifyCollectionChangedEventHandler CollectionChanged;
        public new event PropertyChangedEventHandler PropertyChanged;

        public DataVirtualizationAsync(IData<T> data, int size, int lifetime, int maxPages, int pagesForLoading)
            : base(data, size, lifetime, maxPages)
        {
            this.pagesForLoading = pagesForLoading;
            synchronizationContext = SynchronizationContext.Current;
        }

        public DataVirtualizationAsync(IData<T> data, int size, int lifetime, int pagesForLoading)
            : base(data, size, lifetime)
        {
            this.pagesForLoading = pagesForLoading;
            synchronizationContext = SynchronizationContext.Current;
        }

        protected new void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }
        protected void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
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

        public bool AsyncLoading
        {
            get
            {
                return asyncLoading;
            }
            set
            {
                asyncLoading = value;

                OnPropertyChanged("AsyncLoading");
            }
        }


        private void UpdateUIWithCount(object state)
        {
            Count = (int)state;

            AsyncLoading = false;
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

        private static int Compare(KeyValuePair<int, double> a, KeyValuePair<int, double> b)
        {
            return a.Value.CompareTo(b.Value);
        }

        public override void Clean()
        {

            ObservableCollection<int> lastPages = new ObservableCollection<int>(lastUse.Keys);
            List<KeyValuePair<int, double>> lastUseList = new List<KeyValuePair<int, double>>();

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
                    else
                    {
                        lastUseList.Add(new KeyValuePair<int, double>(page, passed));
                    }
                }
            }

            int len = lastUseList.Count;
            if (len > maxPages)
            {
                lastUseList.Sort(Compare);
                for (int i = maxPages; i < len; i++)
                {
                    int page = lastUseList[i].Key;
                    pages.Remove(page);
                    lastUse.Remove(page);
                }
            }
        }

        private void GetPage(object state)
        {
            AsyncLoading = true;

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

            AsyncLoading = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override void RequestPage(int page)
        {
            Trace.WriteLine("FFFFFFFFFffff", page.ToString());
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
                    for (int bias = 1; bias < pagesForLoading; bias++)
                    {
                        TryToRequestNextPage(page + bias);
                    }
                }
                else
                {
                    for (int bias = 1; bias < pagesForLoading; bias++)
                    {
                        TryToRequestPreviousPage(page - bias);
                    }
                }

                Clean();

                if ((!pages.ContainsKey(page)) || pages[page] is null)
                {
                    return default;
                }

                return pages[page][offset];
            }
        }
    }
}
