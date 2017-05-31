using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PointCollectionAnimationDemmo
{
    /// <summary>
    /// Base which can be derived from to implement PointCollection animations
    /// </summary>
    public abstract class PointCollectionAnimationBase : AnimationTimeline
    {
        #region Boilerplate
        public override Type TargetPropertyType
        {
            get { return typeof(PointCollection); }
        }

        public sealed override object GetCurrentValue(object defaultOriginValue,
            object defaultDestinationValue,
            AnimationClock animationClock)
        {
            return GetCurrentValue(defaultOriginValue as PointCollection,
                defaultDestinationValue as PointCollection,
                animationClock);
        }

        public PointCollection GetCurrentValue(PointCollection defaultOriginValue,
            PointCollection defaultDestinationValue,
            AnimationClock animationClock)
        {
            return GetCurrentValueCore(defaultOriginValue,
                defaultDestinationValue,
                animationClock);
        }

        protected abstract PointCollection GetCurrentValueCore(PointCollection defaultOriginValue,
            PointCollection defaultDestinationValue,
            AnimationClock animationClock);
        #endregion // Boilerplate
    }
}
