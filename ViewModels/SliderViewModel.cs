using AdafruitClassLibrary;
using GalaSoft.MvvmLight.Command;
using RasSlider.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.UI.Popups;

namespace RasSlider.ViewModels
{

    public class SliderViewModel : VMBase
    {
        private MotorService motorService;
        private double panHomePosition = 120;
        private double priorDegreesToPan = 120;
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

        public RelayCommand OSPlayCommand
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


        private double degreesToPan = 120;

        public double DegreesToPan
        {
            get
            {
                return degreesToPan;
            }

            set
            {
                SetProperty(ref degreesToPan, value);
                Debug.WriteLine(DegreesToPan - 120);
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
            OSPlayCommand = new RelayCommand(OSPlayExecute, true);
            KeyFrameCommand = new RelayCommand(KeyFrameExecute, true);
            PlayCommand = new RelayCommand(PlayExecute, true);
            ReleaseCommand = new RelayCommand(ReleaseExecute, true);
            ResetCommand = new RelayCommand(ResetExecute, true);

            KeyFrameCollection = new ObservableCollection<KeyFramesViewModel>();

            motorService = new MotorService();
        }

        private async void ResetExecute()
        {
            MessageDialog msgbox = new MessageDialog("Are you sure?", "Resetting!");

            msgbox.Commands.Clear();
            msgbox.Commands.Add(new UICommand { Label = "Yes", Id = 0 });
            msgbox.Commands.Add(new UICommand { Label = "No", Id = 1 });

            var res = await msgbox.ShowAsync();
            if ((int)res.Id == 0)
            {
                KeyFrameCollection = new ObservableCollection<KeyFramesViewModel>();
                priorDegreesToPan = 120;
                priorSliderPosition = 0;
                homePosition = 0;
                SliderPosition = 0;
                DegreesToPan = 120;
            }

        }

        private void SetHomeExecute()
        {
            priorSliderPosition = SliderPosition;
            homePosition = SliderPosition;
        }

        private void OSPlayExecute()
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
                SliderDirection = GetDirection(SliderPosition, priorSliderPosition),
                PanDirection = GetDirection(DegreesToPan, priorDegreesToPan),
                PauseTime = 0,
                Rate = KeyFramesViewModel.Speed.Medium
            };

            KeyFrameCollection.Add(kf);
            priorSliderPosition = SliderPosition;
            priorDegreesToPan = DegreesToPan;
        }

        private int? GetDirection(double currentPos, double priorPos)
        {
            if (currentPos == priorPos)
                return null;
            else
                return currentPos > priorPos ? (int)MotorHat.Stepper.Command.FORWARD : (int)MotorHat.Stepper.Command.BACKWARD;
        }

        private int GetPanDirection(double currentPos, double priorPos)
        {
            int direction = 0;
            double deltaDeg = priorPos - currentPos;
            if (deltaDeg < panHomePosition)
            {
                direction = (int)MotorHat.Stepper.Command.BACKWARD;
            }
            else
            {
                direction = (int)MotorHat.Stepper.Command.FORWARD;
            }
            return direction;
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
