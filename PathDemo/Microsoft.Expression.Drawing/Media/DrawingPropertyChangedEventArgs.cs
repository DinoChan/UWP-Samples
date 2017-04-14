using System;
using System.Runtime.CompilerServices;

namespace Microsoft.Expression.Media
{
	internal class DrawingPropertyChangedEventArgs : EventArgs
	{
		public bool IsAnimated
		{
			get;
			set;
		}

		public DrawingPropertyMetadata Metadata
		{
			get;
			set;
		}

		public DrawingPropertyChangedEventArgs()
		{
		}
	}
}