using Microsoft.Expression.Drawing.Core;
using Microsoft.Expression.Media;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Microsoft.Expression.Shapes
{
    public abstract class PrimitiveShape : Path, IGeometrySourceParameters, IShape
    {
        private IGeometrySource geometrySource;

        private bool realizeGeometryScheduled;

        private readonly static DependencyProperty StretchListenerProperty;

        private readonly static DependencyProperty ThicknessListenerProperty;

        public new Geometry Data
        {
            get
            {
                return (Geometry)base.GetValue(Path.DataProperty);
            }
            set
            {
                base.SetValue(Path.DataProperty, (object)value);
            }
        }

        public Thickness GeometryMargin
        {
            get
            {
                if (this.Data == null)
                {
                    return new Thickness();
                }
                return this.GeometrySource.LogicalBounds.Subtract(this.Data.Bounds);
            }
        }

        private IGeometrySource GeometrySource
        {
            get
            {
                IGeometrySource geometrySource = this.geometrySource;
                if (geometrySource == null)
                {
                    IGeometrySource geometrySource1 = this.CreateGeometrySource();
                    IGeometrySource geometrySource2 = geometrySource1;
                    this.geometrySource = geometrySource1;
                    geometrySource = geometrySource2;
                }
                return geometrySource;
            }
        }

        public Geometry RenderedGeometry
        {
            get
            {
                return this.GeometrySource.Geometry;
            }
        }

        static PrimitiveShape()
        {
            PrimitiveShape.StretchListenerProperty = DependencyProperty.Register("StretchListener", typeof(System.Windows.Media.Stretch), typeof(PrimitiveShape), new DrawingPropertyMetadata((object)System.Windows.Media.Stretch.Fill, DrawingPropertyMetadataOptions.AffectsRender));
            PrimitiveShape.ThicknessListenerProperty = DependencyProperty.Register("ThicknessListener", typeof(double), typeof(PrimitiveShape), new DrawingPropertyMetadata((object)1, DrawingPropertyMetadataOptions.AffectsRender));
        }

        protected PrimitiveShape()
        {
            this.SetListenerBinding(PrimitiveShape.StretchListenerProperty, "Stretch");
            this.SetListenerBinding(PrimitiveShape.ThicknessListenerProperty, "StrokeThickness");
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (this.GeometrySource.UpdateGeometry(this, finalSize.Bounds()) && !this.realizeGeometryScheduled)
            {
                this.realizeGeometryScheduled = true;
                base.LayoutUpdated += new EventHandler(this.OnLayoutUpdated);
            }
            base.ArrangeOverride(finalSize);
            return finalSize;
        }

        protected abstract IGeometrySource CreateGeometrySource();

        public void InvalidateGeometry(InvalidateGeometryReasons reasons)
        {
            if (this.GeometrySource.InvalidateGeometry(reasons))
            {
                base.InvalidateArrange();
                if (Application.Current != null && Application.Current.RootVisual != null && (bool)Application.Current.RootVisual.GetValue(DesignerProperties.IsInDesignModeProperty) && (int)(reasons & InvalidateGeometryReasons.IsAnimated) != 0 && this.GeometrySource.UpdateGeometry(this, this.ActualBounds()) && !this.realizeGeometryScheduled)
                {
                    this.realizeGeometryScheduled = true;
                    base.LayoutUpdated += new EventHandler(this.OnLayoutUpdated);
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(base.StrokeThickness, base.StrokeThickness);
        }

        System.Windows.Media.Stretch Microsoft.Expression.Media.IGeometrySourceParameters.Stretch
        {
            get { return base.Stretch; }
        }

        Brush Microsoft.Expression.Media.IGeometrySourceParameters.Stroke
        {
            get { return base.Stroke; }
        }

        double Microsoft.Expression.Media.IGeometrySourceParameters.StrokeThickness
        {
            get { return base.StrokeThickness; }
        }

        Brush Microsoft.Expression.Media.IShape.Fill
        {
            get { return base.Fill; }
            set { base.Fill = value; }
        }

        System.Windows.Media.Stretch Microsoft.Expression.Media.IShape.Stretch
        {
            get { return base.Stretch; }
            set { base.Stretch = value; }
        }

        Brush Microsoft.Expression.Media.IShape.Stroke
        {
            get { return base.Stroke; }
            set { base.Stroke = value; }
        }

        double Microsoft.Expression.Media.IShape.StrokeThickness
        {
            get { return base.StrokeThickness; }
            set { base.StrokeThickness = value; }
        }


        
        private new void OnLayoutUpdated(object sender, EventArgs e)
        {
            this.realizeGeometryScheduled = false;
            base.LayoutUpdated -= new EventHandler(this.OnLayoutUpdated);
            this.RealizeGeometry();
        }

        private void RealizeGeometry()
        {
            this.Data = this.GeometrySource.Geometry.CloneCurrentValue();
            if (this.RenderedGeometryChanged != null)
            {
                this.RenderedGeometryChanged(this, EventArgs.Empty);
            }
        }

        private void SetListenerBinding(DependencyProperty targetProperty, string sourceProperty)
        {
            Binding binding = new Binding(sourceProperty)
            {
                Source = this,
                Mode = BindingMode.OneWay
            };
            base.SetBinding(targetProperty, binding);
        }

        public event EventHandler RenderedGeometryChanged;
    }
}