// Decompiled with JetBrains decompiler
// Type: Microsoft.Expression.Media.GeometrySource`1
// Assembly: Microsoft.Expression.Drawing, Version=5.0.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 53267B85-F605-404B-B1EF-D8B9E4AAEA10
// Assembly location: D:\Users\jchen\Documents\Visual Studio 2015\Projects\SilverlightApplication2\SilverlightApplication2\Bin\Debug\Microsoft.Expression.Drawing.dll

using Microsoft.Expression.Drawing.Core;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Media
{
  public abstract class GeometrySource<TParameters> : IGeometrySource where TParameters : IGeometrySourceParameters
  {
    private bool geometryInvalidated;
    protected Geometry cachedGeometry;

    public Geometry Geometry { get; private set; }

    public Rect LogicalBounds { get; private set; }

    public Rect LayoutBounds { get; private set; }

    public bool InvalidateGeometry(InvalidateGeometryReasons reasons)
    {
      if ((reasons & InvalidateGeometryReasons.TemplateChanged) != (InvalidateGeometryReasons) 0)
        this.cachedGeometry = (Geometry) null;
      if (this.geometryInvalidated)
        return false;
      this.geometryInvalidated = true;
      return true;
    }

    public bool UpdateGeometry(IGeometrySourceParameters parameters, Rect layoutBounds)
    {
      bool flag = false;
      if (parameters is TParameters)
      {
        Rect logicalBounds = this.ComputeLogicalBounds(layoutBounds, parameters);
        flag = ((flag ? 1 : 0) | (Rect.op_Inequality(this.LayoutBounds, layoutBounds) ? 1 : (Rect.op_Inequality(this.LogicalBounds, logicalBounds) ? 1 : 0))) != 0;
        if (this.geometryInvalidated || flag)
        {
          this.LayoutBounds = layoutBounds;
          this.LogicalBounds = logicalBounds;
          bool force = flag | this.UpdateCachedGeometry((TParameters) parameters);
          flag = force | this.ApplyGeometryEffect(parameters, force);
        }
      }
      this.geometryInvalidated = false;
      return flag;
    }

    protected abstract bool UpdateCachedGeometry(TParameters parameters);

    protected virtual Rect ComputeLogicalBounds(Rect layoutBounds, IGeometrySourceParameters parameters)
    {
      return GeometryHelper.Inflate(layoutBounds, -parameters.GetHalfStrokeThickness());
    }

    private bool ApplyGeometryEffect(IGeometrySourceParameters parameters, bool force)
    {
      bool flag = false;
      Geometry geometry = this.cachedGeometry;
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
          geometry = geometryEffect.OutputGeometry;
        }
      }
      if (this.Geometry != geometry)
      {
        flag = true;
        this.Geometry = geometry;
      }
      return flag;
    }
  }
}
