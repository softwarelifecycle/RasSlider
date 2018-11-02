using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RasSlider.Services;

namespace RasSlider.ViewModels
{

    public class SliderViewModel : VMBase
    {
        private MotorService motorService;

        public RelayCommand SetHomeCommand
        {
            get;
            private set;
        }

        public RelayCommand GoHomeCommand
        {
            get;
            private set;
        }

        public RelayCommand KeyFrameCommand
        {
            get;
            private set;
        }

        public RelayCommand PlayCommand
        {
            get;
            private set;
        }

        public RelayCommand PauseCommand
        {
            get;
            private set;
        }

        private double degreesToRotate;

        public double DegreesToRotate
        {
            get
            {
                return degreesToRotate;
            }

            set
            {
                SetProperty(ref degreesToRotate, value);
                Debug.WriteLine($"Degrees: {value}");
            }
        }

        private ObservableCollection<KeyFramesViewModel> keyFrameCollection;

        public ObservableCollection<KeyFramesViewModel> KeyFrameCollection
        {
            get
            {
                return keyFrameCollection;
            }

            set
            {
                SetProperty(ref keyFrameCollection, value);
            }
        }


        private ushort sliderPosition;

        public ushort SliderPosition
        {
            get
            {
                return sliderPosition;
            }

            set
            {
                motorService.MoveSlider(value, sliderPosition, value);
                SetProperty(ref sliderPosition, value);
                Debug.WriteLine($"Position: {value}");
            }

        }


        public SliderViewModel()
        {

            SetHomeCommand = new RelayCommand(SetHomeExecute, true);
            GoHomeCommand = new RelayCommand(GoHomeExecute, true);
            KeyFrameCommand = new RelayCommand(KeyFrameExecute, true);
            PlayCommand = new RelayCommand(PlayExecute, true);
            PauseCommand = new RelayCommand(PauseExecute, true);

            KeyFrameCollection = new ObservableCollection<KeyFramesViewModel>();

            motorService = new MotorService();

            KeyFramesViewModel kf = new KeyFramesViewModel() { ID = 1, NumberOfSteps = 200, NumberOfDegrees = 30, PauseTime = 1, Rate = KeyFramesViewModel.Speed.Medium };
            KeyFrameCollection.Add(kf);
        }

        private void SetHomeExecute()
        {

        }

        private void GoHomeExecute()
        {

        }

        private void KeyFrameExecute()
        {

        }


        private  void PlayExecute()
        {
           
        }

        private void PauseExecute()
        {

        }
    }
}
