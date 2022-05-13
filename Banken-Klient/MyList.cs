using System;

namespace Banken_Klient
{
    public class MyList<T>
    {
        T[] list;

        public int Count
        {
            get { return list.Length; }
        }

        public MyList()
        {
            list = new T[0];
        }

        public void Add(T t)
        {
            T[] tempList = new T[list.Length + 1];
            list.CopyTo(tempList, 0);
            tempList[list.Length] = t;

            list = tempList;
        }

        public void Remove(T t)
        {
            T[] tempList = new T[list.Length - 1];
            bool hasOccured = false;
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i]!.Equals(t) || hasOccured)
                {
                    tempList[i] = list[i + 1];
                    hasOccured = true;
                }
                else
                {
                    tempList[i] = list[i];
                }
            }
            list = tempList;
        }

        public void Clear()
        {
            list = new T[0];
        }

        public T this[int i]
        {
            get { return list[i] ?? throw new IndexOutOfRangeException(); }
        }
    }
}