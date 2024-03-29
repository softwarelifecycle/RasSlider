﻿using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace RasSlider.Controls
{
    public class SliderRotateImage : Slider
    {


        public double DegreesToRotate
        {
            get { return (double)GetValue(DegreesToRotateProperty); }
            set
            {
                SetValue(DegreesToRotateProperty, value);
                Debug.WriteLine($"Degrees: {value}");
            }
        }

        // Using a DependencyProperty as the backing store for DegreesToRotate.  This enables animation, styling, binding, etc...
        public static readonly Windows.UI.Xaml.DependencyProperty DegreesToRotateProperty =
            DependencyProperty.Register("DegreesToRotate", typeof(double), typeof(SliderRotateImage), new PropertyMetadata(0));


    }
}
