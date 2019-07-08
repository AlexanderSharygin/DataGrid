using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Extensions {
   public static class MyLinq {
        public static int MaxValue<T>(this IEnumerable<T> items, MaxCalculation<T> selector) {
            var current = int.MinValue;
            foreach (var item in items)
            {
                var a = selector(item);
                if (a>current)
                {
                    current = a;
                }
            }
            return current;
           
            
        }
    }
    public delegate int MaxCalculation<TItem>(TItem item);
        
}
