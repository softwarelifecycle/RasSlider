using Microsoft.Xaml.Interactivity;
using RasSlider.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace RasSlider.Helpers
{
    public class SliderDragEndValueBehavior : Behavior<UpdateWhenStoppedSlider>
    {
        UpdateWhenStoppedSlider slider;

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        "Value", typeof(float), typeof(SliderDragEndValueBehavior), new
        PropertyMetadata(default(float)));
        public float Value
        {
            get { return (float)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        protected override void OnAttached()
        {

            slider = AssociatedObject as UpdateWhenStoppedSlider;
            RoutedEventHandler handler = AssociatedObject_DragCompleted;
            Thumb.DragCompleted += AssociatedObject_DragCompleted;

        }
        private void AssociatedObject_DragCompleted(object sender, RoutedEventArgs e)
        {
            Value = (float)AssociatedObject.Value;
        }
        protected override void OnDetaching()
        {
            slider = AssociatedObject as UpdateWhenStoppedSlider;
            RoutedEventHandler handler = AssociatedObject_DragCompleted;
            Thumb.DragCompleted -= AssociatedObject_DragCompleted;
        }

        private Thumb Thumb
        {
            get
            {
                return GetThumb(slider) as Thumb; ;
            }
        }

        private DependencyObject GetThumb(DependencyObject root)
        {
            if (root is Thumb)
                return root;

            DependencyObject thumb = null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            {
                thumb = GetThumb(VisualTreeHelper.GetChild(root, i));

                if (thumb is Thumb)
                    return thumb;
            }

            return thumb;
        }
    }
}
