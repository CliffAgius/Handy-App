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
                ValueA = f.GetValue(val1),
                ValueB = f.GetValue(val2)
            })
                .Where(v => !v.ValueA.Equals(v.ValueB))
                .ToList();
        }

        public class Variance
        {
            public string Property { get; set; }
            public object ValueA { get; set; }
            public object ValueB { get; set; }
        }

    }
}
