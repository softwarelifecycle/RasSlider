using AdafruitClassLibrary;
using GalaSoft.MvvmLight.Command;
using RasSlider.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace RasSlider.ViewModels
{

    public class SliderViewModel : VMBase
    {
        private MotorService motorService;
        private double priorDegreesToPan = 0;
        private double priorSliderPosition = 0;
        private double homePosition;


        public RelayCommand SetHomeCommand
        {
            get;
            private set;
        }

        public RelayCommand ResetCommand
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

        public RelayCommand ReleaseCommand
        {
            get;
            private set;
        }


        private double degreesToPan = 0;

        public double DegreesToPan
        {
            get
            {
                return degreesToPan;
            }

            set
            {
                SetProperty(ref degreesToPan, value);
                Debug.WriteLine($"Degrees: {value}");
            }
        }

        private uint speed = 300;

        public uint Speed
        {
            get { return speed; }
            set { SetProperty(ref speed, value); }
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

        private KeyFramesViewModel selectedKeyFrame;

        public KeyFramesViewModel SelectedKeyFrame
        {
            get
            {
                return selectedKeyFrame;
            }

            set
            {
                SetProperty(ref selectedKeyFrame, value);
            }
        }



        private double sliderPosition;

        public double SliderPosition
        {
            get
            {
                return sliderPosition;
            }

            set
            {
                SetProperty(ref sliderPosition, value);
            }

        }


        public SliderViewModel()
        {

            SetHomeCommand = new RelayCommand(SetHomeExecute, true);
            GoHomeCommand = new RelayCommand(GoHomeExecute, true);
            KeyFrameCommand = new RelayCommand(KeyFrameExecute, true);
            PlayCommand = new RelayCommand(PlayExecute, true);
            ReleaseCommand = new RelayCommand(ReleaseExecute, true);
            ResetCommand = new RelayCommand(ResetExecute, true);

            KeyFrameCollection = new ObservableCollection<KeyFramesViewModel>();

            motorService = new MotorService();
        }

        private void ResetExecute()
        {
            KeyFrameCollection = new ObservableCollection<KeyFramesViewModel>();
        }

        private void SetHomeExecute()
        {
            priorSliderPosition = SliderPosition;
            homePosition = SliderPosition;
        }

        private void GoHomeExecute()
        {
            //motorService.MoveSlider((ushort)SliderPosition, (ushort)homePosition, Speed);
        }

        private void KeyFrameExecute()
        {
            KeyFramesViewModel kf = new KeyFramesViewModel()
            {
                PriorDegreesToPan = priorDegreesToPan,
                PriorSliderPosition = priorSliderPosition,
                SliderPosition = Math.Abs(SliderPosition - priorSliderPosition),
                DegreesToPan = Math.Abs(DegreesToPan - priorDegreesToPan),
                Direction = SliderPosition > priorSliderPosition ? MotorHat.Stepper.Command.FORWARD : MotorHat.Stepper.Command.BACKWARD,
                PauseTime = 1,
                Rate = KeyFramesViewModel.Speed.Medium
            };

            KeyFrameCollection.Add(kf);
            priorSliderPosition = SliderPosition;
            priorDegreesToPan = DegreesToPan;
        }


        private void PlayExecute()
        {
            // motorService.MoveSlider((ushort)Math.Abs(value - sliderPosition), (ushort)sliderPosition, (ushort)value, Speed);
            // motorService.PanCamera((ushort)Math.Abs(value - sliderPosition), (ushort)sliderPosition, (ushort)value, Speed);


        }

        /// <summary>
        /// release motor current so they can spin and cool off!
        /// </summary>
        private void ReleaseExecute()
        {
            motorService.ReleaseMotors();
        }
    }
}
