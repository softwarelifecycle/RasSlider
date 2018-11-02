using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasSlider.ViewModels
{
    public class KeyFramesViewModel : VMBase
    {

        public enum Speed
        {
            Slow,
            Medium,
            Fast
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


        private int numberOfSteps;

        public int NumberOfSteps
        {
            get
            {
                return numberOfSteps;
            }

            set
            {
                SetProperty(ref numberOfSteps, value);
            }
        }


        private double numberOfDegrees;

        public double NumberOfDegrees
        {
            get
            {
                return numberOfDegrees;
            }

            set
            {
                SetProperty(ref numberOfDegrees, value);
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
