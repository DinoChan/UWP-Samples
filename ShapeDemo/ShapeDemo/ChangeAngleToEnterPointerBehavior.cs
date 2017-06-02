using System;
using Windows.Foundation;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Microsoft.Xaml.Interactivity;

namespace ShapeDemo
{
public class ChangeAngleToEnterPointerBehavior : Behavior<Ellipse>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.PointerEntered += OnAssociatedObjectPointerEntered;
        AssociatedObject.PointerExited += OnAssociatedObjectPointerExited;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.PointerEntered -= OnAssociatedObjectPointerEntered;
        AssociatedObject.PointerExited -= OnAssociatedObjectPointerExited;
    }

    private void OnAssociatedObjectPointerExited(object sender, PointerRoutedEventArgs e)
    {
        UpdateAngle(e);
    }

    private void OnAssociatedObjectPointerEntered(object sender, PointerRoutedEventArgs e)
    {
        UpdateAngle(e);
    }

    private void UpdateAngle(PointerRoutedEventArgs e)
    {
        if (AssociatedObject == null || AssociatedObject.StrokeThickness == 0)
            return;

        AssociatedObject.RenderTransformOrigin = new Point(0.5, 0.5);
        var rotateTransform = AssociatedObject.RenderTransform as RotateTransform;
        if (rotateTransform == null)
        {
            rotateTransform = new RotateTransform();
            AssociatedObject.RenderTransform = rotateTransform;
        }

        var point = e.GetCurrentPoint(AssociatedObject).Position;
        var centerPoint = new Point(AssociatedObject.ActualWidth / 2, AssociatedObject.ActualHeight / 2);
        var angleOfLine = Math.Atan2(point.Y - centerPoint.Y, point.X - centerPoint.X) * 180 / Math.PI;
        rotateTransform.Angle = angleOfLine + 180;
    }
}
}