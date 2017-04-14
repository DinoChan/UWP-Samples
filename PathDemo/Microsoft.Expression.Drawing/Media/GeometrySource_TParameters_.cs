using Microsoft.Expression.Drawing.Core;
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Media
{
	public abstract class GeometrySource<TParameters> : IGeometrySource
	where TParameters : IGeometrySourceParameters
	{
		private bool geometryInvalidated;

		protected System.Windows.Media.Geometry cachedGeometry;

		public System.Windows.Media.Geometry Geometry
		{
			get
			{
				return JustDecompileGenerated_get_Geometry();
			}
			set
			{
				JustDecompileGenerated_set_Geometry(value);
			}
		}

		private System.Windows.Media.Geometry JustDecompileGenerated_Geometry_k__BackingField;

		public System.Windows.Media.Geometry JustDecompileGenerated_get_Geometry()
		{
			return this.JustDecompileGenerated_Geometry_k__BackingField;
		}

		private void JustDecompileGenerated_set_Geometry(System.Windows.Media.Geometry value)
		{
			this.JustDecompileGenerated_Geometry_k__BackingField = value;
		}

		public Rect LayoutBounds
		{
			get
			{
				return JustDecompileGenerated_get_LayoutBounds();
			}
			set
			{
				JustDecompileGenerated_set_LayoutBounds(value);
			}
		}

		private Rect JustDecompileGenerated_LayoutBounds_k__BackingField;

		public Rect JustDecompileGenerated_get_LayoutBounds()
		{
			return this.JustDecompileGenerated_LayoutBounds_k__BackingField;
		}

		private void JustDecompileGenerated_set_LayoutBounds(Rect value)
		{
			this.JustDecompileGenerated_LayoutBounds_k__BackingField = value;
		}

		public Rect LogicalBounds
		{
			get
			{
				return JustDecompileGenerated_get_LogicalBounds();
			}
			set
			{
				JustDecompileGenerated_set_LogicalBounds(value);
			}
		}

		private Rect JustDecompileGenerated_LogicalBounds_k__BackingField;

		public Rect JustDecompileGenerated_get_LogicalBounds()
		{
			return this.JustDecompileGenerated_LogicalBounds_k__BackingField;
		}

		private void JustDecompileGenerated_set_LogicalBounds(Rect value)
		{
			this.JustDecompileGenerated_LogicalBounds_k__BackingField = value;
		}

		protected GeometrySource()
		{
		}

		private bool ApplyGeometryEffect(IGeometrySourceParameters parameters, bool force)
		{
			bool flag = false;
			System.Windows.Media.Geometry outputGeometry = this.cachedGeometry;
			GeometryEffect geometryEffect = parameters.GetGeometryEffect();
			if (geometryEffect != null)
			{
				if (force)
				{
					flag = true;
					geometryEffect.InvalidateGeometry(InvalidateGeometryReasons.ParentInvalidated);
				}
				if (geometryEffect.ProcessGeometry(this.cachedGeometry))
				{
					flag = true;
					outputGeometry = geometryEffect.OutputGeometry;
				}
			}
			if (this.Geometry != outputGeometry)
			{
				flag = true;
				this.Geometry = outputGeometry;
			}
			return flag;
		}

		protected virtual Rect ComputeLogicalBounds(Rect layoutBounds, IGeometrySourceParameters parameters)
		{
			return GeometryHelper.Inflate(layoutBounds, -parameters.GetHalfStrokeThickness());
		}

		public bool InvalidateGeometry(InvalidateGeometryReasons reasons)
		{
			if ((int)(reasons & InvalidateGeometryReasons.TemplateChanged) != 0)
			{
				this.cachedGeometry = null;
			}
			if (this.geometryInvalidated)
			{
				return false;
			}
			this.geometryInvalidated = true;
			return true;
		}

		protected abstract bool UpdateCachedGeometry(TParameters parameters);

		public bool UpdateGeometry(IGeometrySourceParameters parameters, Rect layoutBounds)
		{
			bool flag = false;
			if (parameters is TParameters)
			{
				Rect rect = this.ComputeLogicalBounds(layoutBounds, parameters);
				flag = flag | (this.LayoutBounds != layoutBounds ? true : this.LogicalBounds != rect);
				if (this.geometryInvalidated || flag)
				{
					this.LayoutBounds = layoutBounds;
					this.LogicalBounds = rect;
					flag = flag | this.UpdateCachedGeometry((TParameters)parameters);
					flag = flag | this.ApplyGeometryEffect(parameters, flag);
				}
			}
			this.geometryInvalidated = false;
			return flag;
		}
	}
}