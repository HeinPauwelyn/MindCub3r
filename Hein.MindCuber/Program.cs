using System;
using Lego.Ev3.Core;
using Lego.Ev3.Desktop;
using static System.Console;

namespace Hein.MindCuber.Console
{
    class Program
    {
        private static Brick _brick;

        static void Main(string[] args)
        {
            Connect();
            Reset();

            ReadLine();
        }

        private async static void Reset()
        {
            await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.B, 40, 3000, false);
        }

        private async static void Connect()
        {
            _brick = new Brick(new UsbCommunication());
            _brick.BrickChanged += BrickChanged;
            await _brick.ConnectAsync();
            await _brick.DirectCommand.PlayToneAsync(100, 3000, 100);
        }

        private static void BrickChanged(object sender, BrickChangedEventArgs e)
        {
            float distance = e.Ports[InputPort.Four].SIValue;

            if (distance <= 20)
            {
                WriteLine($"start: {distance}");
            }
            else
            {
                WriteLine($"nope : {distance}");
            }
        }
    }
}
