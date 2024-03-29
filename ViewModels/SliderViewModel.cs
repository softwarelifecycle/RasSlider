﻿using AdafruitClassLibrary;
using GalaSoft.MvvmLight.Command;
using RasSlider.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        public List<Speeds> SpeedList = new List<Speeds>();
        public List<PanSpeeds> PanSpeedList = new List<PanSpeeds>();

        CancellationTokenSource cancelPlayBack = new CancellationTokenSource();

        public SliderViewModel()
        {
            Init();
        }

        private void Init()
        {
            KeyFrameCollection = new ObservableCollection<KeyFramesViewModel>();
            motorService = new MotorService();

            InitCommands();
            InitSpeeds();
            InitPanSpeeds();
        }

        private void InitCommands()
        {
            SetHomeCommand = new RelayCommand(SetHomeExecute, true);
            KeyFrameCommand = new RelayCommand(KeyFrameExecute, true);
            PlayCommand = new RelayCommand(PlayExecute, true);
            ReleaseCommand = new RelayCommand(ReleaseExecute, true);
            ResetCommand = new RelayCommand(ResetExecute, true);
        }

        private void InitSpeeds()
        {
            SpeedList.Add(new Speeds { SpeedID = 1, SpeedValue = 5, SpeedDesc = "Slow" });
            SpeedList.Add(new Speeds { SpeedID = 2, SpeedValue = 200, SpeedDesc = "Normal" });
            SpeedList.Add(new Speeds { SpeedID = 3, SpeedValue = 300, SpeedDesc = "Fast" });
        }

        private void InitPanSpeeds()
        {
            PanSpeedList.Add(new PanSpeeds { PanSpeedID = 1, SpeedValue = 5, SpeedDesc = "Slow" });
            PanSpeedList.Add(new PanSpeeds { PanSpeedID = 2, SpeedValue = 200, SpeedDesc = "Normal" });
            PanSpeedList.Add(new PanSpeeds { PanSpeedID = 3, SpeedValue = 300, SpeedDesc = "Fast" });
            PanSpeedList.Add(new PanSpeeds { PanSpeedID = 4, SpeedValue = 300, SpeedDesc = "Syncronize" });
        }

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
                SpeedID = 3,
                PanSpeedID = 2
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
                return currentPos > priorPos ? (int)MotorHat.Stepper.Command.BACKWARD : (int)MotorHat.Stepper.Command.FORWARD;
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

        private async void PlayExecute()
        {

            foreach (KeyFramesViewModel keyFrame in KeyFrameCollection)
            {
                MotorHat.Stepper.Command panCommand;
                MotorHat.Stepper.Command sliderCommand;
                Task SliderTask = null;
                Task PanTask = null;

                List<Task> TaskList = new List<Task>();

                if (keyFrame.SliderPosition > 0 && Enum.TryParse(keyFrame.SliderDirection.ToString(), out sliderCommand))
                {
                    SliderTask = Task.Factory.StartNew(() =>
                       motorService.MoveSlider((ushort)keyFrame.SliderPosition, sliderCommand,
                               cancelPlayBack.Token, MotorHat.Stepper.Style.DOUBLE,
                               (uint)SpeedList.FirstOrDefault(k => k.SpeedID == keyFrame.SpeedID).SpeedValue)
                   );
                }
                if (keyFrame.DegreesToPan > 0 && Enum.TryParse(keyFrame.PanDirection.ToString(), out panCommand))
                {
                    PanTask = Task.Factory.StartNew(() => motorService.PanCamera((ushort)keyFrame.DegreesToPan, panCommand,
                        cancelPlayBack.Token, MotorHat.Stepper.Style.DOUBLE,
                        (uint)PanSpeedList.FirstOrDefault(k => k.PanSpeedID == keyFrame.SpeedID).SpeedValue));
                }

                if (SliderTask != null)
                    TaskList.Add(SliderTask);

                if (PanTask != null)
                    TaskList.Add(PanTask);

                //wait till both finish... but return to caller, Keep responsive incase need to cancel!
                await Task.WhenAll(TaskList);

                // Pause if specified
                if (keyFrame.PauseTime > 0)
                {
                    Thread.Sleep(keyFrame.PauseTime * 1000);
                }

                motorService.ReleaseSliderMotor();
            }
        }


        /// <summary>
        /// release motor current so they can spin and cool off!
        /// </summary>
        private void ReleaseExecute()
        {
            //cancelPlayBack.Cancel();
            motorService.ReleaseMotors();
        }
    }

    public class Speeds
    {
        public int SpeedID { get; set; }

        public int SpeedValue { get; set; }
        public string SpeedDesc { get; set; }
    }

    public class PanSpeeds
    {
        public int PanSpeedID { get; set; }

        public int SpeedValue { get; set; }
        public string SpeedDesc { get; set; }
    }
}
