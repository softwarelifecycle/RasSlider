using AdafruitClassLibrary;
using System.Threading.Tasks;

namespace RasSlider.Services
{
    public class MotorService
    {
        public MotorHat.Stepper SliderStepper { get; set; }
        public MotorHat.Stepper PanStepper { get; set; }
        private MotorHat motorHat;

        public MotorService()
        {
            InitMotors();
        }

        private async Task InitMotors()
        {
            motorHat = new MotorHat();
            await motorHat.InitAsync(1600).ConfigureAwait(false);
            SliderStepper = motorHat.GetStepper(200, 1);
            SliderStepper.SetSpeed(60);
        }

        public async void MoveSlider(ushort steps, ushort oldPos, ushort newPos)
        {
            MotorHat.Stepper.Command direction = newPos > oldPos ? MotorHat.Stepper.Command.FORWARD : MotorHat.Stepper.Command.BACKWARD;
            await Task.Run(() => SliderStepper.step(steps, direction, MotorHat.Stepper.Style.SINGLE));
        }

        public async Task PanCamera()
        {

        }

    }
}
