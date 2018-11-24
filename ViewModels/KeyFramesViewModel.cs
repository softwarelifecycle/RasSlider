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

        private MotorHat.Stepper.Command direction;

        public string DisplayDirection { get { return direction == MotorHat.Stepper.Command.FORWARD ? "Forward" : "BackWard"; } }

        public MotorHat.Stepper.Command Direction
        {
            get
            {
                return direction;
            }

            set
            {
                SetProperty(ref direction, value);
            }
        }

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
