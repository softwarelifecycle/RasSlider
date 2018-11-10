using RasSlider.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace RasSlider.Controls
{
    public class CameraSlider : Slider
    {
        /// <summary> 
        /// Fired when the thumb has been clicked, and dragging is initiated 
        /// </summary> 
       // public event EventHandler<EventArgs> ThumbDragStarted;

        /// <summary> 
        /// Fired when the thumb has been released 
        /// </summary> 
       // public event EventHandler<EventArgs> ThumbDragCompleted;

        public CameraSlider()
            : base()
        {
            DefaultStyleKey = typeof(Slider);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //Set up drag event handlers 
            if (Thumb != null)
            {
                Thumb.DragStarted += new DragStartedEventHandler(thumb_DragStarted);
                Thumb.DragCompleted += new DragCompletedEventHandler(thumb_DragCompleted);
            }
        }

        void thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            SliderViewModel dc = this.DataContext as SliderViewModel;
            dc.SliderPosition = Value;
        }

        void thumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            //OnThumbDragStarted(this, new EventArgs());
        }

        private Thumb Thumb
        {
            get
            {
                return GetThumb(this) as Thumb; ;
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
