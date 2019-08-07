using System.Collections.Generic;
using System;


namespace Parser.Extensions
{
   public static class MyLinq
    {
        public static int MaxValue<T>(this IEnumerable<T> items, string key, MaxValueCalculation<T> MaxCalculationContract)
        {
            var defaultValue = int.MinValue;
            foreach (var item in items)
            {
                if (item != null)
                {
                    var itemValue = MaxCalculationContract(item, key);
                    if (itemValue > defaultValue)
                    {
                        defaultValue = itemValue;
                    }
                }
            }
            return defaultValue;     
        }
      
    }
    public delegate int MaxValueCalculation<TItem>(TItem item, string key);
   
    
}
