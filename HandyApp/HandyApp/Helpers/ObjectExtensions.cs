using System.Collections.Generic;
using System.Linq;

namespace HandyApp.Helpers
{
    public static class ObjectExtensions
    {

        public static List<Variance> DetailedCompare<T>(this T val1, T val2)
        {
            var propertyInfo = val1.GetType().GetProperties();
            return propertyInfo.Select(f => new Variance
            {
                Property = f.Name,
                OldValue = f.GetValue(val1),
                NewValue = f.GetValue(val2)
            })
                .Where(v => !v.OldValue.Equals(v.NewValue))
                .ToList();
        }

        public class Variance
        {
            public string Property { get; set; }
            public object OldValue { get; set; }
            public object NewValue { get; set; }
        }

    }
}
