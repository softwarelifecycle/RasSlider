using AdafruitClassLibrary;
using System.Threading;

namespace RasSlider.ViewModels
{
    public class KeyFramesViewModel : VMBase
    {

        static int nextID;

        public KeyFramesViewModel()
        {
            ID = Interlocked.Increment(ref nextID);
        }

        public double PriorDegreesToPan { get; set; }

        public double PriorSliderPosition { get; set; }

        private int? sliderDirection;

        public string DisplaySliderDirection => GetDisplayDirection(sliderDirection);

        private string GetDisplayDirection(int? direction)
        {
            switch (direction)
            {
                case (int)MotorHat.Stepper.Command.FORWARD:
                    return "Left";
                case (int)MotorHat.Stepper.Command.BACKWARD:
                    return "Right";
                default:
                    return "None";
            }
        }

        private string GetPanDirection(int? direction)
        {
            switch (direction)
            {
                case (int)MotorHat.Stepper.Command.FORWARD:
                    return "CCW";
                case (int)MotorHat.Stepper.Command.BACKWARD:
                    return "CW";
                default:
                    return "None";
            }
        }

        public int? SliderDirection
        {
            get
            {
                return sliderDirection;
            }

            set
            {
                SetProperty(ref sliderDirection, value);
            }
        }


        private int? panDirection;

        public int? PanDirection
        {
            get
            {
                return panDirection;
            }

            set
            {
                SetProperty(ref panDirection, value);
            }
        }


        public string DisplayPanDirection => GetPanDirection(panDirection);


        private int id;

        public int ID
        {
            get
            {
                return id;
            }

            set
            {
                SetProperty(ref id, value);
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


        private double degreesToPan;

        public double DegreesToPan
        {
            get
            {
                return degreesToPan;
            }

            set
            {
                SetProperty(ref degreesToPan, value);
            }
        }


        private int pauseTime;

        public int PauseTime
        {
            get
            {
                return pauseTime;
            }

            set
            {
                SetProperty(ref pauseTime, value);
            }
        }



        private int speedID;

        public int SpeedID
        {
            get
            {
                return speedID;
            }

            set
            {
                SetProperty(ref speedID, value);
            }
        }

        private int panSpeedID;

        public int PanSpeedID
        {
            get
            {
                return panSpeedID;
            }

            set
            {
                SetProperty(ref panSpeedID, value);
            }
        }
    }
}
