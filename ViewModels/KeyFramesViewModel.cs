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

        public enum Speed
        {
            Slow,
            Medium,
            Fast
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
                    return "Right";
                case (int)MotorHat.Stepper.Command.BACKWARD:
                    return "Left";
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


        public string DisplayPanDirection => GetDisplayDirection(panDirection);


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


        private double pauseTime;

        public double PauseTime
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



        private Speed rate;

        public Speed Rate
        {
            get
            {
                return rate;
            }

            set
            {
                SetProperty(ref rate, value);
            }
        }
    }
}
