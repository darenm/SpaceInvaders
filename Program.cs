using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace SpaceInvaders
{
    class Program
    {
        private static bool _gameOver;
        private static Direction _direction;
        private static AsciiSprite _player;
        private static List<AsciiSprite> _aliens;
        private static List<AsciiSprite> _shields;
        private static Size _size;
        private static int _score;
        private const int _aliensPerRow = 9;
        private const int _rowsOfAliens = 4;
        private const int _numberOfShields = 6;
        private static DateTime _lastAlienMove;
        private static TimeSpan _alienMoveInterval;
        private static Direction _alienDirection;
        private static Bullet _playerBullet;
        private static Bullet[] _alienBullets;

        private static void Main(string[] args)
        {
            Start();
        }

        private static void Start()
        {
            Setup();
            while (_gameOver == false)
            {
                Draw();
                Input();
                Logic();
                Thread.Sleep(10);
            }
            End();
        }

        private static void Setup()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(1,1);
            Console.SetBufferSize(80,40);
            Console.SetWindowSize(80,40);
            _gameOver = false;
            _size = new Size(80, 40);
            _player = CharacterFactory.Create(CharacterType.Gunner, new Point(0, 26));

            _aliens = new List<AsciiSprite>();

            for (int row = 0; row < _rowsOfAliens; row++)
            {
                for (int column = 0; column < _aliensPerRow; column++)
                {
                    _aliens.Add(CharacterFactory.Create(DetermineAlienType(row), new Point(column*7, row*3 + 4)));
                }

            }

            _shields = new List<AsciiSprite>();
            for (int column = 0; column < _aliensPerRow; column++)
            {
                _shields.Add(CharacterFactory.Create(CharacterType.Shield, new Point(column * 9, 22)));
            }

            _lastAlienMove = DateTime.Now;
            _alienMoveInterval = TimeSpan.FromSeconds(1);
            _alienDirection = Direction.Right;

            _playerBullet = new Bullet();
        }

        private static CharacterType DetermineAlienType(in int row)
        {
            return row switch
            {
                0 => CharacterType.Alien1,
                1 => CharacterType.Alien2,
                _ => CharacterType.Alien3
            };
        }

        private static void End()
        {
        }

        private static void Logic()
        {
            if (DateTime.Now - _lastAlienMove > _alienMoveInterval)
            {
                switch (_alienDirection)
                {
                    case Direction.Left:
                        if (_aliens.Left() > 0)
                        {
                            _aliens.ForEach(a => MoveAlien(a, new Point(a.Location.X - 1, a.Location.Y)));
                        }
                        else
                        {
                            UpdateAlienMoveInterval();
                            _alienDirection = Direction.Right;
                            _aliens.ForEach(a => MoveAlien(a, new Point(a.Location.X, a.Location.Y + 1)));
                        }

                        break;
                    case Direction.Right:
                        if (_aliens.Right() < 73)
                        {
                            _aliens.ForEach(a => MoveAlien(a, new Point(a.Location.X+1, a.Location.Y)));
                        }
                        else
                        {
                            UpdateAlienMoveInterval();
                            _alienDirection = Direction.Left;
                            _aliens.ForEach(a => MoveAlien(a, new Point(a.Location.X, a.Location.Y + 1)));
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(_alienDirection));
                }

                _lastAlienMove = DateTime.Now;
                Task.Factory.StartNew(() => Console.Beep(2000, 10));
            }

            _playerBullet.MoveBullet();

            var deadAlien = _aliens.FirstOrDefault(a => ((KillableAsciiSprite) a).Hit);
            if (deadAlien != null)
            {
                _aliens.Remove(deadAlien);
            }

            if (_aliens.Count == 0)
            {
                End();
            }

            if (_playerBullet.IsActive)
            {
                _shields.ForEach(s =>
                {
                    var hit = s.HitTest(_playerBullet.Location);
                    if (hit)
                    {
                        _playerBullet.IsActive = false;
                    }
                });

                _aliens.ForEach(a =>
                {
                    var hit = a.HitTest(_playerBullet.Location);
                    if (hit)
                    {
                        _playerBullet.IsActive = false;
                        ((KillableAsciiSprite)a).Hit = true;
                    }
                });


            }
        }

        private static void MoveAlien(AsciiSprite a, Point newLocation)
        {
            a.Location = newLocation;
            a.ChangeFrame();
        }

        private static void UpdateAlienMoveInterval()
        {
            if (_alienMoveInterval > TimeSpan.FromMilliseconds(100))
            {
                _alienMoveInterval -= TimeSpan.FromMilliseconds(100);
            }
        }

        private static void Input()
        {
            if (!Console.KeyAvailable) { return; }

            var key = Console.ReadKey(false).Key;

            if (key == ConsoleKey.LeftArrow)
            {
                _player.Location = new Point(_player.Location.X > 0 ? _player.Location.X - 1 : 0, _player.Location.Y );
            }
            else if (key == ConsoleKey.RightArrow)
            {
                _player.Location = new Point(_player.Location.X < 73 ? _player.Location.X + 1 : 73, _player.Location.Y);
            }
            else if (key == ConsoleKey.Spacebar)
            {
                PlayerFire();
            }
            else if (key == ConsoleKey.Escape)
            {
                System.Environment.Exit(0);
            }

            // Clear any queued keys
            while (Console.KeyAvailable)
            {
                Console.ReadKey(false);
            }
        }

        private static void PlayerFire()
        {
            if (_playerBullet.IsActive)
            {
                return;
            }

            _playerBullet.Fire(new Point(_player.Location.X +3, _player.Location.Y -1), Direction.Up);

        }

        private static void Draw()
        {
            _aliens.ForEach(a => a.Draw());
            _shields.ForEach(a => a.Draw());
            _player.Draw();
            _playerBullet.DeletePrevious();
            _playerBullet.Draw();

            $"Score: {_score}".Write(3, 1);
        }

    }
}
