using Lego.Ev3.Core;
using Lego.Ev3.Desktop;
using System;

namespace Hein.MindCuber.Console
{
    class Program
    {
        private static Brick _brick;

        static void Main(string[] args)
        {
            Connect();
        }

        private async static void Connect()
        {
            _brick = new Brick(new BluetoothCommunication("COM10"));
            _brick.BrickChanged += BrickChanged;
            await _brick.ConnectAsync();
            await _brick.DirectCommand.PlayToneAsync(100, 3000, 100);
        }

        private static void BrickChanged(object sender, BrickChangedEventArgs e)
        {
            System.Console.WriteLine("Brick changed");
        }
    }
}
