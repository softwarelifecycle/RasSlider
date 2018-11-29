using AdafruitClassLibrary;
using RasSlider.Helpers;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace RasSlider.Services
{
    /// <summary>
    /// Provide services for motorHat!
    /// https://learn.adafruit.com/adafruit-class-library-for-windows-iot-core/motorhat-class
    /// </summary>
    public class MotorService
    {
        private const string SettingsKey = "NumberOfSteps";
        public MotorHat.Stepper SliderStepper { get; set; }
        private int _numberOfSteps;

        public MotorHat.Stepper PanStepper { get; set; }
        private MotorHat motorHat;

        private readonly double distanceToStepsRatio = 40;
        private readonly double degreesToStepsRatio = 50;

        public bool StopCountingSteps { get; set; }


        public MotorService()
        {
            InitMotors();
        }


        /// <summary>
        /// Counts number of steps it takes to get from one end to the other for calibration purposes...
        /// </summary>
        /// <param name="stopCounting"></param>
        /// <returns></returns>
        public async Task<int> CountSteps(bool stopCounting)
        {
            _numberOfSteps = 0;
            do
            {
                await Task.Run(() => SliderStepper.step((ushort)1, MotorHat.Stepper.Command.FORWARD, MotorHat.Stepper.Style.DOUBLE));
                _numberOfSteps++;
            } while (!StopCountingSteps);

            return _numberOfSteps;
        }

        public async Task SaveTotalSteps(int totalSteps)
        {
            await ApplicationData.Current.LocalSettings.SaveAsync(SettingsKey, totalSteps);
        }

        private async Task InitMotors()
        {
            motorHat = new MotorHat();

            // Init MotorHat! : https://forums.adafruit.com/viewtopic.php?f=8&t=107351&sid=e1397fa84094ef13b2033530714ed7b0
            await motorHat.InitAsync(1600).ConfigureAwait(false);
            SliderStepper = motorHat.GetStepper(200, 1);
            PanStepper = motorHat.GetStepper(200, 2);
        }

        public  void MoveSlider(ushort steps, MotorHat.Stepper.Command direction, uint speed = 60)
        {
            SliderStepper.SetSpeed(speed);
            //Task.Run(() => SliderStepper.step((ushort)Math.Abs(steps * distanceToStepsRatio), direction, MotorHat.Stepper.Style.DOUBLE));
            SliderStepper.step((ushort)Math.Abs(steps * distanceToStepsRatio), direction, MotorHat.Stepper.Style.DOUBLE);
        }

        public async void PanCamera(ushort oldPos, ushort newPos, uint speed = 60)
        {
            //PanStepper.SetSpeed(speed);
            //MotorHat.Stepper.Command direction = newPos > oldPos ? MotorHat.Stepper.Command.FORWARD : MotorHat.Stepper.Command.BACKWARD;
            //await Task.Run(() => PanStepper.step((ushort)Math.Abs(steps * distanceToStepsRatio), direction, MotorHat.Stepper.Style.DOUBLE));
            //ReleasePanMotor();
        }

        public async void ReleaseMotors()
        {
            PanStepper.Release();
            SliderStepper.Release();
        }

        public void ReleaseSliderMotor()
        {
            SliderStepper.Release();
        }

        public void ReleasePanMotor()
        {
            PanStepper.Release();
        }



    }
}
