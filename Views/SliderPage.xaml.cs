using System;

using RasSlider.ViewModels;

using Windows.UI.Xaml.Controls;

namespace RasSlider.Views
{
    public sealed partial class SliderPage : Page
    {
        private SliderViewModel ViewModel
        {
            get { return DataContext as SliderViewModel; }
        }

        public SliderPage()
        {
            InitializeComponent();
        }
    }
}
