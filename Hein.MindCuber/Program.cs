using System;
using System.Threading;
using System.Threading.Tasks;
using Lego.Ev3.Core;
using Lego.Ev3.Desktop;

namespace Hein.MindCuber.ConsoleProgram
{
    class Program
    {
        private static Brick _brick;
        private static readonly uint _oneFourthTurn = 900;
        private static ColorSensor _colorSensor;

        static async Task Main(string[] args)
        {
            await Connect();
            await OneFourthTurn();

            //await _colorSensor.MoveToPiece(ColorSensorPosition.MiddlePiece);
            //Console.WriteLine($"Middle color: {_colorSensor.GetColor()}");
            //await _colorSensor.MoveToPiece(ColorSensorPosition.Base);

            Console.ReadLine();
        }

        private async static Task OneFourthTurn()
        {

            await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.C, 30, _oneFourthTurn, false);
            Thread.Sleep(1000);
            await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.C, -30, _oneFourthTurn, false);
            Thread.Sleep(1000);
            await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.C, 30, _oneFourthTurn / 2, false);
            Thread.Sleep(1000);
            await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.C, -30, _oneFourthTurn / 2, false);
        }

        private async static Task Connect()
        {
            _brick = new Brick(new UsbCommunication());
            _colorSensor = new ColorSensor(_brick);
            _colorSensor.ColorChanged += ColorSensorChanged;

            await _brick.ConnectAsync();

            Console.WriteLine($"color: {_colorSensor.GetColor()}");
        }

        private static void ColorSensorChanged(object sender, ColorSensorEventArgs e)
        {
            Console.WriteLine($"color: {e.Value}");
        }

        //private static void BrickChanged(object sender, BrickChangedEventArgs e)
        //{
        //    float distance = e.Ports[InputPort.Four].SIValue;

        //    if (distance <= 20)
        //    {
        //        Console.WriteLine($"start: {distance}");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"nope : {distance}");
        //    }
        //}
    }
}
