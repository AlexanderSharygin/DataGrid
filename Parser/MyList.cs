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
}
