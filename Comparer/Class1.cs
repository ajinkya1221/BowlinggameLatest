using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Comparer
{
    public static class Comparer
    {
        public static bool AreSimilar<T>(T first, T second)
        {
            if (first == null || second == null)
                return first == null && second == null;

            if (IsValueType(first.GetType()) || first is string)
                return CompareValues(first, second);

            if (IsEnumerableType(first.GetType()))
            {
                return CompareCollection(first, second);
            }

            return CheckObjectProperties(first, second);
        }

        private static bool IsValueType(Type type)
        {
            return type.IsValueType;
        }

        private static bool IsEnumerableType(Type type)
        {
            return (typeof(IEnumerable).IsAssignableFrom(type));
        }

        private static bool CompareValues<T>(T first, T second)
        {
            return Equals(first, second);
        }

        private static bool CompareCollection<T>(T first, T second)
        {
            if (first == null || second == null)
                return first == null && second == null;
            else
            {
                if (first.GetType().IsArray)
                {
                    dynamic val1 = first;
                    dynamic val2 = second;

                    if (val1.Length != val2.Length)
                        return false;

                    for (int i = 0; i < val1.Length; i++)
                    {
                        object arrVal1 = val1[i];
                        object arrVal2 = val2[i];

                        var type = arrVal1.GetType();
                        if (IsValueType(type) || type == typeof(string))
                        {
                            if (!CompareValues(arrVal1, arrVal2))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (!CheckObjectProperties(arrVal1, arrVal2))
                            {
                                return false;
                            }
                        }
                    }
                }
                else if (first is IList && typeof(T).IsGenericType)
                {

                } else if(first is IDictionary && typeof(T).IsGenericType)
                {

                }                
            }
            return true;
        }


        private static bool CheckObjectProperties<T>(T first, T second)
        {
            var firstProperties = first.GetType().GetProperties();

            //var secondProperties = second.GetType().GetProperties();
            bool returnVal = true;
            foreach (var prop in firstProperties)
            {
                returnVal = AreSimilar(prop.GetValue(first, null), prop.GetValue(second, null));
                if (!returnVal)
                    return false;
            }
            return true;
        }
    }
}
