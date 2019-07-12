using System.Collections.Generic;
using System;


namespace Parser.Extensions
{
   public static class MyLinq
    {
        public static int MaxValue<T>(this IEnumerable<T> items, MaxValueCalculation<T> MaxCalculationContract)
        {
            var defaultValue = int.MinValue;
            foreach (var item in items)
            {
                var itemValue = MaxCalculationContract(item);
                if (itemValue>defaultValue)
                {
                    defaultValue = itemValue;
                }
            }
            return defaultValue;     
        }
    }
    public delegate int MaxValueCalculation<TItem>(TItem item);
   
    
}
