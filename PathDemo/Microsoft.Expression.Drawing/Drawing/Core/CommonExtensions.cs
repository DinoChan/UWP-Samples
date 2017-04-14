using Microsoft.Expression.Drawing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Microsoft.Expression.Controls;

namespace Microsoft.Expression.Drawing.Core
{
	internal static class CommonExtensions
	{
		public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> newItems)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			List<T> ts = collection as List<T>;
			if (ts != null)
			{
				ts.AddRange(newItems);
				return;
			}
			foreach (T newItem in newItems)
			{
				collection.Add(newItem);
			}
		}

		public static bool ClearIfSet(this DependencyObject dependencyObject, DependencyProperty dependencyProperty)
		{
			if (dependencyObject.ReadLocalValue(dependencyProperty) == DependencyProperty.UnsetValue)
			{
				return false;
			}
			dependencyObject.ClearValue(dependencyProperty);
			return true;
		}

		internal static T DeepCopy<T>(this T obj)
		where T : class
		{
			if (obj == null)
			{
				return default(T);
			}
			Type type = obj.GetType();
			if (type.IsValueType || type == typeof(string))
			{
				return obj;
			}
			if (typeof(IList).IsAssignableFrom(type))
			{
				IList lists = (IList)Activator.CreateInstance(type);
				((IList)(object)obj).ForEach((object o) => lists.Add(o.DeepCopy<object>()));
				return (T)lists;
			}
			if (!type.IsClass)
			{
				CultureInfo currentCulture = CultureInfo.CurrentCulture;
				string typeNotSupported = ExceptionStringTable.TypeNotSupported;
				object[] fullName = new object[] { type.FullName };
				throw new NotSupportedException(string.Format(currentCulture, typeNotSupported, fullName));
			}
			object obj1 = Activator.CreateInstance(obj.GetType());
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			for (int i = 0; i < (int)properties.Length; i++)
			{
				PropertyInfo propertyInfo = properties[i];
				if (propertyInfo.CanRead && propertyInfo.CanWrite)
				{
					object value = propertyInfo.GetValue(obj, null);
					if (value != propertyInfo.GetValue(obj1, null))
					{
						propertyInfo.SetValue(obj1, value.DeepCopy<object>(), null);
					}
				}
			}
			return (T)obj1;
		}

		public static bool EnsureListCount<T>(this IList<T> list, int count, Func<T> factory = null)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (list.EnsureListCountAtLeast<T>(count, factory))
			{
				return true;
			}
			if (list.Count <= count)
			{
				return false;
			}
			List<T> ts = list as List<T>;
			if (ts == null)
			{
				for (int i = list.Count - 1; i >= count; i--)
				{
					list.RemoveAt(i);
				}
			}
			else
			{
				ts.RemoveRange(count, list.Count - count);
			}
			return true;
		}

		public static bool EnsureListCountAtLeast<T>(this IList<T> list, int count, Func<T> factory = null)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (list.Count >= count)
			{
				return false;
			}
			List<T> ts = list as List<T>;
			if (ts == null || factory != null)
			{
				for (int i = list.Count; i < count; i++)
				{
					list.Add((factory == null ? default(T) : factory()));
				}
			}
			else
			{
				ts.AddRange(new T[count - list.Count]);
			}
			return true;
		}

		public static IEnumerable<T> FindVisualDesendent<T>(this DependencyObject parent, Func<T, bool> condition)
		where T : DependencyObject
		{
			Queue<DependencyObject> dependencyObjects = new Queue<DependencyObject>();
			parent.GetVisualChildren().ForEach<DependencyObject>((DependencyObject child) => dependencyObjects.Enqueue(child));
			while (dependencyObjects.Count > 0)
			{
				DependencyObject dependencyObject = dependencyObjects.Dequeue();
				IEnumerable<DependencyObject> visualChildren = dependencyObject.GetVisualChildren();
				visualChildren.ForEach<DependencyObject>((DependencyObject child) => dependencyObjects.Enqueue(child));
				T t = (T)(dependencyObject as T);
				if (t == null || !condition(t))
				{
					continue;
				}
				yield return t;
			}
		}

		public static void ForEach(this IEnumerable items, Action<object> action)
		{
			foreach (object item in items)
			{
				action(item);
			}
		}

		public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
		{
			foreach (T item in items)
			{
				action(item);
			}
		}

		public static void ForEach<T>(this IList<T> list, Action<T, int> action)
		{
			for (int i = 0; i < list.Count; i++)
			{
				action(list[i], i);
			}
		}

		public static IEnumerable<DependencyObject> GetVisualChildren(this DependencyObject parent)
		{
			int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
			for (int i = 0; i < childrenCount; i++)
			{
				yield return VisualTreeHelper.GetChild(parent, i);
			}
		}

		public static T Last<T>(this IList<T> list)
		{
			return list[list.Count - 1];
		}

		public static void RemoveLast<T>(this IList<T> list)
		{
			list.RemoveAt(list.Count - 1);
		}

		public static bool SetIfDifferent(this DependencyObject dependencyObject, DependencyProperty dependencyProperty, object value)
		{
			if (object.Equals(dependencyObject.GetValue(dependencyProperty), value))
			{
				return false;
			}
			dependencyObject.SetValue(dependencyProperty, value);
			return true;
		}
	}
}