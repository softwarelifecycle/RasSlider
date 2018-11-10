using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace RasSlider.Controls
{
    public class SliderValueChangeCompletedEventArgs : RoutedEventArgs
    {
        private readonly double _value;

        public double Value { get { return _value; } }

        public SliderValueChangeCompletedEventArgs(double value)
        {
            _value = value;
        }
    }
    public delegate void SlideValueChangeCompletedEventHandler(object sender, SliderValueChangeCompletedEventArgs args);

    public class UpdateWhenStoppedSlider : Slider
    {
        public event SlideValueChangeCompletedEventHandler ValueChangeCompleted = delegate { };
        private bool _dragging = false;

        public UpdateWhenStoppedSlider()
        {

        }

        protected void OnValueChangeCompleted(double value)
        {
            ValueChangeCompleted?.Invoke(this, new SliderValueChangeCompletedEventArgs(value));
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var thumb = GetTemplateChild("HorizontalThumb") as Thumb;
            if (thumb != null)
            {
                thumb.DragStarted += ThumbOnDragStarted;
                thumb.DragCompleted += ThumbOnDragCompleted;
            }
            thumb = base.GetTemplateChild("VerticalThumb") as Thumb;
            if (thumb != null)
            {
                thumb.DragStarted += ThumbOnDragStarted;
                thumb.DragCompleted += ThumbOnDragCompleted;
            }
        }

        private void ThumbOnDragCompleted(object sender, DragCompletedEventArgs e)
        {
            _dragging = false;
            OnValueChangeCompleted(this.Value);
        }

        private void ThumbOnDragStarted(object sender, DragStartedEventArgs e)
        {
            _dragging = true;
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            if (!_dragging)
            {
                OnValueChangeCompleted(newValue);
            }
        }
    }
}
