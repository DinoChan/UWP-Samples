using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PathDemo
{
    public sealed class MyPath : Shape
    {

        #region Constructors

        /// <summary>
        /// Instantiates a new instance of a Path.
        /// </summary>
        public MyPath()
        {
        }

        #endregion Constructors

        #region Dynamic Properties

        /// <summary>
        /// Data property
        /// </summary>
        //[CommonDependencyProperty]
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            "Data",
            typeof(Geometry),
            typeof(MyPath),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender),
            null);

        /// <summary>
        /// Data property
        /// </summary>
        public Geometry Data
        {
            get
            {
                return (Geometry)GetValue(DataProperty);
            }
            set
            {
                SetValue(DataProperty, value);
            }
        }
        #endregion

        #region Protected Methods and Properties

        /// <summary>
        /// Get the path that defines this shape
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                Geometry data = Data;

                if (data == null)
                {
                    data = Geometry.Empty;
                }

                return data;
            }
        }

        ////
        ////  This property
        ////  1. Finds the correct initial size for the _effectiveValues store on the current DependencyObject
        ////  2. This is a performance optimization
        ////
        // override int EffectiveValuesInitialSize
        //{
        //    get { return 13; }
        //}

        #endregion

        protected override Size ArrangeOverride(Size finalSize)
        {
            Output();
            return base.ArrangeOverride(finalSize);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Output();
            return base.MeasureOverride(constraint);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Output();
            base.OnRender(drawingContext);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            Output();
            base.OnRenderSizeChanged(sizeInfo);
        }


        private void Output([CallerMemberName] string methodName = "")
        {
            Debug.WriteLine(DateTime.Now.ToString("mm:ss fffff") + " " + methodName);
        }
    }
}
