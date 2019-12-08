using System;
using System.Drawing;

namespace SpaceInvaders
{
    // Ascii Space Invader design from https://github.com/macdice/ascii-invaders
    public static class CharacterFactory
    {
        public static AsciiSprite Create(CharacterType characterType, Point location)
        {
            return characterType switch
            {
                CharacterType.Saucer => new KillableAsciiSprite(
                    new[]
                    {
                        @"_/MM\_", 
                        @"qWAAWp"
                    }, null, location, ConsoleColor.DarkRed),
                CharacterType.Alien1 => new KillableAsciiSprite(
                    new[]
                    {
                        @"      ",
                        @" {@@} ", 
                        @" /""""\ ", 
                    },
                    new[]
                    {
                        @"      ",
                        @" {@@} ", 
                        @"  \/  ", 
                    }, location, ConsoleColor.DarkYellow),
                CharacterType.Alien2 => new KillableAsciiSprite(
                    new[]
                    {
                        @"      ",
                        @" dOOb ", 
                        @" ^/\^ ", 
                    },
                    new[]
                    {
                        @"      ",
                        " dOOb ", 
                        " ~||~ ", 
                    }, location, ConsoleColor.DarkYellow),
                CharacterType.Alien3 => new KillableAsciiSprite(
                    new[]
                    {
                        @"      ",
                        @" /MM\ ", 
                        @" |~~| ", 
                    },
                    new[]
                    {
                        @"      ",
                        @" /MM\ ", 
                        @" \~~/ ", 
                    }, location, ConsoleColor.DarkYellow),
                CharacterType.AlienExplode => new KillableAsciiSprite(
                    new[]
                    {
                        @"      ",
                        @" \||/ ", 
                        @" /||\ ", 
                    }, null, location, ConsoleColor.Red),
                CharacterType.Gunner => new KillableAsciiSprite(
                    new[]
                    {
                        "  mAm  ", 
                        " MAMAM "
                    }, null, location, ConsoleColor.Green),
                CharacterType.GunnerExplode => new KillableAsciiSprite(
                    new[]
                    {
                        " ,' %  ",
                        " ;&+,! ",
                        " -,+$! ",
                        " +  ^~ "
                    }, null, location, ConsoleColor.Red),
                CharacterType.Shield => new ShieldAsciiSprite(
                    new[]
                    {
                        @"/MMMMM\",
                        @"MMMMMMM",
                        @"MMM MMM",
                    }, null, location, ConsoleColor.Green),
                _ => throw new ArgumentOutOfRangeException(nameof(characterType))
            };
        }
    }
}