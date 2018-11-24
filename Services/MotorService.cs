using AdafruitClassLibrary;
using System;
using System.Threading.Tasks;

namespace RasSlider.Services
{
    /// <summary>
    /// Provide services for motorHat!
    /// https://learn.adafruit.com/adafruit-class-library-for-windows-iot-core/motorhat-class
    /// </summary>
    public class MotorService
    {
        public MotorHat.Stepper SliderStepper { get; set; }

        public MotorHat.Stepper PanStepper { get; set; }
        private MotorHat motorHat;

        private readonly double distanceToStepsRatio = 20;


        public MotorService()
        {
            InitMotors();
        }

        private async Task InitMotors()
        {
            motorHat = new MotorHat();

            // Init MotorHat! : https://forums.adafruit.com/viewtopic.php?f=8&t=107351&sid=e1397fa84094ef13b2033530714ed7b0
            await motorHat.InitAsync(1600).ConfigureAwait(false);
            SliderStepper = motorHat.GetStepper(200, 1);
            PanStepper = motorHat.GetStepper(200, 2);
        }

        public async void MoveSlider(ushort steps, MotorHat.Stepper.Command direction, uint speed = 60)
        {
            SliderStepper.SetSpeed(speed);
            await Task.Run(() => SliderStepper.step((ushort)Math.Abs(steps * distanceToStepsRatio), direction, MotorHat.Stepper.Style.DOUBLE));
            ReleaseSliderMotor();
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
