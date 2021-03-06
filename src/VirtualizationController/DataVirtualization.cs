﻿using DogData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Virtualization
{
    /// <summary>
    /// Synchronous version of virtualization.
    /// Implementing the IList interface for data conforming to the IData interface
    /// </summary>
    /// 
    public class DataVirtualization<T> : ObservableCollection<T>, IList
    {
        protected IData<T> data { get; }
        protected int size { get; }
        protected int lifetime { get; }
        protected int maxPages { get; }

        protected int count = -1;

        protected Dictionary<int, IList<T>> pages = new Dictionary<int, IList<T>>();
        protected Dictionary<int, DateTime> lastUse = new Dictionary<int, DateTime>();

        public DataVirtualization() { }

        public DataVirtualization(IData<T> data, int size, int lifetime)
        {
            this.data = data;
            this.size = size;
            this.lifetime = lifetime;
        }

        public DataVirtualization(IData<T> data, int size, int lifetime, int maxPages)
        {
            this.data = data;
            this.size = size;
            this.lifetime = lifetime;
            this.maxPages = maxPages;
        }

        public new virtual int Count
        {
            get
            {
                if (count == -1)
                {
                    Count = data.Available();
                }
                return count;
            }
            set
            {
                count = value;
            }
        }

        public new virtual T this[int i]
        {
            get
            {
                int page = i / size;
                int offset = i % size;

                RequestPage(page);
                if (offset > size / 2 && page < Count / size)
                {
                    RequestPage(page + 1);
                }
                else if (page > 0)
                {
                    RequestPage(page - 1);
                }

                Clean();

                return pages[page][offset];
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        private static int Compare(KeyValuePair<int, double> a, KeyValuePair<int, double> b)
        {
            return a.Value.CompareTo(b.Value);
        }

        public virtual void Clean()
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
                for(int i = maxPages; i < len; i++)
                {
                    int page = lastUseList[i].Key;
                    pages.Remove(page);
                    lastUse.Remove(page);
                }
            }
        }

        protected virtual void RequestPage(int page)
        {
            if (!pages.ContainsKey(page))
            {
                pages.Add(page, null);
                lastUse.Add(page, DateTime.MinValue);

                ObservableCollection<T> newPage = data.ListOfAvailable(page * size, size);
                if (pages.ContainsKey(page))
                {
                    pages[page] = newPage;
                }
            }
            lastUse[page] = DateTime.Now;
        }
    }
}
