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

            if (IsValueType(typeof(T)) || first is string)
                return CompareValues(first, second);

            if (IsEnumerableType(typeof(T)))
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
                IEnumerable<object> firstCollection, secondCollection;
                firstCollection = (IEnumerable<object>)first;
                secondCollection = (IEnumerable<object>)second;

                if (firstCollection.Count() != secondCollection.Count())
                    return false;
                else
                {
                    object firstCollValue, secondCollValue;
                    Type collType;
                    for (int itemIndex = 0; itemIndex < firstCollection.Count(); itemIndex++)
                    {
                        firstCollValue = firstCollection.ElementAt(itemIndex);
                        secondCollValue = secondCollection.ElementAt(itemIndex);
                        collType = firstCollValue.GetType();
                        if (IsValueType(collType))
                        {
                            if (!CompareValues(firstCollValue, secondCollValue))
                                return false;
                        }
                        else if (!CheckObjectProperties(firstCollValue, secondCollValue))
                            return false;
                    }
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
                if (IsValueType(prop.PropertyType) || prop.PropertyType == typeof(string))
                {
                    returnVal = object.Equals(prop.GetValue(first, null), prop.GetValue(second, null));
                    if (!returnVal)
                        return false;
                } else if (IsEnumerableType(prop.PropertyType))
                {
                    returnVal = CompareCollection(prop.GetValue(first, null), prop.GetValue(second, null));
                    if (!returnVal)
                        return false;
                }
                else
                {
                    returnVal = CheckObjectProperties(prop.GetValue(first, null), prop.GetValue(second, null));
                    if (!returnVal)
                        return false;
                }
                
            }
            return true;
        }
    }
}
