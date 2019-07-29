using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Parser.Extensions
{
    public static class MyList
    {
        // bad api. You claim that you are extending ICollection, but assume that the API will be used with arrays only.
        // this code is for arrays, and its API should clearly state this. Besides, ICollection provides Add() out of the box see F12. 
        // We shouldn't write code that contradicts with existing concepts even in the sandbox. 

        public static T[] Add<T>(this ICollection<T> items, T a)
        {
            T[] mass = new T[items.Count];
            var counter = 0;
            foreach (var item in items)
            {
                mass[counter] = item;
                counter++;
            }
            // You can avoid using this operation. 
            // In addition, I suggest you review documentation before using any existing API. 
            // https://docs.microsoft.com/en-us/dotnet/api/system.array.resize?view=netframework-4.8
            Array.Resize<T>(ref mass, items.Count + 1);
            mass[items.Count] = a;
            items = mass;
            return mass;

        }
        /*
                public static void MassAdd<T>(this T [] items, T a)
                {
                    if (items == null)
                    {
                        items = new T[items.Length+1];
                        return;
                    }

                    if (items.Length != items.Length + 1)
                    {
                        T[] newArray = new T[items.Length + 1];
                        Array.Copy(items, 0, newArray, 0, items.Length > items.Length + 1 ? items.Length + 1 : items.Length);
                       items = newArray;
                        items[items.Length-1] = a;
                    }

                }
                */
    }
    class NewList<TypeOfList>
    {
        private TypeOfList[] List;
        public NewList()
        {
            List = new TypeOfList[0];
        }
        public TypeOfList this[int elementIndex]
        {
            get
            {
                ThrowIfInvalidInsertIndex(elementIndex);
                return List[elementIndex];
            }
            set
            {
                ThrowIfInvalidInsertIndex(elementIndex);
                List[elementIndex] = value;
            }
        }
        private void ChangeListSize(int newSize)
        {
            TypeOfList[] temp = new TypeOfList[newSize];
            for (int i = 0; i < List.Length; i++)
            {
                temp[i] = List[i];
            }
            List = temp;
        }
        private void InsertByIndex(int index, TypeOfList item)
        {
            ThrowIfInvalidInsertIndex(index);
            ChangeListSize(List.Length + 1);
            for (var i = List.Length - 1; i > index; i--)
            {
                List[i] = List[i - 1];
            }
            List[index] = item;
        }
        private void ThrowIfInvalidInsertIndex(int itemIndex)
        {
            if ((itemIndex < 0) || (itemIndex > List.Length))
            {
                throw new IndexOutOfRangeException("Попытка добавить/получить элемент в список типа MyList с недопустимым индексом");
            }
        }
        public int IndexOf(TypeOfList x)
        {
            int itemFirstIndex = 0;
            foreach (var item in List)
            {
                if (item.Equals(x))
                {
                    return itemFirstIndex;
                }
                itemFirstIndex++;
            }
            return -1;
        }
        public void AddFirst(TypeOfList item) => InsertByIndex(0, item);
        public void AddByIndex(int index, TypeOfList item) => InsertByIndex(index, item);
        public void AddLast(TypeOfList item) => InsertByIndex(List.Length, item);
        public void Resize(int newSize) => ChangeListSize(newSize);
        public int Length => List.Length;
    }
}
