using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthyChef.Common.StaticClasses
{
    public static class CompareHelper
    {
        public static List<string> GetFieldNames(Type dataType, List<Type> fieldTypes)
        {
            return dataType.GetProperties().ToList().Where(a => fieldTypes.Contains(a.PropertyType)).Select(a => a.Name).ToList();
        }
    }

    public class StringDataTypeComparer<T> : IComparer<T>
    {
        public string FieldName { get; set; }
        public bool Ascending { get; set; }

        public int Compare(T x, T y)
        {
            IComparable compareX = (IComparable)x.GetType().GetProperty(FieldName).GetValue(x, null);
            IComparable compareY = (IComparable)y.GetType().GetProperty(FieldName).GetValue(y, null);

            if (Ascending)
            {
                return compareX.CompareTo(compareY);
            }
            else
            {
                return compareX.CompareTo(compareY) * -1;
            }
        }
    }
}
