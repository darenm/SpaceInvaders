using System;
using System.Collections.Generic;
using System.Drawing;

namespace SpaceInvaders
{
    public abstract class AsciiSprite
    {
        private readonly ConsoleColor _color;
        protected readonly List<string[]> _frames;
        protected int _currentFrame;

        public CharacterType CharacterType { get; private set; }

        public AsciiSprite(CharacterType characterType, string[] frame1, string[] frame2, Point initialLocation, ConsoleColor color)
        {
            CharacterType = characterType;
            _frames = new List<string[]> {frame1};
            if (frame2 != null) _frames.Add(frame2);

            Location = initialLocation;
            _color = color;
            _currentFrame = 0;
        }

        public Point Location { get; set; }

        public virtual void Draw()
        {
            var drawLocation = Location;
            foreach (var line in _frames[_currentFrame])
            {
                drawLocation.Write(line, _color);
                drawLocation.Y += 1;
            }
        }

        public abstract bool HitTest(Point bullet);

        public void ChangeFrame()
        {
            if (_frames.Count > 1) _currentFrame ^= 1;
        }
    }

    public class KillableAsciiSprite : AsciiSprite
    {
        public bool Hit { get; set; }
        public KillableAsciiSprite(CharacterType characterType, string[] frame1, string[] frame2, Point initialLocation, ConsoleColor color) : base(
            characterType, frame1, frame2, initialLocation, color)
        {
        }

        public override bool HitTest(Point bullet)
        {
            var offset = new Point(bullet.X - Location.X, bullet.Y - Location.Y);

            if (offset.X < 0 || offset.Y < 0)
            {
                return false;
            }

            if (offset.X >= _frames[_currentFrame][0].Length || offset.Y >= _frames[_currentFrame].Length)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(_frames[_currentFrame][offset.Y].Substring(offset.X, 1)))
            {
                return false;
            }

            return true;
        }

        public override void Draw()
        {
            if (Hit)
            {
                var drawLocation = Location;
                var blankString = new string(' ', _frames[_currentFrame][0].Length);
                foreach (var line in _frames[_currentFrame])
                {
                    drawLocation.Write(blankString);
                    drawLocation.Y += 1;
                }
            }
            else
            {
                base.Draw();
            }
        }
    }

    public class ShieldAsciiSprite : AsciiSprite
    {
        public ShieldAsciiSprite(CharacterType characterType, string[] frame1, string[] frame2, Point initialLocation, ConsoleColor color) : base(
            characterType, frame1, frame2, initialLocation, color)
        {
        }

        public override bool HitTest(Point bullet)
        {
            var offset = new Point(bullet.X - Location.X, bullet.Y - Location.Y);

            if (offset.X < 0 || offset.Y < 0)
            {
                return false;
            }

            if (offset.X >= _frames[_currentFrame][0].Length || offset.Y >= _frames[_currentFrame].Length)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(_frames[_currentFrame][offset.Y].Substring(offset.X, 1)))
            {
                return false;
            }

            var lineArray = _frames[_currentFrame][offset.Y].ToCharArray();
            lineArray[offset.X] = ' ';
            _frames[_currentFrame][offset.Y] = new string(lineArray);
            return true;
        }

        public void AliensHitShields(in int aliensBottom)
        {
            for (var index = 0; index < _frames[_currentFrame].Length; index++)
            {
                var line = _frames[_currentFrame][index];
                if (line == new string(' ', 7))
                {
                    _frames[_currentFrame][index] = string.Empty;
                }
            }

            var offset = aliensBottom - Location.Y;
            if (offset < 0)
            {
                return;
            }

            if (offset >= _frames[_currentFrame].Length)
            {
                return;
            }

            _frames[_currentFrame][offset] = new string(' ', 7);
        }
    }
}