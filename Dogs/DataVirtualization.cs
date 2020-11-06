using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Virtualization
{
    class DataVirtualization<T> : ObservableCollection<T>, IList
    {
        private IData<T> Data { get; }
        private int Size { get; } = 100; 
        private int Lifetime { get; } = 10000;
        private int count = -1;

        private Dictionary<int, IList<T>> Pages = new Dictionary<int, IList<T>>();
        private Dictionary<int, DateTime> LastUse = new Dictionary<int, DateTime>();

        public DataVirtualization(IData<T> data, int size)
        {
            this.Data = data;
            this.Size = size;
        }

        public DataVirtualization(IData<T> data)
        {
            this.Data = data;
        }

        public int Count
        {
            get { 
                if (count == -1) {
                    Count = Data.Available();
                }
                return count;
            }
            set { count = value; }
        }

        public T this[int i]
        {
            get {
                int page = i / Size;
                int offset = i % Size;

                RequestPage(page);
                if (offset > Size / 2 && page < Count / Size) {
                    RequestPage(page + 1);
                } else if (page > 0) {
                    RequestPage(page - 1);
                }

                Clean();

                return Pages[page][offset];
            }
        }

        object IList.this[int index]
        {
            get { return this[index]; }
            set { throw new NotSupportedException(); }
        }

        public void Clean()
        {
            ObservableCollection<int> lastPages = new ObservableCollection<int>(LastUse.Keys);
            foreach (int page in lastPages)
            {
                if (page != 0 && (DateTime.Now - LastUse[page]).TotalMilliseconds > Lifetime) {
                    Pages.Remove(page);
                    LastUse.Remove(page);
                }
            }
        }

        protected void RequestPage(int page)
        {
            if (!Pages.ContainsKey(page))
            {
                Pages.Add(page, null);
                LastUse.Add(page, DateTime.MinValue);

                ObservableCollection<T> newPage = Data.ListOfAvailable(page * Size, Size);
                if (Pages.ContainsKey(page))
                    Pages[page] = newPage;
            }
            LastUse[page] = DateTime.Now;
        }
    }
}
