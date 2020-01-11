using Lego.Ev3.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hein.MindCuber.ConsoleProgram
{
    public class ColorSensor
    {
        private float _readedColor;
        private readonly uint _oneFourthTurn = 1800;

        public event Action<object, ColorSensorEventArgs> ColorChanged;

        public Brick Brick { get; set; }

        public ColorSensorPosition Position { get; private set; }

        public ColorSensor(Brick brick)
        {
            Brick = brick;
            Position = ColorSensorPosition.Base;

            Brick.BrickChanged += ReadColorSensor;
        }

        public float GetColor()
        {
            return _readedColor;
        }

        public async Task MoveToPiece(ColorSensorPosition newColorSensorPosition)
        {
            if (newColorSensorPosition != Position)
            {
                (bool forward, uint turns)? command = (true, 0);

                switch (newColorSensorPosition)
                {
                    case ColorSensorPosition.Base:
                        command = GoToBasePosition();
                        break;

                    case ColorSensorPosition.MiddlePiece:
                        command = GoToMiddlePiece();
                        break;

                    case ColorSensorPosition.EdgePiece:
                        break;

                    case ColorSensorPosition.CornerPiece:
                        break;

                    default:
                        break;
                }

                const int power = 20;
                uint duration = _oneFourthTurn * (command?.turns ?? 0);

                await Brick.DirectCommand.TurnMotorAtPowerForTimeAsync(
                    OutputPort.D,
                    command?.forward == true ? power : -power + 5,
                    duration,
                    false
                );

                Thread.Sleep(int.Parse(duration.ToString()) + 100);

                Position = newColorSensorPosition;
            }
            else
            {
                Console.WriteLine("Already in that position");
            }
        }

        private (bool, uint)? GoToMiddlePiece()
        {
            switch (Position)
            {
                case ColorSensorPosition.Base:
                    return (false, 2);
                
                case ColorSensorPosition.MiddlePiece:
                    return null;
                
                case ColorSensorPosition.EdgePiece:
                    break;
                
                case ColorSensorPosition.CornerPiece:
                    break;
            }

            return (true, 0);
        }

        private (bool, uint)? GoToBasePosition()
        {
            switch (Position)
            {
                case ColorSensorPosition.Base:
                    return null;
                
                case ColorSensorPosition.MiddlePiece:
                    return (true, 2);
                
                case ColorSensorPosition.EdgePiece:
                    break;
                
                case ColorSensorPosition.CornerPiece:
                    break;
            }

            return (true, 0);
        }

        private void ReadColorSensor(object sender, BrickChangedEventArgs e)
        {
            float value = e.Ports[InputPort.Three].SIValue;
            if (value != _readedColor)
            {
                _readedColor = e.Ports[InputPort.Three].SIValue;
                ColorChanged?.Invoke(this, new ColorSensorEventArgs(Brick, _readedColor));
            }
        }
    }

    public class ColorSensorEventArgs: EventArgs
    {
        public Brick Brick { get; set; }

        public float Value { get; set; }

        public ColorSensorEventArgs(Brick brick, float value)
        {
            Brick = brick;
            Value = value;
        }
    }

    public enum ColorSensorPosition
    {
        Base,
        MiddlePiece,
        EdgePiece,
        CornerPiece
    }
}
