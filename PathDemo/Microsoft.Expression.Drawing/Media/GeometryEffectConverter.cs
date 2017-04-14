using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.Expression.Media
{
	public sealed class GeometryEffectConverter : TypeConverter
	{
		private static Dictionary<string, GeometryEffect> registeredEffects;

		static GeometryEffectConverter()
		{
			Dictionary<string, GeometryEffect> strs = new Dictionary<string, GeometryEffect>()
			{
				{ "None", GeometryEffect.DefaultGeometryEffect },
				{ "Sketch", new SketchGeometryEffect() }
			};
			GeometryEffectConverter.registeredEffects = strs;
		}

		public GeometryEffectConverter()
		{
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return typeof(string).IsAssignableFrom(sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return typeof(string).IsAssignableFrom(destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			GeometryEffect geometryEffect;
			string str = value as string;
			if (str == null || !GeometryEffectConverter.registeredEffects.TryGetValue(str, out geometryEffect))
			{
				return null;
			}
			return geometryEffect.CloneCurrentValue();
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			object key;
			if (typeof(string).IsAssignableFrom(destinationType))
			{
				if (value is string)
				{
					return value;
				}
				Dictionary<string, GeometryEffect>.Enumerator enumerator = GeometryEffectConverter.registeredEffects.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, GeometryEffect> current = enumerator.Current;
						if ((current.Value == null ? value != null : !current.Value.Equals(value as GeometryEffect)))
						{
							continue;
						}
						key = current.Key;
						return key;
					}
					return null;
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				return key;
			}
			return null;
		}
	}
}