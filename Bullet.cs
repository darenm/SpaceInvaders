using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SpaceInvaders
{
    public class Bullet
    {
        private Direction _direction;
        private Point _location;
        private Point _oldLocation;
        public bool IsActive { get; set; }

        public Direction Direction
        {
            get => _direction;
            set
            {
                if (value != Direction.Down && value != Direction.Up)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _direction = value;
            }
        }

        public Point Location
        {
            get => _location;
            private set => _location = value;
        }

        public void Fire(Point startLocation, Direction direction)
        {
            if (IsActive)
            {
                return;
            }
            _direction = direction;
            Location = startLocation;
            _oldLocation = startLocation;
            IsActive = true;
        }

        public void MoveBullet()
        {
            if (!IsActive)
            {
                return;
            }

            _oldLocation = Location;

            switch (Direction)
            {
                case Direction.Up:
                    if (Location.Y > 0)
                    {
                        Location = new Point(Location.X, Location.Y -1);
                    }
                    else
                    {
                        IsActive = false;
                    }
                    break;
                case Direction.Down:
                    if (Location.Y < 40)
                    {
                        Location = new Point(Location.X, Location.Y + 1);
                    }
                    else
                    {
                        IsActive = false;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Draw()
        {
            if (IsActive)
            {
                _oldLocation.Write(" ");
                Location.Write("|", ConsoleColor.DarkRed);
            }
            else
            {
                _oldLocation.Write(" ");
                Location.Write(" ");
            }
        }

        public void DeletePrevious()
        {
            if (IsActive)
            {
            }
        }

    }
}
