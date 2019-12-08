using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    public static class Extensions
    {
        public static void RemoveFirst<T>(this List<T> list)
        {
            if (list.Any())
            {
                list.Remove(list[0]);
            }
        }

        public static void RemoveLast<T>(this List<T> list)
        {
            if (list.Any())
            {
                list.Remove(list[^1]);
            }
        }

        public static Point Copy(this Point point)
        {
            return new Point(point.X, point.Y);
        }

        public static void Write(this IEnumerable<Point> points, string text)
        {
            foreach (var point in points)
            {
                Write(text, point.X, point.Y);
            }
        }

        public static void Write(this IEnumerable<Point> points, string text, ConsoleColor foreground, ConsoleColor background)
        {
            foreach (var point in points)
            {
                Write(text, point.X, point.Y, foreground, background);
            }
        }

        public static void Write(this Point point, string text)
        {
            Write(text, point.X, point.Y);
        }

        public static void Write(this Point point, string text, ConsoleColor foreground)
        {
            Write(text, point.X, point.Y, foreground, Console.BackgroundColor);
        }

        public static void Write(this Point point, string text, ConsoleColor foreground, ConsoleColor background)
        {
            Write(text, point.X, point.Y, foreground, background);
        }

        public static void Write(this string text, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(text);
        }

        public static void Write(this string text, int x, int y, ConsoleColor foreground, ConsoleColor background)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Write(text);
            Console.ResetColor();
        }

        public static int Left(this List<AsciiSprite> sprites)
        {
            return sprites.Select(s => s.Location.X).Concat(new[] {int.MaxValue}).Min();
        }

        public static int Right(this List<AsciiSprite> sprites)
        {
            return sprites.Select(s => s.Location.X).Concat(new[] { int.MinValue }).Max();
        }

    }
}
